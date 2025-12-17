using System.Collections.Generic;

namespace BaconBinary.Core.Models
{
    public class ServerItemList : Dictionary<uint, ServerItem>
    {
        public uint MajorVersion { get; set; }
        public uint MinorVersion { get; set; }
        public uint BuildNumber { get; set; }
        public uint ClientVersion { get; set; }
    }
}
