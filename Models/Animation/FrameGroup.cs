using System;
using System.Collections.Generic;
using BaconBinary.Core.Enum;

namespace BaconBinary.Core.Models
{
    /// <summary>
    /// Represents the duration of a specific animation frame.
    /// </summary>
    public struct FrameDuration
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public FrameDuration(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    public class FrameGroup
    {
        public byte Width { get; set; } = 1;
        public byte Height { get; set; } = 1;
        public byte Layers { get; set; } = 1;
        public byte PatternX { get; set; } = 1;
        public byte PatternY { get; set; } = 1;
        public byte PatternZ { get; set; } = 1;
        public byte Frames { get; set; } = 1;
        
        public bool IsAnimation { get; set; }
        public int LoopCount { get; set; }
        public byte AnimationMode { get; set; }
        public uint StartFrame { get; set; }
        public List<FrameDuration> FrameDurations { get; set; } = new();
        
        public uint[] SpriteIDs { get; set; }

        public FrameGroup()
        {
        }

        /// <summary>
        /// Configures the dimensions of the FrameGroup and initializes the storage array.
        /// </summary>
        public void SetDimensions(byte width, byte height, byte layers, byte patternX, byte patternY, byte patternZ, byte frames)
        {
            Width = width;
            Height = height;
            Layers = layers;
            PatternX = patternX;
            PatternY = patternY;
            PatternZ = patternZ;
            Frames = frames;
            
            int totalSprites = Width * Height * Layers * PatternX * PatternY * PatternZ * Frames;
            SpriteIDs = new uint[totalSprites];
        }

        /// <summary>
        /// Calculates the specific index in the SpriteIDs array based on the current state.
        /// Replicates the formula used in standard Tibia clients.
        /// </summary>
        public int GetSpriteIndex(int frame, int x, int y, int z, int layer)
        {
            int index = ((((frame % Frames) * PatternZ + z) * PatternY + y) * PatternX + x) * Layers + layer;
            
            return index * Width * Height;
        }

        /// <summary>
        /// Gets the specific Sprite ID for a given coordinate and animation frame.
        /// </summary>
        public uint GetSpriteId(int frame, int x, int y, int z, int layer, int widthOffset = 0, int heightOffset = 0)
        {
            if (SpriteIDs == null || SpriteIDs.Length == 0)
            {
                return 0;
            }
            
            if (frame >= Frames) frame = frame % Frames;
            if (z >= PatternZ) z = z % PatternZ;
            if (y >= PatternY) y = y % PatternY;
            if (x >= PatternX) x = x % PatternX;
            if (layer >= Layers) layer = layer % Layers;

            int baseIndex = GetSpriteIndex(frame, x, y, z, layer);
            
            int finalIndex = baseIndex + (heightOffset * Width) + widthOffset;

            if (finalIndex >= 0 && finalIndex < SpriteIDs.Length)
            {
                return SpriteIDs[finalIndex];
            }

            return 0;
        }
    }
}