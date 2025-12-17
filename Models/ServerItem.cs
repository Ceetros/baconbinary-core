using BaconBinary.Core.Enum;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BaconBinary.Core.Models
{
    public partial class ServerItem : ObservableObject
    {
        [ObservableProperty] private ushort _id;
        [ObservableProperty] private ushort _clientId;
        [ObservableProperty] private ServerItemType _type;
        [ObservableProperty] private string _name;
        [ObservableProperty] private string _description;
        [ObservableProperty] private byte[] _spriteHash;
        
        [ObservableProperty] private bool _unpassable;
        [ObservableProperty] private bool _blockMissiles;
        [ObservableProperty] private bool _blockPathfinder;
        [ObservableProperty] private bool _hasElevation;
        [ObservableProperty] private bool _forceUse;
        [ObservableProperty] private bool _multiUse;
        [ObservableProperty] private bool _pickupable;
        [ObservableProperty] private bool _movable;
        [ObservableProperty] private bool _stackable;
        [ObservableProperty] private bool _hasStackOrder;
        [ObservableProperty] private bool _readable;
        [ObservableProperty] private bool _rotatable;
        [ObservableProperty] private bool _hangable;
        [ObservableProperty] private bool _hookSouth;
        [ObservableProperty] private bool _hookEast;
        [ObservableProperty] private bool _allowDistanceRead;
        [ObservableProperty] private bool _hasCharges;
        [ObservableProperty] private bool _ignoreLook;
        [ObservableProperty] private bool _fullGround;
        [ObservableProperty] private bool _isAnimation;
        
        [ObservableProperty] private ushort _groundSpeed;
        [ObservableProperty] private ushort _minimapColor;
        [ObservableProperty] private ushort _maxReadWriteChars;
        [ObservableProperty] private ushort _maxReadChars;
        [ObservableProperty] private ushort _lightLevel;
        [ObservableProperty] private ushort _lightColor;
        [ObservableProperty] private TileStackOrder _stackOrder;
        [ObservableProperty] private ushort _tradeAs;
        
        [ObservableProperty] private string _article;
        [ObservableProperty] private string _editorSuffix;
        [ObservableProperty] private ItemType _itemType;
        [ObservableProperty] private SlotType _slotType;
        [ObservableProperty] private WeaponType _weaponType;
        [ObservableProperty] private AmmoType _ammoType;
        [ObservableProperty] private ShootType _shootType;
        [ObservableProperty] private ShootType _effect;
        [ObservableProperty] private int _attack;
        [ObservableProperty] private int _defense;
        [ObservableProperty] private int _armor;
        [ObservableProperty] private int _charges;
        [ObservableProperty] private int _duration;
        [ObservableProperty] private int _decayTo;
        [ObservableProperty] private int _rotateTo;
        [ObservableProperty] private int _range;
        [ObservableProperty] private int _weight;
        [ObservableProperty] private int _worth;
        [ObservableProperty] private int _speed;
        [ObservableProperty] private uint _healthGain;
        [ObservableProperty] private uint _healthTicks;
        [ObservableProperty] private uint _manaGain;
        [ObservableProperty] private uint _manaTicks;
        [ObservableProperty] private int _maxHitChance;
        [ObservableProperty] private int _hitChance;
        [ObservableProperty] private int _magicLevelPoints;
        [ObservableProperty] private int _absorbPercentDeath;
        [ObservableProperty] private int _absorbPercentDrown;
        [ObservableProperty] private int _absorbPercentEarth;
        [ObservableProperty] private int _absorbPercentEnergy;
        [ObservableProperty] private int _absorbPercentFire;
        [ObservableProperty] private int _absorbPercentHoly;
        [ObservableProperty] private int _absorbPercentIce;
        [ObservableProperty] private int _absorbPercentLifeDrain;
        [ObservableProperty] private int _absorbPercentManaDrain;
        [ObservableProperty] private int _absorbPercentMagic;
        [ObservableProperty] private int _absorbPercentPhysical;
        [ObservableProperty] private bool _suppressDrunk;
        [ObservableProperty] private ItemField? _field;
        
        public string NameWithId => $"{Id} - {Name}";
    }
}
