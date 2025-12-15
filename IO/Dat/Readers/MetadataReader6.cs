using System.IO;
using System.Text;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Dat.Interfaces;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat.Readers
{
    public class MetadataReader6 : IMetadataReader
    {
        private enum _datFlags : uint
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
            
            while (flag < (uint)_datFlags.LastFlag)
            {
                uint previousFlag = flag;
                
                flag = reader.ReadByte();
                
                if (flag == (uint)_datFlags.LastFlag)
                    return true;

                switch ((_datFlags)flag)
                {
                    case _datFlags.Ground:
                        type.IsGround = true;
                        type.GroundSpeed = reader.ReadU16();
                        break;

                    case _datFlags.GroundBorder: type.IsGroundBorder = true; break;
                    case _datFlags.OnBottom: type.IsOnBottom = true; break;
                    case _datFlags.OnTop: type.IsOnTop = true; break;
                    case _datFlags.Container: type.IsContainer = true; break;
                    case _datFlags.Stackable: type.IsStackable = true; break;
                    case _datFlags.ForceUse: type.ForceUse = true; break;
                    case _datFlags.MultiUse: type.MultiUse = true; break;

                    case _datFlags.Writable:
                        type.IsWritable = true;
                        type.MaxTextLength = reader.ReadU16();
                        break;

                    case _datFlags.WritableOnce:
                        type.IsWritableOnce = true;
                        type.MaxTextLength = reader.ReadU16();
                        break;

                    case _datFlags.FluidContainer: type.IsFluidContainer = true; break;
                    case _datFlags.Fluid: type.IsFluid = true; break;
                    case _datFlags.Unpassable: type.IsUnpassable = true; break;
                    case _datFlags.Unmoveable: type.IsUnmoveable = true; break;
                    case _datFlags.BlockMissile: type.BlockMissile = true; break;
                    case _datFlags.BlockPathfind: type.BlockPathfind = true; break;
                    
                    case _datFlags.NoMoveAnimation: 
                        type.NoMoveAnimation = true; 
                        break;

                    case _datFlags.Pickupable: type.IsPickupable = true; break;
                    case _datFlags.Hangable: type.IsHangable = true; break;
                    case _datFlags.Vertical: type.IsVertical = true; break;
                    case _datFlags.Horizontal: type.IsHorizontal = true; break;
                    case _datFlags.Rotatable: type.IsRotatable = true; break;

                    case _datFlags.HasLight:
                        type.HasLight = true;
                        type.LightLevel = reader.ReadU16();
                        type.LightColor = reader.ReadU16();
                        break;

                    case _datFlags.DontHide: type.DontHide = true; break;
                    case _datFlags.Translucent: type.IsTranslucent = true; break;

                    case _datFlags.HasOffset:
                        type.HasOffset = true;
                        type.OffsetX = reader.ReadInt16();
                        type.OffsetY = reader.ReadInt16();
                        break;

                    case _datFlags.HasElevation:
                        type.HasElevation = true;
                        type.Elevation = reader.ReadU16();
                        break;

                    case _datFlags.LyingObject: type.IsLyingObject = true; break;
                    case _datFlags.AnimateAlways: type.AnimateAlways = true; break;

                    case _datFlags.MiniMap:
                        type.IsMiniMap = true;
                        type.MiniMapColor = reader.ReadU16();
                        break;

                    case _datFlags.LensHelp:
                        type.IsLensHelp = true;
                        type.LensHelp = reader.ReadU16();
                        break;

                    case _datFlags.FullGround: type.IsFullGround = true; break;
                    case _datFlags.IgnoreLook: type.IgnoreLook = true; break;

                    case _datFlags.Cloth:
                        type.IsCloth = true;
                        type.ClothSlot = reader.ReadU16();
                        break;

                    case _datFlags.MarketItem:
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

                    case _datFlags.DefaultAction:
                        type.HasDefaultAction = true;
                        type.DefaultAction = reader.ReadU16();
                        break;

                    case _datFlags.Wrappable: type.IsWrapable = true; break;
                    case _datFlags.Unwrappable: type.IsUnwrapable = true; break;
                    case _datFlags.TopEffect: type.IsTopEffect = true; break;
                    case _datFlags.Usable: type.IsUsable = true; break;

                    default:
                        string msg = $"readUnknownFlag: Flag=0x{flag:X} (Prev=0x{previousFlag:X}) | Category={type.Category} | ID={type.ID} | Pos={reader.BaseStream.Position}";
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
                        frameGroup.LoopCount = reader.ReadInt32();
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
    }
}