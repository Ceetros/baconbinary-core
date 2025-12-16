using System.IO;
using System.Text;
using BaconBinary.Core.Models;
using BaconBinary.Core.Security;
using K4os.Compression.LZ4;
using System.Collections.Generic;
using System.Linq;
using BaconBinary.Core.IO.Spr;

namespace BaconBinary.Core.IO.Asset
{
    public class AssetWriter
    {
        private const int SpritesPerFile = 1000000;

        public void WriteAssetFiles(SprFile sprFile, string basePath, bool encrypt, string key, byte compressionType = 1)
        {
            var baseName = Path.GetFileNameWithoutExtension(basePath);
            var directory = Path.GetDirectoryName(basePath);

            uint spritesWritten = 0;
            int fileIndex = 0;

            var spriteIds = sprFile.Sprites.Keys.OrderBy(id => id).ToList();

            while (spritesWritten < sprFile.SpriteCount)
            {
                string currentPath = Path.Combine(directory, $"{baseName}.asset{fileIndex}");
                var spritesForThisFile = spriteIds.Skip((int)spritesWritten).Take(SpritesPerFile).ToList();
                
                if (!spritesForThisFile.Any()) break;

                WriteSingleAssetFile(sprFile, spritesForThisFile, currentPath, encrypt, key, compressionType);
                
                spritesWritten += (uint)spritesForThisFile.Count;
                fileIndex++;
            }
        }

        private void WriteSingleAssetFile(SprFile sprFile, List<uint> spriteIds, string path, bool encrypt, string key, byte compressionType)
        {
            var bodyStream = new MemoryStream();
            using (var bodyWriter = new BinaryWriter(bodyStream))
            {
                WriteBody(bodyWriter, sprFile, spriteIds, compressionType);
            }

            byte[] bodyBytes = bodyStream.ToArray();

            byte[] iv = null;
            if (encrypt)
            {
                bodyBytes = CryptographyService.Encrypt(bodyBytes, key, out iv);
            }

            using (var finalStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(finalStream))
            {
                writer.Write(Encoding.ASCII.GetBytes("BSUIT"));
                writer.Write((byte)1); // Format Version
                writer.Write(encrypt ? (byte)1 : (byte)0);
                
                if (encrypt)
                {
                    writer.Write(iv);
                }
                
                writer.Write(bodyBytes);
            }
        }

        private void WriteBody(BinaryWriter writer, SprFile sprFile, List<uint> spriteIds, byte compressionType)
        {
            uint spriteCountInThisFile = (uint)spriteIds.Count;

            writer.Write(compressionType);
            writer.Write(spriteCountInThisFile);

            long addressTablePosition = writer.BaseStream.Position;
            for (int i = 0; i < spriteCountInThisFile; i++)
            {
                writer.Write((uint)0);
            }

            var addresses = new Dictionary<uint, uint>();
            foreach (uint id in spriteIds)
            {
                addresses[id] = (uint)writer.BaseStream.Position;
                
                if (sprFile.Sprites.TryGetValue(id, out var sprite) && sprite.Size > 0)
                {
                    var spriteData = sprite.CompressedPixels;

                    if (compressionType == 1) // LZ4
                    {
                        var compressedData = new byte[LZ4Codec.MaximumOutputSize(spriteData.Length)];
                        int compressedSize = LZ4Codec.Encode(spriteData, 0, spriteData.Length, compressedData, 0, compressedData.Length, LZ4Level.L12_MAX);
                        
                        writer.Write(compressedSize);
                        writer.Write(compressedData, 0, compressedSize);
                    }
                    else
                    {
                        writer.Write(spriteData);
                    }
                }
            }

            writer.BaseStream.Position = addressTablePosition;
            foreach (uint id in spriteIds)
            {
                writer.Write(addresses[id]);
            }
        }
    }
}
