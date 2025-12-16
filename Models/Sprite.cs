using System;

namespace BaconBinary.Core.Models
{
    public class Sprite
    {
        public const ushort ARGBPixelsDataSize = 4096;
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
        
        public byte[] GetPixels()
        {
            if (this.CompressedPixels == null || this.CompressedPixels.Length == 0)
            {
                return BlankARGBSprite;
            }

            var pixels = new byte[ARGBPixelsDataSize];
            int writePos = 0;
            int readPos = 0;
            int inputLength = this.CompressedPixels.Length;

            while (readPos < inputLength)
            { 
                if (readPos + 4 > inputLength) break;

                ushort transparentPixels = BitConverter.ToUInt16(this.CompressedPixels, readPos);
                readPos += 2;

                ushort coloredPixels = BitConverter.ToUInt16(this.CompressedPixels, readPos);
                readPos += 2;
                
                writePos += transparentPixels * 4;

                for (int i = 0; i < coloredPixels; i++)
                {
                    if (writePos + 4 > ARGBPixelsDataSize) break;
                    
                    int bytesToRead = IsTransparent ? 4 : 3;
                    if (readPos + bytesToRead > inputLength) break;

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
