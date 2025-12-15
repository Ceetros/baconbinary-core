using System;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Dat.Interfaces;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat
{
    public class MetadataReader2 : IMetadataReader
    {
        public bool ReadProperties(ClientBinaryReader reader, ThingType type)
        {
            while (true)
            {
                byte flagByte = reader.ReadByte();
                DatFlags flag = (DatFlags)flagByte;

                if (flag == DatFlags.LastFlag)
                    break;

                switch (flag)
                {
                    case DatFlags.Ground:
                        type.IsGround = true;
                        type.GroundSpeed = reader.ReadU16();
                        break;

                    case DatFlags.OnBottom:
                        type.IsOnBottom = true;
                        break;

                    case DatFlags.OnTop:
                        type.IsOnTop = true;
                        break;

                    case DatFlags.Container:
                        type.IsContainer = true;
                        break;

                    case DatFlags.Stackable:
                        type.IsStackable = true;
                        break;

                    case DatFlags.MultiUse:
                        type.MultiUse = true;
                        break;

                    case DatFlags.ForceUse:
                        type.ForceUse = true;
                        break;

                    case DatFlags.Writable:
                        type.IsWritable = true;
                        type.MaxTextLength = reader.ReadU16();
                        break;

                    case DatFlags.WritableOnce:
                        type.IsWritableOnce = true;
                        type.MaxTextLength = reader.ReadU16();
                        break;

                    case DatFlags.FluidContainer:
                        type.IsFluidContainer = true;
                        break;

                    case DatFlags.Fluid:
                        type.IsFluid = true;
                        break;

                    case DatFlags.Unpassable:
                        type.IsUnpassable = true;
                        break;

                    case DatFlags.Unmoveable:
                        type.IsUnmoveable = true;
                        break;

                    case DatFlags.BlockMissile:
                        type.BlockMissile = true;
                        break;

                    case DatFlags.BlockPathfind:
                        type.BlockPathfind = true;
                        break;

                    case DatFlags.Pickupable:
                        type.IsPickupable = true;
                        break;

                    case DatFlags.HasLight:
                        type.HasLight = true;
                        type.LightLevel = reader.ReadU16();
                        type.LightColor = reader.ReadU16();
                        break;

                    case DatFlags.FullGround:
                        type.IsFullGround = true;
                        break;

                    case DatFlags.HasElevation:
                        type.HasElevation = true;
                        type.Elevation = reader.ReadU16();
                        break;

                    case DatFlags.HasOffset:
                        type.HasOffset = true;
                        type.OffsetX = 8;
                        type.OffsetY = 8;
                        break;

                    case DatFlags.MiniMap:
                        type.IsMiniMap = true;
                        type.MiniMapColor = reader.ReadU16();
                        break;

                    case DatFlags.Rotatable:
                        type.IsRotatable = true;
                        break;

                    case DatFlags.LyingObject:
                        type.IsLyingObject = true;
                        break;

                    case DatFlags.Hangable:
                        type.IsHangable = true;
                        break;

                    case DatFlags.Vertical:
                        type.IsVertical = true;
                        break;

                    case DatFlags.Horizontal:
                        type.IsHorizontal = true;
                        break;

                    case DatFlags.AnimateAlways:
                        type.AnimateAlways = true;
                        break;

                    case DatFlags.LensHelp:
                        type.IsLensHelp = true;
                        type.LensHelp = reader.ReadU16();
                        break;

                    case DatFlags.Wrappable:
                        type.IsWrappable = true;
                        break;

                    case DatFlags.Unwrappable:
                        type.IsUnwrappable = true;
                        break;

                    case DatFlags.TopEffect:
                        type.IsTopEffect = true;
                        break;

                    default:
                        // Flags desconhecidas são ignoradas ou logadas
                        break;
                }
            }
            return true;
        }

        public void ReadTexturePatterns(ClientBinaryReader reader, ThingType type)
        {
            bool extended = false;
            bool frameGroups = false;
            
            int groupCount = 1;
            
            for (int groupType = 0; groupType < groupCount; groupType++)
            {
                var frameGroup = new FrameGroup();
                frameGroup.SetDimensions(
                    width: reader.ReadByte(),
                    height: reader.ReadByte(),
                    layers: 0,
                    patternX: 0,
                    patternY: 0,
                    patternZ: 0,
                    frames: 0
                );
                
                if (frameGroup.Width > 1 || frameGroup.Height > 1)
                {
                    reader.ReadByte();
                }

                byte layers = reader.ReadByte();
                byte patternX = reader.ReadByte();
                byte patternY = reader.ReadByte();
                byte patternZ = 1;
                byte frames = reader.ReadByte();

                frameGroup.SetDimensions(frameGroup.Width, frameGroup.Height, layers, patternX, patternY, patternZ, frames);

                if (frameGroup.Frames > 1)
                {
                    frameGroup.IsAnimation = true;
                    int defaultDuration = 100;
                    for (int k = 0; k < frameGroup.Frames; k++)
                    {
                        frameGroup.FrameDurations.Add(new FrameDuration(defaultDuration, defaultDuration));
                    }
                }
                
                int totalSprites = frameGroup.Width * frameGroup.Height * frameGroup.Layers * frameGroup.PatternX * frameGroup.PatternY * frameGroup.PatternZ * frameGroup.Frames;
                
                frameGroup.SpriteIDs = new uint[totalSprites];

                for (int i = 0; i < totalSprites; i++)
                {
                    frameGroup.SpriteIDs[i] = ClientFeatures.Extended ? reader.ReadUInt32() : reader.ReadU16();
                }

                type.FrameGroups[FrameGroupType.Default] = frameGroup;
            }
        }
    }
}