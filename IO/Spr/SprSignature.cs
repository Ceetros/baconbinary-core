using System;

namespace BaconBinary.Core.IO.Spr
{
    public static class SprSignature
    {
        public static byte[] GetSignature(string version)
        {
            var info = ClientVersionRepository.Get(version);
            
            if (info == null)
            {
                throw new ArgumentException($"Version '{version}' not found in Repository.");
            }

            return BitConverter.GetBytes(info.SprSignature);
        }
    }
}