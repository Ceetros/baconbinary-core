using System;
using System.Collections.Generic;

namespace BaconBinary.Core.IO.Spr
{
    public static class SpriteCompressor
    {
        public static byte[] Compress(byte[] bgraPixels, bool isTransparent)
        {
            var compressed = new List<byte>();
            int readPos = 0;
            
            while (readPos < bgraPixels.Length)
            {
                ushort transparentCount = 0;
                while (readPos < bgraPixels.Length && bgraPixels[readPos + 3] == 0)
                {
                    transparentCount++;
                    readPos += 4;
                }

                ushort coloredCount = 0;
                int coloredStartPos = readPos;
                while (readPos < bgraPixels.Length && bgraPixels[readPos + 3] != 0)
                {
                    coloredCount++;
                    readPos += 4;
                }

                compressed.AddRange(BitConverter.GetBytes(transparentCount));
                compressed.AddRange(BitConverter.GetBytes(coloredCount));

                for (int i = 0; i < coloredCount; i++)
                {
                    int pixelPos = coloredStartPos + i * 4;
                    byte blue = bgraPixels[pixelPos];
                    byte green = bgraPixels[pixelPos + 1];
                    byte red = bgraPixels[pixelPos + 2];
                    byte alpha = bgraPixels[pixelPos + 3];

                    compressed.Add(red);
                    compressed.Add(green);
                    compressed.Add(blue);
                    if (isTransparent)
                    {
                        compressed.Add(alpha);
                    }
                }
            }

            return compressed.ToArray();
        }
    }
}
