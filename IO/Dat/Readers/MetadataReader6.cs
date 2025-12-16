using System.IO;
using System.Text;
using BaconBinary.Core.Configurations;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Dat.Interfaces;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat.Readers
{
    public class MetadataReader6 : IMetadataReader
    {
        public enum DatFlags : uint
        {
            Ground = 0x00,
            GroundBorder = 0x01,
            OnBottom = 0x02,
            OnTop = 0x03,
            Container = 0x04,
            Stackable = 0x05,
            ForceUse = 0x06,
            MultiUse = 0x07,
            Writable = 0x08,
            WritableOnce = 0x09,
            FluidContainer = 0x0A,
            Fluid = 0x0B,
            Unpassable = 0x0C,
            Unmoveable = 0x0D,
            BlockMissile = 0x0E,
            BlockPathfind = 0x0F,
            NoMoveAnimation = 0x10,
            Pickupable = 0x11,
            Hangable = 0x12,
            Vertical = 0x13,
            Horizontal = 0x14,
            Rotatable = 0x15,
            HasLight = 0x16,
            DontHide = 0x17,
            Translucent = 0x18,
            HasOffset = 0x19,
            HasElevation = 0x1A,
            LyingObject = 0x1B,
            AnimateAlways = 0x1C,
            MiniMap = 0x1D,
            LensHelp = 0x1E,
            FullGround = 0x1F,
            IgnoreLook = 0x20,
            Cloth = 0x21,
            MarketItem = 0x22,
            DefaultAction = 0x23,
            Wrappable = 0x24,
            Unwrappable = 0x25,
            TopEffect = 0x26,

            Usable = 0xFE,
            LastFlag = 0xFF
        }

       public bool ReadProperties(ClientBinaryReader reader, ThingType type)
        {
            uint flag = 0;
            
            while (flag < (uint)DatFlags.LastFlag)
            {
                uint previousFlag = flag;
                
                flag = reader.ReadByte();
                
                if (flag == (uint)DatFlags.LastFlag)
                    return true;

                switch ((DatFlags)flag)
                {
                    case DatFlags.Ground:
                        type.IsGround = true;
                        type.GroundSpeed = reader.ReadU16();
                        break;

                    case DatFlags.GroundBorder: type.IsGroundBorder = true; break;
                    case DatFlags.OnBottom: type.IsOnBottom = true; break;
                    case DatFlags.OnTop: type.IsOnTop = true; break;
                    case DatFlags.Container: type.IsContainer = true; break;
                    case DatFlags.Stackable: type.IsStackable = true; break;
                    case DatFlags.ForceUse: type.ForceUse = true; break;
                    case DatFlags.MultiUse: type.IsMultiUse = true; break;

                    case DatFlags.Writable:
                        type.IsWritable = true;
                        type.MaxTextLength = reader.ReadU16();
                        break;

                    case DatFlags.WritableOnce:
                        type.IsWritableOnce = true;
                        type.MaxTextLength = reader.ReadU16();
                        break;

                    case DatFlags.FluidContainer: type.IsFluidContainer = true; break;
                    case DatFlags.Fluid: type.IsFluid = true; break;
                    case DatFlags.Unpassable: type.IsUnpassable = true; break;
                    case DatFlags.Unmoveable: type.IsUnmoveable = true; break;
                    case DatFlags.BlockMissile: type.BlockMissile = true; break;
                    case DatFlags.BlockPathfind: type.BlockPathfind = true; break;
                    
                    case DatFlags.NoMoveAnimation: 
                        type.NoMoveAnimation = true; 
                        break;

                    case DatFlags.Pickupable: type.IsPickupable = true; break;
                    case DatFlags.Hangable: type.IsHangable = true; break;
                    case DatFlags.Vertical: type.IsHookEast = true; break;
                    case DatFlags.Horizontal: type.IsHookSouth = true; break;
                    case DatFlags.Rotatable: type.IsRotatable = true; break;

                    case DatFlags.HasLight:
                        type.HasLight = true;
                        type.LightLevel = reader.ReadU16();
                        type.LightColor = reader.ReadU16();
                        break;

                    case DatFlags.DontHide: type.DontHide = true; break;
                    case DatFlags.Translucent: type.IsTranslucent = true; break;

                    case DatFlags.HasOffset:
                        type.HasOffset = true;
                        type.OffsetX = reader.ReadInt16();
                        type.OffsetY = reader.ReadInt16();
                        break;

                    case DatFlags.HasElevation:
                        type.HasElevation = true;
                        type.Elevation = reader.ReadU16();
                        break;

                    case DatFlags.LyingObject: type.IsLyingObject = true; break;
                    case DatFlags.AnimateAlways: type.AnimateAlways = true; break;

                    case DatFlags.MiniMap:
                        type.IsMiniMap = true;
                        type.MiniMapColor = reader.ReadU16();
                        break;

                    case DatFlags.LensHelp:
                        type.IsLensHelp = true;
                        type.LensHelp = reader.ReadU16();
                        break;

                    case DatFlags.FullGround: type.IsFullGround = true; break;
                    case DatFlags.IgnoreLook: type.IgnoreLook = true; break;

                    case DatFlags.Cloth:
                        type.IsCloth = true;
                        type.ClothSlot = reader.ReadU16();
                        break;

                    case DatFlags.MarketItem:
                        type.IsMarketItem = true;
                        type.MarketCategory = reader.ReadU16();
                        type.MarketTradeAs = reader.ReadU16();
                        type.MarketShowAs = reader.ReadU16();
                        
                        ushort nameLen = reader.ReadU16();
                        if (nameLen > 0)
                        {
                            byte[] stringBytes = reader.ReadBytes(nameLen);
                            type.MarketName = Encoding.Latin1.GetString(stringBytes);
                        }
                        else
                        {
                            type.MarketName = string.Empty;
                        }

                        type.MarketRestrictProfession = reader.ReadU16();
                        type.MarketRestrictLevel = reader.ReadU16();
                        break;

                    case DatFlags.DefaultAction:
                        type.HasDefaultAction = true;
                        type.DefaultAction = reader.ReadU16();
                        break;

                    case DatFlags.Wrappable: type.IsWrapable = true; break;
                    case DatFlags.Unwrappable: type.IsUnwrapable = true; break;
                    case DatFlags.TopEffect: type.IsTopEffect = true; break;
                    case DatFlags.Usable: type.IsUsable = true; break;

                    default:
                        string msg = $"readUnknownFlag: Flag=0x{flag:X} (Prev=0x{previousFlag:X}) | Category={type.Category} | ID={type.Id} | Pos={reader.BaseStream.Position}";
                        throw new InvalidDataException(msg);
                }
            }

            return true;
        }

        public void ReadTexturePatterns(ClientBinaryReader reader, ThingType type)
        {
            bool extended = ClientFeatures.Extended;
            bool frameDurations = ClientFeatures.FrameDurations;
            bool frameGroups = ClientFeatures.FrameGroups;

            int groupCount = 1;
            
            if (frameGroups && type.Category == ThingCategory.Outfit)
            {
                groupCount = reader.ReadByte();
            }

            for (uint groupType = 0; groupType < groupCount; groupType++)
            {
                if (frameGroups && type.Category == ThingCategory.Outfit)
                {
                    reader.ReadByte();
                }

                var frameGroup = new FrameGroup();
                frameGroup.Width = reader.ReadByte();
                frameGroup.Height = reader.ReadByte();
                
                if (frameGroup.Width > 1 || frameGroup.Height > 1)
                    reader.ReadByte();
                else
                    _ = 32;

                frameGroup.Layers = reader.ReadByte();
                frameGroup.PatternX = reader.ReadByte();
                frameGroup.PatternY = reader.ReadByte();
                frameGroup.PatternZ = reader.ReadByte();
                frameGroup.Frames = reader.ReadByte();


                if (frameGroup.Frames > 1)
                {
                    frameGroup.IsAnimation = true;
                    
                    if (frameDurations)
                    {
                        frameGroup.FrameDurations = new List<FrameDuration>();
                        frameGroup.AnimationMode = reader.ReadByte();
                        frameGroup.LoopCount = (uint)reader.ReadInt32();
                        frameGroup.StartFrame = reader.ReadByte();

                        for (int f = 0; f < frameGroup.Frames; f++)
                        {
                            uint min = reader.ReadUInt32();
                            uint max = reader.ReadUInt32();
                            frameGroup.FrameDurations.Add(new FrameDuration((int)min, (int)max));
                        }
                    }
                    else
                    {
                        for (int f = 0; f < frameGroup.Frames; f++)
                        {
                            frameGroup.FrameDurations.Add(new FrameDuration(100, 100));
                        }
                    }
                }
                
                int totalSprites = frameGroup.Width * frameGroup.Height * frameGroup.Layers * frameGroup.PatternX * frameGroup.PatternY * frameGroup.PatternZ * frameGroup.Frames;

                frameGroup.SpriteIDs = new uint[totalSprites];

                for (int s = 0; s < totalSprites; s++)
                {
                    if (extended)
                        frameGroup.SpriteIDs[s] = reader.ReadUInt32();
                    else
                        frameGroup.SpriteIDs[s] = reader.ReadUInt16();
                }
                
                if (frameGroups && type.Category == ThingCategory.Outfit)
                {
                    if (groupType == 0) type.FrameGroups[FrameGroupType.Default] = frameGroup;
                    else type.FrameGroups[(FrameGroupType)groupType] = frameGroup;
                }
                else
                {
                    type.FrameGroups[FrameGroupType.Default] = frameGroup;
                }
            }
        }

        public void WriteThing(BinaryWriter writer, ThingType thing)
        {
            if (thing.IsGround)
            {
                writer.Write((byte)DatFlags.Ground);
                writer.Write((ushort)thing.GroundSpeed);
            }
            
            if (thing.IsGroundBorder) writer.Write((byte)DatFlags.GroundBorder);
            if (thing.IsOnBottom) writer.Write((byte)DatFlags.OnBottom);
            if (thing.IsOnTop) writer.Write((byte)DatFlags.OnTop);
            if (thing.IsContainer) writer.Write((byte)DatFlags.Container);
            if (thing.IsStackable) writer.Write((byte)DatFlags.Stackable);
            if (thing.ForceUse) writer.Write((byte)DatFlags.ForceUse);
            if (thing.IsMultiUse) writer.Write((byte)DatFlags.MultiUse);

            if (thing.IsWritable)
            {
                writer.Write((byte)DatFlags.Writable);
                writer.Write((ushort)thing.MaxTextLength);
            }

            if (thing.IsReadable)
            {
                writer.Write((byte)DatFlags.WritableOnce);
                writer.Write((ushort)thing.MaxTextLength);
            }

            if (thing.IsFluidContainer) writer.Write((byte)DatFlags.FluidContainer);
            if (thing.IsFluid) writer.Write((byte)DatFlags.Fluid);
            if (thing.IsUnpassable) writer.Write((byte)DatFlags.Unpassable);
            if (thing.IsUnmoveable) writer.Write((byte)DatFlags.Unmoveable);
            if (thing.BlockMissile) writer.Write((byte)DatFlags.BlockMissile);
            if (thing.BlockPathfind) writer.Write((byte)DatFlags.BlockPathfind);
            if (thing.NoMoveAnimation) writer.Write((byte)DatFlags.NoMoveAnimation);
            if (thing.IsPickupable) writer.Write((byte)DatFlags.Pickupable);
            if (thing.IsHangable) writer.Write((byte)DatFlags.Hangable);
            if (thing.IsVertical) writer.Write((byte)DatFlags.Vertical);
            if (thing.IsHorizontal) writer.Write((byte)DatFlags.Horizontal);
            if (thing.IsRotatable) writer.Write((byte)DatFlags.Rotatable);

            if (thing.HasLight)
            {
                writer.Write((byte)DatFlags.HasLight);
                writer.Write((ushort)thing.LightLevel);
                writer.Write((ushort)thing.LightColor);
            }

            if (thing.DontHide) writer.Write((byte)DatFlags.DontHide);
            if (thing.IsTranslucent) writer.Write((byte)DatFlags.Translucent);
            if (thing.HasOffset)
            {
                writer.Write((byte)DatFlags.HasOffset);
                writer.Write((short)thing.OffsetX);
                writer.Write((short)thing.OffsetY);
            }
            
            if (thing.HasElevation)
            {
                writer.Write((byte)DatFlags.HasElevation);
                writer.Write(thing.Elevation);
            }
            
            if (thing.IsLyingObject) writer.Write((byte)DatFlags.LyingObject);
            if (thing.AnimateAlways) writer.Write((byte)DatFlags.AnimateAlways);
            if (thing.IsMiniMap)
            {
                writer.Write((byte)DatFlags.MiniMap);
                writer.Write((ushort)thing.MiniMapColor);
            }
            
            if (thing.IsLensHelp)
            {
                writer.Write((byte)DatFlags.LensHelp);
                writer.Write((ushort)thing.LensHelp);
            }
            
            if (thing.IsFullGround) writer.Write((byte)DatFlags.FullGround);
            if (thing.IgnoreLook) writer.Write((byte)DatFlags.IgnoreLook);
            if (thing.IsCloth)
            {
                writer.Write((byte)DatFlags.Cloth);
                writer.Write(thing.ClothSlot);
            }

            if (thing.IsMarketItem)
            {
                writer.Write((byte)DatFlags.MarketItem);
                writer.Write((ushort)thing.MarketCategory);
                writer.Write((ushort)thing.MarketTradeAs);
                writer.Write((ushort)thing.MarketShowAs);
                var nameBytes = Encoding.Latin1.GetBytes(thing.MarketName);
                writer.Write((ushort)nameBytes.Length);
                writer.Write(nameBytes);
                writer.Write((ushort)thing.MarketRestrictProfession);
                writer.Write((ushort)thing.MarketRestrictLevel);
            }
            
            if (thing.HasDefaultAction)
            {
                writer.Write((byte)DatFlags.DefaultAction);
                writer.Write((ushort)thing.DefaultAction);
            }
            if (thing.IsWrapable) writer.Write((byte)DatFlags.Wrappable);
            if (thing.IsUnwrappable) writer.Write((byte)DatFlags.Unwrappable);
            if (thing.IsUsable) writer.Write((byte)DatFlags.Usable);

            writer.Write((byte)DatFlags.LastFlag);


            byte groupCount = 1;
            if (ClientFeatures.FrameGroups && thing.Category == ThingCategory.Outfit)
            {
                groupCount = (byte)thing.FrameGroups.Count;
                writer.Write((byte)groupCount);
            }

            for (int i = 0; i < groupCount; i++)
            {
                var group = thing.FrameGroups[(FrameGroupType)i];
                if (ClientFeatures.FrameGroups && thing.Category == ThingCategory.Outfit)
                {
                    writer.Write((byte)0);
                }

                writer.Write((byte)group.Width);
                writer.Write((byte)group.Height);

                if (group.Width > 1 || group.Height > 1)
                    writer.Write((byte)0);

                writer.Write((byte)group.Layers);
                writer.Write((byte)group.PatternX);
                writer.Write((byte)group.PatternY);
                writer.Write((byte)group.PatternZ);
                writer.Write((byte)group.Frames);

                if (group.Frames > 1)
                {
                    if (ClientFeatures.FrameDurations)
                    {
                        writer.Write((byte)group.AnimationMode);
                        writer.Write((uint)group.LoopCount);
                        writer.Write((byte)group.StartFrame);
                        for (int f = 0; f < group.Frames; f++)
                        {
                            FrameDuration? fd = group.FrameDurations.Count >= f ? group.FrameDurations[f] : null;
                            if (fd == null)
                                continue;
                            writer.Write((uint)fd.Value.Minimum);
                            writer.Write((uint)fd.Value.Maximum);
                        }
                    }
                }

                foreach (var spriteId in group.SpriteIDs) writer.Write(ClientFeatures.Extended ? (uint)spriteId : (ushort)spriteId);
            }
        }
                
    }
}