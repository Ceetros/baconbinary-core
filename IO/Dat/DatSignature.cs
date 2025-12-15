using System;

namespace BaconBinary.Core.IO.Dat
{
    public static class DatSignature
    {
        public static uint GetSignature(string version)
        {
            var info = ClientVersionRepository.Get(version);
            
            if (info == null)
            {
                throw new ArgumentException($"Version '{version}' not found in Repository.");
            }
            
            return info.DatSignature;
        }
    }
}