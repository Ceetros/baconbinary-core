using System.IO;
using BaconBinary.Core.Configurations;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Spr
{
    public class SprWriter
    {
        public void WriteSprFile(SprFile sprFile, string path, string version)
        {
            string tempPath = path + ".tmp";
            
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            ushort versionNumber = ushort.Parse(version.Replace(".", ""));
            ClientFeatures.SetVersion(versionNumber);

            using (var tempStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(tempStream))
            {
                writer.Write(sprFile.Signature);

                uint count = sprFile.SpriteCount;
                uint headerSize;

                if (ClientFeatures.Extended)
                {
                    writer.Write(count);
                    headerSize = 8;
                }
                else
                {
                    writer.Write((ushort)count);
                    headerSize = 6;
                }

                long addressTablePosition = writer.BaseStream.Position;
                uint offset = (count * 4) + headerSize;

                for (uint id = 1; id <= count; id++)
                {
                    writer.BaseStream.Position = addressTablePosition + ((id - 1) * 4);

                    if (sprFile.Sprites.TryGetValue(id, out var sprite) && sprite.Size > 0)
                    {
                        writer.Write(offset);
                        
                        writer.BaseStream.Position = offset;
                        
                        writer.Write((byte)0xFF);
                        writer.Write((byte)0x00);
                        writer.Write((byte)0xFF);
                        writer.Write(sprite.Size);
                        
                        writer.Write(sprite.CompressedPixels);
                        
                        offset = (uint)writer.BaseStream.Position;
                    }
                    else
                    {
                        writer.Write((uint)0);
                    }
                }
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Move(tempPath, path);
        }
    }
}
