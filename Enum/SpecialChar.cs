namespace BaconBinary.Core.Enum
{
    public enum SpecialChar : byte
    {
        NodeStart = 0xFE,
        NodeEnd = 0xFF,
        EscapeChar = 0xFD,
    }
}
