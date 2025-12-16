using System;
using System.Collections.Generic;
using System.IO;
using BaconBinary.Core.Configurations;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Spr
{
    public class SprReader
    {
        public SprFile ReadSprFile(string filePath, string version)
        {
            var sprFile = new SprFile();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                sprFile.Signature = reader.ReadUInt32();
                
                ushort versionNumber = ushort.Parse(version.Replace(".", ""));
                ClientFeatures.SetVersion(versionNumber);

                if (ClientFeatures.Extended)
                {
                    sprFile.SpriteCount = reader.ReadUInt32();
                }
                else
                {
                    sprFile.SpriteCount = reader.ReadUInt16();
                }
                
                var addresses = new List<uint>();
                for (uint i = 0; i < sprFile.SpriteCount; i++)
                {
                    addresses.Add(reader.ReadUInt32());
                }
                
                for (uint id = 1; id <= sprFile.SpriteCount; id++)
                {
                    uint address = addresses[(int)id - 1];
                    if (address == 0)
                    {
                        sprFile.Sprites[id] = new Sprite(id, ClientFeatures.Transparency);
                        continue;
                    }

                    reader.BaseStream.Position = address;
                    
                    reader.ReadBytes(3);
                    
                    ushort pixelDataSize = reader.ReadUInt16();
                    
                    var sprite = new Sprite(id, ClientFeatures.Transparency)
                    {
                        Size = pixelDataSize
                    };

                    if (pixelDataSize > 0)
                    {
                        sprite.CompressedPixels = reader.ReadBytes(pixelDataSize);
                    }
                    
                    sprFile.Sprites[id] = sprite;
                }
            }

            return sprFile;
        }
    }
}
