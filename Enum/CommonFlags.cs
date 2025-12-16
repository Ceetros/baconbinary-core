namespace BaconBinary.Core.Enum;

[Flags]
enum CommonFlags : uint
{
    None = 0,
    AnimateAlways = 1 << 0,
    LyingObject = 1 << 1,
    TopEffect = 1 << 2,
    Translucent = 1 << 3,
    HasOffset = 1 << 4,
    HasElevation = 1 << 5,
    IsRotatable = 1 << 6,
    IsHangable = 1 << 7,
    IsHookSouth = 1 << 8,
    IsHookEast = 1 << 9,
    HasLight = 1 << 10,
    DontHide = 1 << 11,
    IsMiniMap = 1 << 12,
    IsLensHelp = 1 << 13,
    IgnoreLook = 1 << 14,
    IsCloth = 1 << 15,
    HasDefaultAction = 1 << 16,
    IsWrappable = 1 << 17,
    IsUnwrappable = 1 << 18,
    IsUsable = 1 << 19,
    IsVertical = 1 << 20,
    IsHorizontal = 1 << 21
}