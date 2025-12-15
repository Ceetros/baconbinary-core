using BaconBinary.Core.IO.Dat;
using BaconBinary.Core.IO.Dat.Interfaces;
using BaconBinary.Core.IO.Dat.Readers;

public static class MetadataReaderFactory
{
    public static IMetadataReader GetReader(ushort version)
    {
        return version switch
        {
            < 710 => throw new NotSupportedException("Not sppported version"),
            <= 730 => new MetadataReader1(),
            <= 750 => new MetadataReader2(),
            <= 772 => new MetadataReader3(),
            <= 854 => new MetadataReader4(),
            <= 986 => new MetadataReader5(),
            _     => new MetadataReader6()
        };
    }
}