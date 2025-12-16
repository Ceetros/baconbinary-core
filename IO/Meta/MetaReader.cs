using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BaconBinary.Core.Configurations;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Asset;
using BaconBinary.Core.IO.Dat;
using BaconBinary.Core.IO.Spr;
using BaconBinary.Core.Models;
using BaconBinary.Core.Security;

namespace BaconBinary.Core.IO.Meta
{
    public class MetaReader
    {
        public (DatFile, SprFile) ReadMetaFile(string path, string key)
        {
            Console.WriteLine($"Reading Meta File: {path}");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Meta file not found: {path}");

            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fileStream);

            byte[] signatureBytes = reader.ReadBytes(5);
            string signature = Encoding.ASCII.GetString(signatureBytes);
            if (signature != "BSUIT")
                throw new InvalidDataException("Invalid meta file signature.");

            byte formatVersion = reader.ReadByte();
            byte encryptionType = reader.ReadByte();
            byte[] iv = null;

            if (encryptionType == 1) iv = reader.ReadBytes(16);

            byte[] bodyBytes = reader.ReadBytes((int)(fileStream.Length - fileStream.Position));

            if (encryptionType == 1)
            {
                if (string.IsNullOrEmpty(key)) throw new ArgumentException("Encryption key required.");
                bodyBytes = CryptographyService.Decrypt(bodyBytes, key, iv);
            }

            using var bodyStream = new MemoryStream(bodyBytes);
            using var bodyReader = new BinaryReader(bodyStream);

            var datFile = new DatFile();
            var sprFile = new SprFile();

            try
            {
                byte major = bodyReader.ReadByte();
                byte minor = bodyReader.ReadByte();
                datFile.Version = $"{major}.{minor}";
                Console.WriteLine($"Version: {datFile.Version}");

                byte featureFlagsByte = bodyReader.ReadByte();
                var features = (FeatureFlags)featureFlagsByte;
                
                ClientFeatures.Extended = features.HasFlag(FeatureFlags.Extended);
                ClientFeatures.Transparency = features.HasFlag(FeatureFlags.Transparency);
                ClientFeatures.FrameDurations = features.HasFlag(FeatureFlags.FrameDurations);
                ClientFeatures.FrameGroups = features.HasFlag(FeatureFlags.FrameGroups);
                
                Console.WriteLine($"Features: Ext={ClientFeatures.Extended}, Trans={ClientFeatures.Transparency}, Dur={ClientFeatures.FrameDurations}, Grp={ClientFeatures.FrameGroups}");

                uint itemCount = bodyReader.ReadUInt32();
                uint outfitCount = bodyReader.ReadUInt32();
                uint effectCount = bodyReader.ReadUInt32();
                uint missileCount = bodyReader.ReadUInt32();
                Console.WriteLine($"Counts: Items={itemCount}, Outfits={outfitCount}, Effects={effectCount}, Missiles={missileCount}");

                uint assetFileCount = bodyReader.ReadUInt32();
                var assetReader = new AssetReader();
                string directory = Path.GetDirectoryName(path);

                for (int i = 0; i < assetFileCount; i++)
                {
                    byte fileIndex = bodyReader.ReadByte();
                    uint startId = bodyReader.ReadUInt32();
                    uint endId = bodyReader.ReadUInt32();

                    string assetFileName = $"{Path.GetFileNameWithoutExtension(path)}.asset{fileIndex}";
                    string assetPath = Path.Combine(directory, assetFileName);

                    var sprites = assetReader.ReadAssetData(assetPath, key);
                    
                    for (int s = 0; s < sprites.Count; s++)
                    {
                        uint globalId = startId + (uint)s;
                        var spriteData = sprites[s];
                        var sprite = new Sprite(globalId, ClientFeatures.Transparency);
                        if (spriteData != null)
                        {
                            sprite.CompressedPixels = spriteData;
                            sprite.Size = (ushort)spriteData.Length;
                        }
                        sprFile.Sprites[globalId] = sprite;
                    }
                }
                
                sprFile.SpriteCount = (uint)sprFile.Sprites.Count;
                Console.WriteLine($"Sprites Loaded: {sprFile.SpriteCount}");
                
                uint firstItemAddress = 0;
                if (itemCount > 0) firstItemAddress = bodyReader.ReadUInt32();
                
                Console.WriteLine($"First Item Address: {firstItemAddress}");
                
                if (firstItemAddress > 0)
                {
                    if (firstItemAddress > bodyStream.Length)
                    {
                        throw new InvalidDataException($"First item address {firstItemAddress} is beyond stream length {bodyStream.Length}");
                    }
                    bodyStream.Position = firstItemAddress;
                }
                else
                {
                    long lookupSize = (itemCount + outfitCount + effectCount + missileCount) * 4;
                    if (itemCount > 0) lookupSize -= 4;
                    bodyStream.Seek(lookupSize, SeekOrigin.Current);
                }
                
                ReadThings(bodyReader, itemCount, ThingCategory.Item, 100, datFile.Items);
                ReadThings(bodyReader, outfitCount, ThingCategory.Outfit, 1, datFile.Outfits);
                ReadThings(bodyReader, effectCount, ThingCategory.Effect, 1, datFile.Effects);
                ReadThings(bodyReader, missileCount, ThingCategory.Missile, 1, datFile.Missiles);
            }
            catch (Exception ex)
            {
                string msg = $"Error reading meta file at position {bodyStream.Position}. Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    msg += $" Inner: {ex.InnerException.Message}";
                }
                Console.WriteLine(msg);
                throw new InvalidDataException(msg, ex);
            }

            return (datFile, sprFile);
        }

        private void ReadThings(BinaryReader reader, uint count, ThingCategory category, uint startId, Dictionary<uint, ThingType> dictionary)
        {
            for (uint i = 0; i < count; i++)
            {
                uint currentId = startId + i;
                long startPos = reader.BaseStream.Position;
                
                try
                {
                    uint blockSize = reader.ReadUInt32(); 
                    
                    
                    var thing = new ThingType { Id = currentId, Category = category };
                    
                    uint commonFlagsVal = reader.ReadUInt32();
                    uint itemFlagsVal = reader.ReadUInt32();
                    
                    
                    var common = (CommonFlags)commonFlagsVal;
                    var itemFlags = (ItemFlags)itemFlagsVal;
                    
                    if (common.HasFlag(CommonFlags.AnimateAlways)) thing.AnimateAlways = true;
                    if (common.HasFlag(CommonFlags.LyingObject)) thing.IsLyingObject = true;
                    if (common.HasFlag(CommonFlags.TopEffect)) thing.IsTopEffect = true;
                    if (common.HasFlag(CommonFlags.Translucent)) thing.IsTranslucent = true;
                    if (common.HasFlag(CommonFlags.IsRotatable)) thing.IsRotatable = true;
                    if (common.HasFlag(CommonFlags.IsHangable)) thing.IsHangable = true;
                    if (common.HasFlag(CommonFlags.IsHookSouth)) thing.IsHookSouth = true;
                    if (common.HasFlag(CommonFlags.IsHookEast)) thing.IsHookEast = true;
                    if (common.HasFlag(CommonFlags.DontHide)) thing.DontHide = true;
                    if (common.HasFlag(CommonFlags.IgnoreLook)) thing.IgnoreLook = true;
                    if (common.HasFlag(CommonFlags.IsWrappable)) thing.IsWrappable = true;
                    if (common.HasFlag(CommonFlags.IsUnwrappable)) thing.IsUnwrappable = true;
                    if (common.HasFlag(CommonFlags.IsUsable)) thing.IsUsable = true;
                    if (common.HasFlag(CommonFlags.IsVertical)) thing.IsVertical = true;
                    if (common.HasFlag(CommonFlags.IsHorizontal)) thing.IsHorizontal = true;

                    if (itemFlags.HasFlag(ItemFlags.IsGround)) thing.IsGround = true;
                    if (itemFlags.HasFlag(ItemFlags.IsGroundBorder)) thing.IsGroundBorder = true;
                    if (itemFlags.HasFlag(ItemFlags.IsOnBottom)) thing.IsOnBottom = true;
                    if (itemFlags.HasFlag(ItemFlags.IsOnTop)) thing.IsOnTop = true;
                    if (itemFlags.HasFlag(ItemFlags.IsContainer)) thing.IsContainer = true;
                    if (itemFlags.HasFlag(ItemFlags.IsStackable)) thing.IsStackable = true;
                    if (itemFlags.HasFlag(ItemFlags.ForceUse)) thing.ForceUse = true;
                    if (itemFlags.HasFlag(ItemFlags.IsMultiUse)) thing.IsMultiUse = true;
                    if (itemFlags.HasFlag(ItemFlags.IsWritable)) thing.IsWritable = true;
                    if (itemFlags.HasFlag(ItemFlags.IsReadable)) thing.IsReadable = true;
                    if (itemFlags.HasFlag(ItemFlags.IsFluidContainer)) thing.IsFluidContainer = true;
                    if (itemFlags.HasFlag(ItemFlags.IsFluid)) thing.IsFluid = true;
                    if (itemFlags.HasFlag(ItemFlags.IsUnpassable)) thing.IsUnpassable = true;
                    if (itemFlags.HasFlag(ItemFlags.IsUnmoveable)) thing.IsUnmoveable = true;
                    if (itemFlags.HasFlag(ItemFlags.BlockMissile)) thing.BlockMissile = true;
                    if (itemFlags.HasFlag(ItemFlags.BlockPathfind)) thing.BlockPathfind = true;
                    if (itemFlags.HasFlag(ItemFlags.NoMoveAnimation)) thing.NoMoveAnimation = true;
                    if (itemFlags.HasFlag(ItemFlags.IsPickupable)) thing.IsPickupable = true;
                    if (itemFlags.HasFlag(ItemFlags.IsFullGround)) thing.IsFullGround = true;
                    if (itemFlags.HasFlag(ItemFlags.IsMarketItem)) thing.IsMarketItem = true;

                    if (common.HasFlag(CommonFlags.HasOffset))
                    {
                        thing.HasOffset = true;
                        thing.OffsetX = reader.ReadInt16();
                        thing.OffsetY = reader.ReadInt16();
                    }
                    if (common.HasFlag(CommonFlags.HasElevation))
                    {
                        thing.HasElevation = true;
                        thing.Elevation = reader.ReadUInt16();
                    }
                    if (common.HasFlag(CommonFlags.HasLight))
                    {
                        thing.HasLight = true;
                        thing.LightLevel = reader.ReadUInt16();
                        thing.LightColor = reader.ReadUInt16();
                    }
                    if (common.HasFlag(CommonFlags.IsMiniMap))
                    {
                        thing.IsMiniMap = true;
                        thing.MiniMapColor = reader.ReadUInt16();
                    }
                    if (common.HasFlag(CommonFlags.IsLensHelp))
                    {
                        thing.IsLensHelp = true;
                        thing.LensHelp = reader.ReadUInt16();
                    }
                    if (common.HasFlag(CommonFlags.IsCloth))
                    {
                        thing.IsCloth = true;
                        thing.ClothSlot = reader.ReadUInt16();
                    }
                    if (common.HasFlag(CommonFlags.HasDefaultAction))
                    {
                        thing.HasDefaultAction = true;
                        thing.DefaultAction = reader.ReadUInt16();
                    }

                    if (itemFlags.HasFlag(ItemFlags.IsGround)) thing.GroundSpeed = reader.ReadUInt16();
                    if (itemFlags.HasFlag(ItemFlags.IsWritable)) thing.MaxTextLength = reader.ReadUInt16();
                    if (itemFlags.HasFlag(ItemFlags.IsReadable)) thing.MaxTextLength = reader.ReadUInt16();
                    if (itemFlags.HasFlag(ItemFlags.IsMarketItem))
                    {
                        thing.MarketCategory = reader.ReadUInt16();
                        thing.MarketTradeAs = reader.ReadUInt16();
                        thing.MarketShowAs = reader.ReadUInt16();
                        ushort nameLen = reader.ReadUInt16();
                        thing.MarketName = Encoding.UTF8.GetString(reader.ReadBytes(nameLen));
                        thing.MarketRestrictProfession = reader.ReadUInt16();
                        thing.MarketRestrictLevel = reader.ReadUInt16();
                    }
                    
                    byte groupCount = reader.ReadByte();
                    
                    for (int g = 0; g < groupCount; g++)
                    {
                        var group = new FrameGroup();
                        group.Width = reader.ReadByte();
                        group.Height = reader.ReadByte();
                        group.Layers = reader.ReadByte();
                        group.PatternX = reader.ReadByte();
                        group.PatternY = reader.ReadByte();
                        group.PatternZ = reader.ReadByte();
                        group.Frames = reader.ReadByte();
                        

                        if (group.Frames > 1)
                        {
                            if (ClientFeatures.FrameDurations)
                            {
                                group.IsAnimation = true;
                                group.AnimationMode = reader.ReadByte();
                                group.LoopCount = reader.ReadUInt32();
                                group.StartFrame = reader.ReadByte();
                                
                                for (int f = 0; f < group.Frames; f++)
                                {
                                    group.FrameDurations.Add(new FrameDuration((int)reader.ReadUInt32(), (int)reader.ReadUInt32()));
                                }
                            }
                        }

                        uint spriteCount = reader.ReadUInt32();

                        group.SpriteIDs = new uint[spriteCount];
                        for (int s = 0; s < spriteCount; s++)
                        {
                            group.SpriteIDs[s] = reader.ReadUInt32();
                        }
                        
                        thing.FrameGroups[(FrameGroupType)g] = group;
                    }

                    dictionary[currentId] = thing;
                    
                    if (reader.BaseStream.Position != startPos + blockSize + 4)
                    {
                        if (reader.BaseStream.Position != startPos + blockSize + 4)
                        {
                            reader.BaseStream.Position = startPos + blockSize + 4;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"Error reading {category} ID {currentId} at pos {startPos}. Error: {ex.Message}";
                    throw new InvalidDataException(msg, ex);
                }
            }
        }
    }
}
