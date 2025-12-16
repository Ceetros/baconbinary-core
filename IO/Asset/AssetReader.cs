using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BaconBinary.Core.IO.Spr;
using BaconBinary.Core.Security;
using K4os.Compression.LZ4;

namespace BaconBinary.Core.IO.Asset
{
    public class AssetReader
    {
        public List<byte[]> ReadAssetData(string path, string key)
        {
            Console.WriteLine($"Reading Asset File: {path}");
            var sprites = new List<byte[]>();
            
            if (!File.Exists(path))
            {
                Console.WriteLine($"ERROR: Asset file not found: {path}");
                throw new FileNotFoundException($"Asset file not found: {path}");
            }

            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fileStream);
            
            byte[] signatureBytes = reader.ReadBytes(5);
            string signature = Encoding.ASCII.GetString(signatureBytes);
            if (signature != "BSUIT")
                throw new InvalidDataException("Invalid asset file signature.");

            byte formatVersion = reader.ReadByte();
            byte encryptionType = reader.ReadByte();
            byte[] iv = null;

            if (encryptionType == 1) iv = reader.ReadBytes(16);
            
            byte[] bodyBytes = reader.ReadBytes((int)(fileStream.Length - fileStream.Position));

            if (encryptionType == 1)
            {
                if (string.IsNullOrEmpty(key)) throw new ArgumentException("Encryption key required.");
                try
                {
                    bodyBytes = CryptographyService.Decrypt(bodyBytes, key, iv);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Decryption failed: {ex.Message}");
                    throw;
                }
            }
            
            using var bodyStream = new MemoryStream(bodyBytes);
            using var bodyReader = new BinaryReader(bodyStream);

            byte compressionType = bodyReader.ReadByte();
            uint spriteCountInFile = bodyReader.ReadUInt32();
            Console.WriteLine($"Asset Body: Compression={compressionType}, Sprites={spriteCountInFile}");

            var addresses = new uint[spriteCountInFile];
            for (int i = 0; i < spriteCountInFile; i++)
            {
                addresses[i] = bodyReader.ReadUInt32();
            }

            for (int i = 0; i < spriteCountInFile; i++)
            {
                uint address = addresses[i];
                if (address == 0)
                {
                    sprites.Add(null);
                    continue;
                }

                bodyStream.Position = address;
                byte[] pixelData;

                if (compressionType == 1)
                {
                    int compressedSize = bodyReader.ReadInt32();
                    byte[] compressedData = bodyReader.ReadBytes(compressedSize);
                    pixelData = new byte[4096];
                    LZ4Codec.Decode(compressedData, 0, compressedData.Length, pixelData, 0, pixelData.Length);
                }
                else
                {
                    pixelData = bodyReader.ReadBytes(4096);
                }
                
                sprites.Add(pixelData);
            }
            
            Console.WriteLine($"Loaded {sprites.Count} sprites from asset file.");
            return sprites;
        }
    }
}
