using System;

namespace BaconBinary.Core
{
    public static class ClientFeatures
    {
        public static bool Extended { get; set; }
        public static bool FrameGroups { get; set; }
        public static bool FrameDurations { get; set; }
        public static bool Transparency { get; set; }
        
        public static void SetVersion(int version)
        {
            Extended = false;
            FrameGroups = false;
            FrameDurations = false;
            Transparency = false;
            
            if (version >= 960)
            {
                Extended = true;
            }
            
            if (version >= 1010)
            {
                FrameGroups = true;
            }

            if (version >= 1050)
            {
                FrameDurations = true;
            }
            
            Transparency = false; 
        }
    }
}