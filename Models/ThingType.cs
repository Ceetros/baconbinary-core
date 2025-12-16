using System.Collections.Generic;
using System.Linq;
using BaconBinary.Core.Enum;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BaconBinary.Core.Models
{
    public partial class ThingType : ObservableObject
    {
        [ObservableProperty] private uint _id;
        [ObservableProperty] private ThingCategory _category;

        public int? FrameIndex { get; set; } = 0;
        
        public Dictionary<FrameGroupType, FrameGroup> FrameGroups { get; set; } = new();
        
        // Helper properties for UI Binding
        public byte Width => GetDefaultFrameGroup()?.Width ?? 1;
        public byte Height => GetDefaultFrameGroup()?.Height ?? 1;
        public byte Layers => GetDefaultFrameGroup()?.Layers ?? 1;
        public byte PatternX => GetDefaultFrameGroup()?.PatternX ?? 1;
        public byte PatternY => GetDefaultFrameGroup()?.PatternY ?? 1;
        public byte PatternZ => GetDefaultFrameGroup()?.PatternZ ?? 1;
        public int Frames => GetDefaultFrameGroup()?.Frames ?? 1;
        
        // Ground Properties
        [ObservableProperty] private bool _isGround;
        [ObservableProperty] private ushort _groundSpeed;
        [ObservableProperty] private bool _isGroundBorder;
        [ObservableProperty] private bool _isFullGround;
        [ObservableProperty] private bool _floorChange;

        // Stacking / Container
        [ObservableProperty] private bool _isOnBottom;
        [ObservableProperty] private bool _isOnTop;
        [ObservableProperty] private bool _isContainer;
        [ObservableProperty] private bool _isStackable;

        // Interaction
        [ObservableProperty] private bool _forceUse;
        [ObservableProperty] private bool _isMultiUse;
        [ObservableProperty] private bool _hasCharges;
        [ObservableProperty] private bool _isUsable;
        [ObservableProperty] private bool _hasDefaultAction;
        [ObservableProperty] private bool _isWrapable;
        [ObservableProperty] private bool _isUnwrapable;
        [ObservableProperty] private ushort _defaultAction;

        // Writing
        [ObservableProperty] private bool _isWritable;
        [ObservableProperty] private bool _isWritableOnce;
        [ObservableProperty] private ushort _maxTextLength;
        
        public bool IsReadable { get => IsWritableOnce; set => SetProperty(ref _isWritableOnce, value); }

        // Fluids
        [ObservableProperty] private bool _isFluidContainer;
        [ObservableProperty] private bool _isFluid;

        // Movement / Blocking
        [ObservableProperty] private bool _isUnpassable;
        [ObservableProperty] private bool _isUnmoveable;
        [ObservableProperty] private bool _blockMissile;
        [ObservableProperty] private bool _blockPathfind;
        [ObservableProperty] private bool _noMoveAnimation;
        [ObservableProperty] private bool _isPickupable;
        [ObservableProperty] private bool _isHangable;
        [ObservableProperty] private bool _isHookSouth;
        [ObservableProperty] private bool _isHookEast;
        
        public bool WalkStack { get => Elevation > 0; set { if(value && Elevation == 0) Elevation = 1; else if(!value) Elevation = 0; OnPropertyChanged(nameof(WalkStack)); } }

        // Orientation
        [ObservableProperty] private bool _isVertical;
        [ObservableProperty] private bool _isHorizontal;
        [ObservableProperty] private bool _isRotatable;
        [ObservableProperty] private bool _isLyingObject;

        // Visuals
        [ObservableProperty] private bool _hasLight;
        [ObservableProperty] private ushort _lightLevel;
        [ObservableProperty] private ushort _lightColor;
        [ObservableProperty] private bool _dontHide;
        [ObservableProperty] private bool _isTranslucent;
        [ObservableProperty] private bool _animateAlways;
        [ObservableProperty] private bool _isTopEffect;
        [ObservableProperty] private bool _ignoreLook;

        // Offset / Elevation
        [ObservableProperty] private bool _hasOffset;
        [ObservableProperty] private short _offsetX;
        [ObservableProperty] private short _offsetY;
        [ObservableProperty] private bool _hasElevation;
        [ObservableProperty] private ushort _elevation;

        // Minimap & Lens
        [ObservableProperty] private bool _isMiniMap;
        [ObservableProperty] private ushort _miniMapColor;
        [ObservableProperty] private bool _isLensHelp;
        [ObservableProperty] private ushort _lensHelp;

        // Cloth / Player
        [ObservableProperty] private bool _isCloth;
        [ObservableProperty] private ushort _clothSlot;

        // Market
        [ObservableProperty] private bool _isMarketItem;
        [ObservableProperty] private ushort _marketCategory;
        [ObservableProperty] private ushort _marketTradeAs;
        [ObservableProperty] private ushort _marketShowAs;
        [ObservableProperty] private string _marketName;
        [ObservableProperty] private ushort _marketRestrictProfession;
        [ObservableProperty] private ushort _marketRestrictLevel;

        // Wrapping
        [ObservableProperty] private bool _isWrappable;
        [ObservableProperty] private bool _isUnwrappable;


        public ThingType()
        {
        }

        /// <summary>
        /// Creates a deep clone of the ThingType, including its FrameGroups.
        /// </summary>
        public ThingType Clone()
        {
            var clone = (ThingType)this.MemberwiseClone();
            
            // Deep clone the FrameGroups dictionary
            clone.FrameGroups = new Dictionary<FrameGroupType, FrameGroup>();
            foreach (var pair in this.FrameGroups)
            {
                clone.FrameGroups.Add(pair.Key, pair.Value.Clone());
            }

            return clone;
        }

        /// <summary>
        /// Applies properties from another ThingType to this instance.
        /// </summary>
        public void ApplyProps(ThingType source)
        {
            var properties = typeof(ThingType).GetProperties().Where(p => p.CanWrite && p.CanRead);
            foreach (var prop in properties)
            {
                // Don't copy FrameGroups, as they are managed separately
                if (prop.Name != nameof(FrameGroups))
                {
                    var value = prop.GetValue(source);
                    prop.SetValue(this, value);
                }
            }
        }

        public FrameGroup GetDefaultFrameGroup()
        {
            return FrameGroups.Values.FirstOrDefault();
        }
    }
}
