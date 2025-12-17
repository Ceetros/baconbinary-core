namespace BaconBinary.Core.Enum
{
    public enum ServerItemAttribute : byte
    {
        ServerId = 0x10,
        ClientId = 0x11,
        Name = 0x12,
        GroundSpeed = 0x14,
        SpriteHash = 0x20,
        MinimaColor = 0x21,
        MaxReadWriteChars = 0x22,
        MaxReadChars = 0x23,
        Light = 0x2A,
        StackOrder = 0x2B,
        TradeAs = 0x2D
    }
}
