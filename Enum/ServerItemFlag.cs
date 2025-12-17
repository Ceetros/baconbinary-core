using System;

namespace BaconBinary.Core.Enum
{
    [Flags]
    public enum ServerItemFlag : uint
    {
        None                = 0,
        Unpassable          = 1 << 0,
        BlockMissiles       = 1 << 1,
        BlockPathfinder     = 1 << 2,
        HasElevation        = 1 << 3,
        MultiUse            = 1 << 4,
        Pickupable          = 1 << 5,
        Movable             = 1 << 6,
        Stackable           = 1 << 7,
        FloorChangeDown     = 1 << 8,
        FloorChangeNorth    = 1 << 9,
        FloorChangeEast     = 1 << 10,
        FloorChangeSouth    = 1 << 11,
        FloorChangeWest     = 1 << 12,
        StackOrder          = 1 << 13,
        Readable            = 1 << 14,
        Rotatable           = 1 << 15,
        Hangable            = 1 << 16,
        HookSouth           = 1 << 17,
        HookEast            = 1 << 18,
        CanNotDecay         = 1 << 19,
        AllowDistanceRead   = 1 << 20,
        Unused              = 1 << 21,
        ClientCharges       = 1 << 22,
        IgnoreLook          = 1 << 23,
        IsAnimation         = 1 << 24,
        FullGround          = 1 << 25,
        ForceUse            = 1 << 26
    }
}
