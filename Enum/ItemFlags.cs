namespace BaconBinary.Core.Enum;

[Flags]
enum ItemFlags : uint
{
    None = 0,
    IsGround = 1 << 0,
    IsGroundBorder = 1 << 1,
    IsOnBottom = 1 << 2,
    IsOnTop = 1 << 3,
    IsContainer = 1 << 4,
    IsStackable = 1 << 5,
    ForceUse = 1 << 6,
    IsMultiUse = 1 << 7,
    IsWritable = 1 << 8,
    IsReadable = 1 << 9,
    IsFluidContainer = 1 << 10,
    IsFluid = 1 << 11,
    IsUnpassable = 1 << 12,
    IsUnmoveable = 1 << 13,
    BlockMissile = 1 << 14,
    BlockPathfind = 1 << 15,
    NoMoveAnimation = 1 << 16,
    IsPickupable = 1 << 17,
    IsFullGround = 1 << 18,
    IsMarketItem = 1 << 19
}