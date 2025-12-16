using BaconBinary.Core.Configurations;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Dat.Interfaces;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat.Readers
{
    public class MetadataReader5 : IMetadataReader
    {
        public bool ReadProperties(ClientBinaryReader reader, ThingType type)
        {
            while (true)
            {
                byte flagByte = reader.ReadByte();
                DatFlags flag = (DatFlags)flagByte;

                if (flag == DatFlags.LastFlag) break;

                switch (flag)
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
                        type.MarketName = reader.ReadStringU16();
                        type.MarketRestrictProfession = reader.ReadU16();
                        type.MarketRestrictLevel = reader.ReadU16();
                        break;

                    default:
                        break;
                }
            }
            return true;
        }

        public void ReadTexturePatterns(ClientBinaryReader reader, ThingType type)
        {

            byte width = reader.ReadByte();
            byte height = reader.ReadByte();

            if (width > 1 || height > 1) reader.ReadByte();

            byte layers = reader.ReadByte();
            byte patternX = reader.ReadByte();
            byte patternY = reader.ReadByte();
            byte patternZ = reader.ReadByte();
            byte frames = reader.ReadByte();

            var group = new FrameGroup();
            group.SetDimensions(width, height, layers, patternX, patternY, patternZ, frames);

            if (frames > 1)
            {
                group.IsAnimation = true;
                for (int i = 0; i < frames; i++)
                    group.FrameDurations.Add(new FrameDuration(100, 100));
            }

            int totalSprites = width * height * layers * patternX * patternY * patternZ * frames;
            group.SpriteIDs = new uint[totalSprites];

            for (int i = 0; i < totalSprites; i++)
            {
                group.SpriteIDs[i] = ClientFeatures.Extended ? reader.ReadUInt32() : reader.ReadU16();
            }

            type.FrameGroups[FrameGroupType.Default] = group;
        }

        public void WriteThing(BinaryWriter writer, ThingType thing)
        {
            throw new NotImplementedException();
        }
    }
}