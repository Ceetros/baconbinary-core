using System.IO;
using System.Text;
using BaconBinary.Core.Enum;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat
{
    public class DatWriter
    {
        public void WriteDatFile(DatFile datFile, string path)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new BinaryWriter(stream);

            var metaData = MetadataReaderFactory.GetReader(ushort.Parse(datFile.Version.Replace(".", "")));
            
            writer.Write((uint)datFile.Signature);
            writer.Write((ushort)datFile.Items.Count);
            writer.Write((ushort)datFile.Outfits.Count);
            writer.Write((ushort)datFile.Effects.Count);
            writer.Write((ushort)datFile.Missiles.Count);
            
            for (var id = 100; id <= datFile.Items.Count; id++)
            {
                var pair = datFile.Items[(uint)id];
                metaData.WriteThing(writer, pair);
            }
            
            foreach (var pair in datFile.Outfits)
            {
                metaData.WriteThing(writer, pair.Value);
            }
            
            foreach (var pair in datFile.Effects)
            {
                metaData.WriteThing(writer, pair.Value);
            }
            
            foreach (var pair in datFile.Missiles)
            {
                metaData.WriteThing(writer, pair.Value);
            }
        }
    }
}
