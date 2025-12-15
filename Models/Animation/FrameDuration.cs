namespace BaconBinary.Core.Models.Animation
{
    public struct FrameDuration
    {
        public uint Minimum { get; set; }
        public uint Maximum { get; set; }

        public FrameDuration(uint min, uint max)
        {
            Minimum = min;
            Maximum = max;
        }
    }
}