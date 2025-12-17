using BaconBinary.Core.Enum;

namespace BaconBinary.Core.Models
{
    public class ServerItem
    {
        public ushort Id { get; set; }
        public ushort ClientId { get; set; }
        public ServerItemType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] SpriteHash { get; set; }
        
        public bool Unpassable { get; set; }
        public bool BlockMissiles { get; set; }
        public bool BlockPathfinder { get; set; }
        public bool HasElevation { get; set; }
        public bool ForceUse { get; set; }
        public bool MultiUse { get; set; }
        public bool Pickupable { get; set; }
        public bool Movable { get; set; }
        public bool Stackable { get; set; }
        public bool HasStackOrder { get; set; }
        public bool Readable { get; set; }
        public bool Rotatable { get; set; }
        public bool Hangable { get; set; }
        public bool HookSouth { get; set; }
        public bool HookEast { get; set; }
        public bool AllowDistanceRead { get; set; }
        public bool HasCharges { get; set; }
        public bool IgnoreLook { get; set; }
        public bool FullGround { get; set; }
        public bool IsAnimation { get; set; }
        
        public ushort GroundSpeed { get; set; }
        public ushort MinimapColor { get; set; }
        public ushort MaxReadWriteChars { get; set; }
        public ushort MaxReadChars { get; set; }
        public ushort LightLevel { get; set; }
        public ushort LightColor { get; set; }
        public TileStackOrder StackOrder { get; set; }
        public ushort TradeAs { get; set; }
        
        public string Article { get; set; }
        public string EditorSuffix { get; set; }
        public ItemType ItemType { get; set; }
        public SlotType SlotType { get; set; }
        public WeaponType WeaponType { get; set; }
        public AmmoType AmmoType { get; set; }
        public ShootType ShootType { get; set; }
        public ShootType Effect { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Armor { get; set; }
        public int Charges { get; set; }
        public int Duration { get; set; }
        public int DecayTo { get; set; }
        public int RotateTo { get; set; }
        public int Range { get; set; }
        public int Weight { get; set; }
        public int Worth { get; set; }
        public int Speed { get; set; }
        public uint HealthGain { get; set; }
        public uint HealthTicks { get; set; }
        public uint ManaGain { get; set; }
        public uint ManaTicks { get; set; }
        public int MaxHitChance { get; set; }
        public int HitChance { get; set; }
        public int MagicLevelPoints { get; set; }
        public int AbsorbPercentDeath { get; set; }
        public int AbsorbPercentDrown { get; set; }
        public int AbsorbPercentEarth { get; set; }
        public int AbsorbPercentEnergy { get; set; }
        public int AbsorbPercentFire { get; set; }
        public int AbsorbPercentHoly { get; set; }
        public int AbsorbPercentIce { get; set; }
        public int AbsorbPercentLifeDrain { get; set; }
        public int AbsorbPercentManaDrain { get; set; }
        public int AbsorbPercentMagic { get; set; }
        public int AbsorbPercentPhysical { get; set; }
        public bool SuppressDrunk { get; set; }
        public ItemField? Field { get; set; }
        
        public string NameWithId => $"{Id} - {Name}";
    }
}
