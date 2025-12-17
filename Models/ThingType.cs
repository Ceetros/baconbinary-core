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
        
        public byte Width
        {
            get => GetDefaultFrameGroup()?.Width ?? 1;
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.Width != value)
                {
                    group.Width = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public byte Height
        {
            get => GetDefaultFrameGroup()?.Height ?? 1;
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.Height != value)
                {
                    group.Height = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public byte Layers
        {
            get => GetDefaultFrameGroup()?.Layers ?? 1;
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.Layers != value)
                {
                    group.Layers = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public byte PatternX
        {
            get => GetDefaultFrameGroup()?.PatternX ?? 1;
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.PatternX != value)
                {
                    group.PatternX = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public byte PatternY
        {
            get => GetDefaultFrameGroup()?.PatternY ?? 1;
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.PatternY != value)
                {
                    group.PatternY = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public byte PatternZ
        {
            get => GetDefaultFrameGroup()?.PatternZ ?? 1;
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.PatternZ != value)
                {
                    group.PatternZ = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public byte Frames
        {
            get => (byte)(GetDefaultFrameGroup()?.Frames ?? 1);
            set
            {
                var group = GetDefaultFrameGroup();
                if (group != null && group.Frames != value)
                {
                    group.Frames = value;
                    OnPropertyChanged();
                }
            }
        }
        
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
        [ObservableProperty] private bool _isSplash;

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

        [ObservableProperty] private bool _hasServerItem;


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
            if (!FrameGroups.Any())
            {
                FrameGroups[FrameGroupType.Default] = new FrameGroup();
            }
            return FrameGroups.Values.FirstOrDefault();
        }
    }
}
