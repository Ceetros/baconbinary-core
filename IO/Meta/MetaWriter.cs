using System.Text;
using BaconBinary.Core.Configurations;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Dat;
using BaconBinary.Core.IO.Spr;
using BaconBinary.Core.Models;
using BaconBinary.Core.Security;

namespace BaconBinary.Core.IO.Meta
{
    public class MetaWriter
    {
        public void WriteMetaFile(DatFile datFile, SprFile sprFile, string path, bool encrypt, string key)
        {
            var bodyStream = new MemoryStream();
            using (var bodyWriter = new BinaryWriter(bodyStream))
            {
                WriteBody(bodyWriter, datFile, sprFile, path);
            }
            
            byte[] bodyBytes = bodyStream.ToArray();

            byte[] iv = null;
            if (encrypt)
            {
                bodyBytes = CryptographyService.Encrypt(bodyBytes, key, out iv);
            }

            using (var finalStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(finalStream))
            {
                writer.Write(Encoding.ASCII.GetBytes("BSUIT"));
                writer.Write((byte)1); // Format Version
                writer.Write(encrypt ? (byte)1 : (byte)0); // Encryption Type
                
                if (encrypt)
                {
                    writer.Write(iv);
                }
                
                writer.Write(bodyBytes);
            }
        }

        private void WriteBody(BinaryWriter writer, DatFile datFile, SprFile sprFile, string metaPath)
        {
            ushort versionNumber = ushort.Parse(datFile.Version.Replace(".", ""));
            writer.Write((byte)(versionNumber / 100));
            writer.Write((byte)(versionNumber % 100));
            
            FeatureFlags features = FeatureFlags.None;
            if (ClientFeatures.Extended) features |= FeatureFlags.Extended;
            if (ClientFeatures.Transparency) features |= FeatureFlags.Transparency;
            if (ClientFeatures.FrameDurations) features |= FeatureFlags.FrameDurations;
            if (ClientFeatures.FrameGroups) features |= FeatureFlags.FrameGroups;
            
            writer.Write((byte)features);

            var itemData = new MemoryStream();
            var outfitData = new MemoryStream();
            var effectData = new MemoryStream();
            var missileData = new MemoryStream();

            var itemAddresses = WriteThingsToStream(itemData, datFile.Items.Values);
            var outfitAddresses = WriteThingsToStream(outfitData, datFile.Outfits.Values);
            var effectAddresses = WriteThingsToStream(effectData, datFile.Effects.Values);
            var missileAddresses = WriteThingsToStream(missileData, datFile.Missiles.Values);

            writer.Write((uint)itemAddresses.Count);
            writer.Write((uint)outfitAddresses.Count);
            writer.Write((uint)effectAddresses.Count);
            writer.Write((uint)missileAddresses.Count);

            const int spritesPerFile = 1000000;
            uint numAssetFiles = (uint)Math.Ceiling((double)sprFile.SpriteCount / spritesPerFile);
            writer.Write(numAssetFiles);

            for (int i = 0; i < numAssetFiles; i++)
            {
                uint startId = (uint)(i * spritesPerFile) + 1;
                uint endId = Math.Min((uint)((i + 1) * spritesPerFile), sprFile.SpriteCount);
                
                writer.Write((byte)i);
                writer.Write(startId);
                writer.Write(endId);
            }

            uint currentOffset = (uint)writer.BaseStream.Position;
            
            uint lookupTableSize = (uint)((itemAddresses.Count + outfitAddresses.Count + effectAddresses.Count + missileAddresses.Count) * 4);
            uint dataStartOffset = currentOffset + lookupTableSize;

            foreach (var address in itemAddresses) { writer.Write(dataStartOffset + address); }
            dataStartOffset += (uint)itemData.Length;
            
            foreach (var address in outfitAddresses) { writer.Write(dataStartOffset + address); }
            dataStartOffset += (uint)outfitData.Length;

            foreach (var address in effectAddresses) { writer.Write(dataStartOffset + address); }
            dataStartOffset += (uint)effectData.Length;

            foreach (var address in missileAddresses) { writer.Write(dataStartOffset + address); }

            writer.Write(itemData.ToArray());
            writer.Write(outfitData.ToArray());
            writer.Write(effectData.ToArray());
            writer.Write(missileData.ToArray());
        }

        private List<uint> WriteThingsToStream(MemoryStream stream, IEnumerable<ThingType> things)
        {
            var addresses = new List<uint>();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                foreach (var thing in things)
                {
                    addresses.Add((uint)stream.Position);
                    WriteThing(writer, thing);
                }
            }
            return addresses;
        }

        private void WriteThing(BinaryWriter writer, ThingType thing)
        {
            var thingStream = new MemoryStream();
            using (var thingWriter = new BinaryWriter(thingStream))
            {
                CommonFlags common = CommonFlags.None;
                ItemFlags itemFlags = ItemFlags.None;

                if (thing.AnimateAlways) common |= CommonFlags.AnimateAlways;
                if (thing.IsLyingObject) common |= CommonFlags.LyingObject;
                if (thing.IsTopEffect) common |= CommonFlags.TopEffect;
                if (thing.IsTranslucent) common |= CommonFlags.Translucent;
                if (thing.HasOffset) common |= CommonFlags.HasOffset;
                if (thing.HasElevation) common |= CommonFlags.HasElevation;
                if (thing.IsRotatable) common |= CommonFlags.IsRotatable;
                if (thing.IsHangable) common |= CommonFlags.IsHangable;
                if (thing.IsHookSouth) common |= CommonFlags.IsHookSouth;
                if (thing.IsHookEast) common |= CommonFlags.IsHookEast;
                if (thing.HasLight) common |= CommonFlags.HasLight;
                if (thing.DontHide) common |= CommonFlags.DontHide;
                if (thing.IsMiniMap) common |= CommonFlags.IsMiniMap;
                if (thing.IsLensHelp) common |= CommonFlags.IsLensHelp;
                if (thing.IgnoreLook) common |= CommonFlags.IgnoreLook;
                if (thing.IsCloth) common |= CommonFlags.IsCloth;
                if (thing.HasDefaultAction) common |= CommonFlags.HasDefaultAction;
                if (thing.IsWrapable) common |= CommonFlags.IsWrappable;
                if (thing.IsUnwrapable) common |= CommonFlags.IsUnwrappable;
                if (thing.IsUsable) common |= CommonFlags.IsUsable;
                if (thing.IsVertical) common |= CommonFlags.IsVertical;
                if (thing.IsHorizontal) common |= CommonFlags.IsHorizontal;

                if (thing.IsGround) itemFlags |= ItemFlags.IsGround;
                if (thing.IsGroundBorder) itemFlags |= ItemFlags.IsGroundBorder;
                if (thing.IsOnBottom) itemFlags |= ItemFlags.IsOnBottom;
                if (thing.IsOnTop) itemFlags |= ItemFlags.IsOnTop;
                if (thing.IsContainer) itemFlags |= ItemFlags.IsContainer;
                if (thing.IsStackable) itemFlags |= ItemFlags.IsStackable;
                if (thing.ForceUse) itemFlags |= ItemFlags.ForceUse;
                if (thing.IsMultiUse) itemFlags |= ItemFlags.IsMultiUse;
                if (thing.IsWritable) itemFlags |= ItemFlags.IsWritable;
                if (thing.IsReadable) itemFlags |= ItemFlags.IsReadable;
                if (thing.IsFluidContainer) itemFlags |= ItemFlags.IsFluidContainer;
                if (thing.IsFluid) itemFlags |= ItemFlags.IsFluid;
                if (thing.IsUnpassable) itemFlags |= ItemFlags.IsUnpassable;
                if (thing.IsUnmoveable) itemFlags |= ItemFlags.IsUnmoveable;
                if (thing.BlockMissile) itemFlags |= ItemFlags.BlockMissile;
                if (thing.BlockPathfind) itemFlags |= ItemFlags.BlockPathfind;
                if (thing.NoMoveAnimation) itemFlags |= ItemFlags.NoMoveAnimation;
                if (thing.IsPickupable) itemFlags |= ItemFlags.IsPickupable;
                if (thing.IsFullGround) itemFlags |= ItemFlags.IsFullGround;
                if (thing.IsMarketItem) itemFlags |= ItemFlags.IsMarketItem;

                thingWriter.Write((uint)common);
                thingWriter.Write((uint)itemFlags);

                if (thing.HasOffset) { thingWriter.Write(thing.OffsetX); thingWriter.Write(thing.OffsetY); }
                if (thing.HasElevation) thingWriter.Write(thing.Elevation);
                if (thing.HasLight) { thingWriter.Write(thing.LightLevel); thingWriter.Write(thing.LightColor); }
                if (thing.IsMiniMap) thingWriter.Write(thing.MiniMapColor);
                if (thing.IsLensHelp) thingWriter.Write(thing.LensHelp);
                if (thing.IsCloth) thingWriter.Write(thing.ClothSlot);
                if (thing.HasDefaultAction) thingWriter.Write(thing.DefaultAction);
                if (thing.IsGround) thingWriter.Write(thing.GroundSpeed);
                if (thing.IsWritable) thingWriter.Write(thing.MaxTextLength);
                if (thing.IsReadable) thingWriter.Write(thing.MaxTextLength);
                if (thing.IsMarketItem)
                {
                    thingWriter.Write(thing.MarketCategory);
                    thingWriter.Write(thing.MarketTradeAs);
                    thingWriter.Write(thing.MarketShowAs);
                    byte[] nameBytes = Encoding.UTF8.GetBytes(thing.MarketName ?? "");
                    thingWriter.Write((ushort)nameBytes.Length);
                    thingWriter.Write(nameBytes);
                    thingWriter.Write(thing.MarketRestrictProfession);
                    thingWriter.Write(thing.MarketRestrictLevel);
                }

                thingWriter.Write((byte)thing.FrameGroups.Count);
                foreach (var group in thing.FrameGroups.Values)
                {
                    thingWriter.Write(group.Width);
                    thingWriter.Write(group.Height);
                    thingWriter.Write(group.Layers);
                    thingWriter.Write(group.PatternX);
                    thingWriter.Write(group.PatternY);
                    thingWriter.Write(group.PatternZ);
                    thingWriter.Write(group.Frames);

                    if (group.Frames > 1)
                    {
                        if (ClientFeatures.FrameDurations)
                        {
                            thingWriter.Write(group.AnimationMode);
                            thingWriter.Write((uint)group.LoopCount);
                            thingWriter.Write(group.StartFrame);
                            foreach (var fd in group.FrameDurations)
                            {
                                thingWriter.Write((uint)fd.Minimum);
                                thingWriter.Write((uint)fd.Maximum);
                            }
                        }
                    }

                    thingWriter.Write((uint)group.SpriteIDs.Length);
                    foreach (var spriteId in group.SpriteIDs)
                    {
                        thingWriter.Write(spriteId);
                    }
                }
            }
            
            byte[] thingData = thingStream.ToArray();
            writer.Write((uint)thingData.Length); // Changed to uint
            writer.Write(thingData);
        }
    }
}
