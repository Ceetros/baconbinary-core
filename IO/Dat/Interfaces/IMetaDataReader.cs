using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Dat.Interfaces
{
    public interface IMetadataReader
    {
        /// <summary>
        /// Reads flags and boolean properties (e.g., IsGround, IsStackable).
        /// </summary>
        bool ReadProperties(ClientBinaryReader reader, ThingType type);

        /// <summary>
        /// Reads the graphic structure: Width, Height, Layers, Frames and SpriteIDs.
        /// </summary>
        void ReadTexturePatterns(ClientBinaryReader reader, ThingType type);
    }
}