using System.Collections.Generic;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat
{
    public class DatFile
    {
        public Dictionary<uint, ThingType> Items { get; private set; } = new();
        public Dictionary<uint, ThingType> Outfits { get; private set; } = new();
        public Dictionary<uint, ThingType> Effects { get; private set; } = new();
        public Dictionary<uint, ThingType> Missiles { get; private set; } = new();
        
        public void Clear()
        {
            Items.Clear();
            Outfits.Clear();
            Effects.Clear();
            Missiles.Clear();
        }
    }
}