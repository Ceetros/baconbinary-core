using System;

namespace BaconBinary.Core.Models
{
    public class Sprite
    {
        public const ushort ARGBPixelsDataSize = 4096; // 32*32*4
        public static readonly byte[] BlankARGBSprite = new byte[ARGBPixelsDataSize];

        public uint Id { get; }
        public ushort Size { get; set; }
        public byte[] CompressedPixels { get; set; }
        public bool IsTransparent { get; }

        public Sprite(uint id, bool isTransparent)
        {
            Id = id;
            IsTransparent = isTransparent;
        }

        /// <summary>
        /// Decompresses the RLE-encoded sprite data into a 32bpp BGRA pixel array.
        /// </summary>
        public byte[] GetPixels()
        {
            if (this.CompressedPixels == null || this.Size == 0)
            {
                return BlankARGBSprite;
            }

            var pixels = new byte[ARGBPixelsDataSize];
            int writePos = 0;
            int readPos = 0;

            while (readPos < this.Size)
            {
                ushort transparentPixels = BitConverter.ToUInt16(this.CompressedPixels, readPos);
                readPos += 2;

                ushort coloredPixels = BitConverter.ToUInt16(this.CompressedPixels, readPos);
                readPos += 2;

                // Skip transparent pixels by advancing the write position
                writePos += transparentPixels * 4;

                for (int i = 0; i < coloredPixels; i++)
                {
                    if (writePos >= ARGBPixelsDataSize) break;

                    byte red = this.CompressedPixels[readPos++];
                    byte green = this.CompressedPixels[readPos++];
                    byte blue = this.CompressedPixels[readPos++];
                    byte alpha = this.IsTransparent ? this.CompressedPixels[readPos++] : (byte)0xFF;
                    
                    pixels[writePos++] = blue;
                    pixels[writePos++] = green;
                    pixels[writePos++] = red;
                    pixels[writePos++] = alpha;
                }
            }

            return pixels;
        }
    }
}
