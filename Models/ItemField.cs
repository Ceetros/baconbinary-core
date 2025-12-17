using BaconBinary.Core.Enum;

namespace BaconBinary.Core.Models
{
    public class ItemField
    {
        public FieldType Type { get; set; }
        public int Damage { get; set; }
        public int Ticks { get; set; }
        public int Count { get; set; }
        public int Start { get; set; }
        public int InitDamage { get; set; }
    }
}
