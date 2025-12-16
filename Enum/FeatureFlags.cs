namespace BaconBinary.Core.Enum;

[Flags]
enum FeatureFlags : byte
{
    None = 0,
    Extended = 1 << 0,
    Transparency = 1 << 1,
    FrameDurations = 1 << 2,
    FrameGroups = 1 << 3
}