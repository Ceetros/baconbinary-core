using System.Collections.Generic;
using BaconBinary.Core.Enum;

namespace BaconBinary.Core.Models
{
    public class ThingType
    {
        public uint ID { get; set; }
        public ThingCategory Category { get; set; }

        public int? FrameIndex { get; set; } = 0;
        
        public Dictionary<FrameGroupType, FrameGroup> FrameGroups { get; private set; } = new();
        
        
        // Ground Properties
        public bool IsGround { get; set; }
        public ushort GroundSpeed { get; set; }
        public bool IsGroundBorder { get; set; }
        public bool IsFullGround { get; set; }
        public bool FloorChange { get; set; }

        // Stacking / Container
        public bool IsOnBottom { get; set; }
        public bool IsOnTop { get; set; }
        public bool IsContainer { get; set; }
        public bool IsStackable { get; set; }

        // Interaction
        public bool ForceUse { get; set; }
        public bool MultiUse { get; set; }
        public bool HasCharges { get; set; }
        public bool IsUsable { get; set; }
        public bool HasDefaultAction { get; set; }
        public bool IsWrapable { get; set; }
        public bool IsUnwrapable { get; set; }
        public ushort DefaultAction { get; set; }

        // Writing
        public bool IsWritable { get; set; }
        public bool IsWritableOnce { get; set; }
        public ushort MaxTextLength { get; set; }

        // Fluids
        public bool IsFluidContainer { get; set; }
        public bool IsFluid { get; set; }

        // Movement / Blocking
        public bool IsUnpassable { get; set; }
        public bool IsUnmoveable { get; set; }
        public bool BlockMissile { get; set; }
        public bool BlockPathfind { get; set; }
        public bool NoMoveAnimation { get; set; }
        public bool IsPickupable { get; set; }
        public bool IsHangable { get; set; }

        // Orientation
        public bool IsVertical { get; set; }
        public bool IsHorizontal { get; set; }
        public bool IsRotatable { get; set; }
        public bool IsLyingObject { get; set; }

        // Visuals
        public bool HasLight { get; set; }
        public ushort LightLevel { get; set; }
        public ushort LightColor { get; set; }
        public bool DontHide { get; set; }
        public bool IsTranslucent { get; set; }
        public bool AnimateAlways { get; set; }
        public bool IsTopEffect { get; set; }
        public bool IgnoreLook { get; set; }

        // Offset / Elevation
        public bool HasOffset { get; set; }
        public short OffsetX { get; set; }
        public short OffsetY { get; set; }
        public bool HasElevation { get; set; }
        public ushort Elevation { get; set; }

        // Minimap & Lens
        public bool IsMiniMap { get; set; }
        public ushort MiniMapColor { get; set; }
        public bool IsLensHelp { get; set; }
        public ushort LensHelp { get; set; }

        // Cloth / Player
        public bool IsCloth { get; set; }
        public ushort ClothSlot { get; set; }

        // Market
        public bool IsMarketItem { get; set; }
        public ushort MarketCategory { get; set; }
        public ushort MarketTradeAs { get; set; }
        public ushort MarketShowAs { get; set; }
        public string MarketName { get; set; }
        public ushort MarketRestrictProfession { get; set; }
        public ushort MarketRestrictLevel { get; set; }

        // Wrapping
        public bool IsWrappable { get; set; }
        public bool IsUnwrappable { get; set; }


        public ThingType()
        {
        }

        /// <summary>
        /// Helper to get the main framegroup (usually Default/Idle).
        /// </summary>
        public FrameGroup GetDefaultFrameGroup()
        {
            return FrameGroups.ContainsKey(FrameGroupType.Default) 
                ? FrameGroups[FrameGroupType.Default] 
                : null;
        }
    }
}