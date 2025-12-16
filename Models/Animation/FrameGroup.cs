using System;
using System.Collections.Generic;
using System.Linq;
using BaconBinary.Core.Enum;

namespace BaconBinary.Core.Models
{
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
        public uint LoopCount { get; set; }
        public byte AnimationMode { get; set; }
        public uint StartFrame { get; set; }
        public List<FrameDuration> FrameDurations { get; set; } = new();
        
        public uint[] SpriteIDs { get; set; }

        public FrameGroup()
        {
        }

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

        private int GetLinearIndex(int frame, int patternX, int patternY, int patternZ, int layer, int x, int y)
        {
            if (frame >= Frames || patternX >= PatternX || patternY >= PatternY || patternZ >= PatternZ || layer >= Layers || x >= Width || y >= Height)
            {
                return -1;
            }
            
            int index = ((((frame * PatternZ + patternZ) * PatternY + patternY) * PatternX + patternX) * Layers + layer) * Height * Width + (y * Width + x);
            return index;
        }

        public uint GetSpriteId(int frame, int patternX, int patternY, int patternZ, int layer, int x, int y)
        {
            if (SpriteIDs == null || SpriteIDs.Length == 0) return 0;
            
            int index = GetLinearIndex(frame, patternX, patternY, patternZ, layer, x, y);

            if (index >= 0 && index < SpriteIDs.Length)
            {
                return SpriteIDs[index];
            }

            return 0;
        }

        public void SetSpriteId(int frame, int patternX, int patternY, int patternZ, int layer, int x, int y, uint spriteId)
        {
            if (SpriteIDs == null || SpriteIDs.Length == 0) return;

            int index = GetLinearIndex(frame, patternX, patternY, patternZ, layer, x, y);

            if (index >= 0 && index < SpriteIDs.Length)
            {
                SpriteIDs[index] = spriteId;
            }
        }

        public FrameGroup Clone()
        {
            var clone = (FrameGroup)this.MemberwiseClone();
            
            if (SpriteIDs != null)
            {
                clone.SpriteIDs = (uint[])SpriteIDs.Clone();
            }
            
            if (FrameDurations != null)
            {
                clone.FrameDurations = new List<FrameDuration>(FrameDurations);
            }

            return clone;
        }

        public void Resize(byte width, byte height, byte layers, byte patternX, byte patternY, byte patternZ, byte frames)
        {
            var oldGroup = this.Clone();
            
            SetDimensions(width, height, layers, patternX, patternY, patternZ, frames);
            
            int minWidth = Math.Min(width, oldGroup.Width);
            int minHeight = Math.Min(height, oldGroup.Height);
            int minLayers = Math.Min(layers, oldGroup.Layers);
            int minPatternX = Math.Min(patternX, oldGroup.PatternX);
            int minPatternY = Math.Min(patternY, oldGroup.PatternY);
            int minPatternZ = Math.Min(patternZ, oldGroup.PatternZ);
            int minFrames = Math.Min(frames, oldGroup.Frames);

            for (int f = 0; f < minFrames; f++)
            for (int z = 0; z < minPatternZ; z++)
            for (int py = 0; py < minPatternY; py++)
            for (int px = 0; px < minPatternX; px++)
            for (int l = 0; l < minLayers; l++)
            for (int y = 0; y < minHeight; y++)
            for (int x = 0; x < minWidth; x++)
            {
                uint spriteId = oldGroup.GetSpriteId(f, px, py, z, l, x, y);
                SetSpriteId(f, px, py, z, l, x, y, spriteId);
            }
            
            if (frames != oldGroup.Frames)
            {
                var newDurations = new List<FrameDuration>();
                for (int i = 0; i < frames; i++)
                {
                    if (i < oldGroup.FrameDurations.Count)
                        newDurations.Add(oldGroup.FrameDurations[i]);
                    else
                        newDurations.Add(new FrameDuration(100, 100));
                }
                FrameDurations = newDurations;
            }
        }
    }
}
