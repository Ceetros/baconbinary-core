using System;
using System.IO;
using System.Linq;
using BaconBinary.Core.IO; 

namespace BaconBinary.Core.IO.Spr
{
    public class SprReader
    {
        public SprFile ReadSprFile(string filePath, string version, bool extended = false)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Sprite file not found: {filePath}");
            }
            
            byte[] expectedSig = SprSignature.GetSignature(version);
            
            var sprFile = new SprFile(filePath);
            
            var stream = sprFile.GetStreamForHeaderReading();
            var reader = new BinaryReader(stream);

            byte[] fileSig = reader.ReadBytes(expectedSig.Length);
            if (!fileSig.SequenceEqual(expectedSig))
            {
                sprFile.Dispose();
                throw new InvalidDataException($"Invalid .spr file signature for version {version}.");
            }

            int spriteCount = ClientFeatures.Extended ? (int)reader.ReadUInt32() : reader.ReadUInt16();
            
            long[] offsets = new long[spriteCount + 1]; 
            
            for (int i = 1; i <= spriteCount; i++)
            {
                offsets[i] = reader.ReadUInt32();
            }
            
            sprFile.SetOffsets(offsets, spriteCount);

            return sprFile;
        }
        
        public byte[] ExtractPixels(SprFile sprFile, int spriteId)
        {
            var reader = sprFile.GetReaderAt(spriteId);
            if (reader == null)
            {
                return null;
            }
            
            try
            {
                byte[] header = reader.ReadBytes(5);
                
                if (header.Length < 5) return null;
                
                ushort dataSize = BitConverter.ToUInt16(header, 3);
                
                byte[] body = reader.ReadBytes(dataSize);
                
                byte[] fullBlock = new byte[header.Length + body.Length];
                Buffer.BlockCopy(header, 0, fullBlock, 0, header.Length);
                Buffer.BlockCopy(body, 0, fullBlock, header.Length, body.Length);
                
                return fullBlock;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}