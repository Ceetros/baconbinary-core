using System.Collections.Generic;
using System.IO;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Spr
{
    public class SprFile
    {
        public uint Signature { get; set; }
        public uint SpriteCount { get; set; }

        public Dictionary<uint, Sprite> Sprites { get; } = new();

        public SprFile()
        {
        }
    }
}
