using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using BaconBinary.Core.Models;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Dat.Interfaces;

namespace BaconBinary.Core.IO.Dat
{
    public class DatReader
    {
        /// <summary>
        /// Reads the .dat file and returns the full data structure in memory.
        /// </summary>
        public DatFile ReadDatFile(string filePath, string selectedVersion)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file was not found: {filePath}");
            }
            
            var expectedSignature = DatSignature.GetSignature(selectedVersion);
            
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var baseReader = new BinaryReader(fileStream))
            {    
                baseReader.BaseStream.Position = 0;
                uint fileSignature = baseReader.ReadUInt32();
                if (expectedSignature != fileSignature)
                {
                    throw new InvalidDataException($"Invalid file signature.");
                }
                
                return ParseContent(baseReader, selectedVersion);
            }
        }

        private DatFile ParseContent(BinaryReader baseReader, string version)
        {
            var datFile = new DatFile();
            var reader = new ClientBinaryReader(baseReader.BaseStream);
            
            ushort versionNumber = ParseVersionString(version);
            IMetadataReader metadataReader = MetadataReaderFactory.GetReader(versionNumber);

            reader.BaseStream.Position = 4;
            var itemCount = reader.ReadU16();
            reader.BaseStream.Position = 6;
            var outfitCount = reader.ReadU16();
            reader.BaseStream.Position = 8;
            var effectCount = reader.ReadU16();
            reader.BaseStream.Position = 10;
            var missileCount = reader.ReadU16();
            
            System.Diagnostics.Debug.WriteLine($"HEADER CHECK: Items={itemCount}, Outfits={outfitCount}, Effects={effectCount}, Missiles={missileCount}");
            
            ReadCategory(reader, metadataReader, itemCount, ThingCategory.Item, 100, datFile.Items);
            
            ReadCategory(reader, metadataReader, outfitCount, ThingCategory.Outfit, 1, datFile.Outfits);
            ReadCategory(reader, metadataReader, effectCount, ThingCategory.Effect, 1, datFile.Effects);
            ReadCategory(reader, metadataReader, missileCount, ThingCategory.Missile, 1, datFile.Missiles);

            return datFile;
        }

        private void ReadCategory(ClientBinaryReader reader, IMetadataReader metaReader, int count, ThingCategory category, uint startId, Dictionary<uint, ThingType> dictionary)
        {
            for (uint i = startId; i <= count; i++)
            {
                uint currentId = i;

                var thing = new ThingType();
                thing.ID = currentId;
                thing.Category = category;
                
                metaReader.ReadProperties(reader, thing);
                
                metaReader.ReadTexturePatterns(reader, thing);
                
                dictionary[currentId] = thing;
            }
        }

        private ushort ParseVersionString(string version)
        {
            string clean = version.Replace(".", "");
            if (ushort.TryParse(clean, out ushort result)) return result;
            return 760;
        }
    }
}