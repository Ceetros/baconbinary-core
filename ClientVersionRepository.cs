using System.Collections.Generic;
using System.Linq;

namespace BaconBinary.Core
{
    public class ClientVersion
    {
        public string Name { get; set; }
        public uint DatSignature { get; set; }
        public uint SprSignature { get; set; }
    }

    public static class ClientVersionRepository
    {

        private static readonly List<ClientVersion> _versions = new()
        {
            new ClientVersion { Name = "7.10", DatSignature = 0x3DFF4B2A, SprSignature = 0x3DFF4AEB },
            new ClientVersion { Name = "7.30", DatSignature = 0x411A6233, SprSignature = 0x411A6279 },
            new ClientVersion { Name = "7.40", DatSignature = 0x41BF619C, SprSignature = 0x41B9EA86 },
            new ClientVersion { Name = "7.50", DatSignature = 0x42F81973, SprSignature = 0x42F81949 },
            new ClientVersion { Name = "7.55", DatSignature = 0x437B2B8F, SprSignature = 0x434F9CDE },
            new ClientVersion { Name = "7.60", DatSignature = 0x439D5A33, SprSignature = 0x439852BE },
            new ClientVersion { Name = "7.70", DatSignature = 0x439D5A33, SprSignature = 0x439852BE },
            new ClientVersion { Name = "7.80", DatSignature = 0x44CE4743, SprSignature = 0x44CE4206 },
            new ClientVersion { Name = "7.90", DatSignature = 0x457D854E, SprSignature = 0x457957C8 },
            new ClientVersion { Name = "7.92", DatSignature = 0x459E7B73, SprSignature = 0x45880FE8 },
            new ClientVersion { Name = "8.00", DatSignature = 0x467FD7E6, SprSignature = 0x467F9E74 },
            new ClientVersion { Name = "8.10", DatSignature = 0x475D3747, SprSignature = 0x475D0B01 },
            new ClientVersion { Name = "8.11", DatSignature = 0x47F60E37, SprSignature = 0x47EBB9B2 },
            new ClientVersion { Name = "8.20", DatSignature = 0x486905AA, SprSignature = 0x4868ECC9 },
            new ClientVersion { Name = "8.30", DatSignature = 0x48DA1FB6, SprSignature = 0x48C8E712 },
            new ClientVersion { Name = "8.40", DatSignature = 0x493D607A, SprSignature = 0x493D4E7C },
            new ClientVersion { Name = "8.41", DatSignature = 0x49B7CC19, SprSignature = 0x49B140EA },
            new ClientVersion { Name = "8.42", DatSignature = 0x49C233C9, SprSignature = 0x49B140EA },
            

            new ClientVersion { Name = "8.50 v1", DatSignature = 0x4A49C5EB, SprSignature = 0x4A44FD4E },
            new ClientVersion { Name = "8.50 v2", DatSignature = 0x4A4CC0DC, SprSignature = 0x4A44FD4E },
            new ClientVersion { Name = "8.50 v3", DatSignature = 0x4AE97492, SprSignature = 0x4ACB5230 },
            
            new ClientVersion { Name = "8.52", DatSignature = 0x4A4CC0DC, SprSignature = 0x4A44FD4E },
            new ClientVersion { Name = "8.53", DatSignature = 0x4AE97492, SprSignature = 0x4ACB5230 },
            

            new ClientVersion { Name = "8.54 v1", DatSignature = 0x4B1E2CAA, SprSignature = 0x4B1E2C87 },
            new ClientVersion { Name = "8.54 v2", DatSignature = 0x4B0D46A9, SprSignature = 0x4B0D3AFF },
            new ClientVersion { Name = "8.54 v3", DatSignature = 0x4B28B89E, SprSignature = 0x4B1E2C87 },
            
            new ClientVersion { Name = "8.55", DatSignature = 0x4B98FF53, SprSignature = 0x4B913871 },
            

            new ClientVersion { Name = "8.60 v1", DatSignature = 0x4C28B721, SprSignature = 0x4C220594 },
            new ClientVersion { Name = "8.60 v2", DatSignature = 0x4C2C7993, SprSignature = 0x4C220594 },
            
            new ClientVersion { Name = "8.61", DatSignature = 0x4C6A4CBC, SprSignature = 0x4C63F145 },
            new ClientVersion { Name = "8.62", DatSignature = 0x4C973450, SprSignature = 0x4C63F145 },
            new ClientVersion { Name = "8.70", DatSignature = 0x4CFE22C5, SprSignature = 0x4CFD078A },
            new ClientVersion { Name = "8.71", DatSignature = 0x4D41979E, SprSignature = 0x4D3D65D0 },
            new ClientVersion { Name = "8.72", DatSignature = 0x4DAD1A1A, SprSignature = 0x4DAD1A32 },
            new ClientVersion { Name = "9.00", DatSignature = 0x4DBAA20B, SprSignature = 0x4DAD1A32 },
            new ClientVersion { Name = "9.10", DatSignature = 0x4E12DAFF, SprSignature = 0x4E12DB27 },
            new ClientVersion { Name = "9.20", DatSignature = 0x4E807C08, SprSignature = 0x4E807C23 },
            new ClientVersion { Name = "9.40", DatSignature = 0x4EE71DE5, SprSignature = 0x4EE71E06 },
            

            new ClientVersion { Name = "9.44 v0", DatSignature = 0x4F0EEFBB, SprSignature = 0x4F0EEFEF },
            new ClientVersion { Name = "9.44 v1", DatSignature = 0x4F105168, SprSignature = 0x4F1051D7 },
            new ClientVersion { Name = "9.44 v2", DatSignature = 0x4F16C0D7, SprSignature = 0x4F1051D7 },
            new ClientVersion { Name = "9.44 v3", DatSignature = 0x4F3131CF, SprSignature = 0x4F3131F6 },
            
            new ClientVersion { Name = "9.46", DatSignature = 0x4F75B7AB, SprSignature = 0x4F5DCEF7 },
            new ClientVersion { Name = "9.50", DatSignature = 0x4F75B7AB, SprSignature = 0x4F75B7CD },
            new ClientVersion { Name = "9.52", DatSignature = 0x4F857F6C, SprSignature = 0x4F857F8E },
            new ClientVersion { Name = "9.53", DatSignature = 0x4FA11252, SprSignature = 0x4FA11282 },
            new ClientVersion { Name = "9.54", DatSignature = 0x4FD5956B, SprSignature = 0x4FD595B7 },
            new ClientVersion { Name = "9.60", DatSignature = 0x4FFA74CC, SprSignature = 0x4FFA74F9 },
            new ClientVersion { Name = "9.61", DatSignature = 0x50226F9D, SprSignature = 0x50226FBD },
            new ClientVersion { Name = "9.63", DatSignature = 0x503CB933, SprSignature = 0x503CB954 },
            new ClientVersion { Name = "9.70", DatSignature = 0x5072A490, SprSignature = 0x5072A567 },
            new ClientVersion { Name = "9.80", DatSignature = 0x50C70674, SprSignature = 0x50C70753 },
            new ClientVersion { Name = "9.81", DatSignature = 0x50D1C5B6, SprSignature = 0x50D1C685 },
            new ClientVersion { Name = "9.82", DatSignature = 0x512CAD09, SprSignature = 0x512CAD68 },
            new ClientVersion { Name = "9.83", DatSignature = 0x51407B67, SprSignature = 0x51407BC7 },
            new ClientVersion { Name = "9.85", DatSignature = 0x51641A1B, SprSignature = 0x51641A84 },
            new ClientVersion { Name = "9.86", DatSignature = 0x5170E904, SprSignature = 0x5170E96F },
            

            new ClientVersion { Name = "10.10", DatSignature = 0x51E3F8C3, SprSignature = 0x51E3F8E9 },
            new ClientVersion { Name = "10.20", DatSignature = 0x5236F129, SprSignature = 0x5236F14F },
            new ClientVersion { Name = "10.21", DatSignature = 0x526A5068, SprSignature = 0x526A5090 },
            new ClientVersion { Name = "10.30", DatSignature = 0x52A59036, SprSignature = 0x52A5905F },
            new ClientVersion { Name = "10.31", DatSignature = 0x52AED581, SprSignature = 0x52AED5A7 },
            new ClientVersion { Name = "10.32", DatSignature = 0x52D8D0A9, SprSignature = 0x52D8D0CE },
            new ClientVersion { Name = "10.34", DatSignature = 0x52E74AB5, SprSignature = 0x52E74ADA },
            new ClientVersion { Name = "10.35", DatSignature = 0x52FDFC2C, SprSignature = 0x52FDFC54 },
            new ClientVersion { Name = "10.36", DatSignature = 0x53159C7E, SprSignature = 0x53159CA9 },
            new ClientVersion { Name = "10.37", DatSignature = 0x531EA82E, SprSignature = 0x531EA856 },
            new ClientVersion { Name = "10.38", DatSignature = 0x5333C199, SprSignature = 0x5333C1C3 },
            new ClientVersion { Name = "10.39", DatSignature = 0x535A50AD, SprSignature = 0x535A50D5 },
            new ClientVersion { Name = "10.40", DatSignature = 0x5379984D, SprSignature = 0x53799876 },
            new ClientVersion { Name = "10.41", DatSignature = 0x5383504E, SprSignature = 0x53835077 },
            new ClientVersion { Name = "10.50", DatSignature = 0x53B6460E, SprSignature = 0x53B64639 },
            new ClientVersion { Name = "10.51", DatSignature = 0x53C8CC17, SprSignature = 0x53C8CC3F },
            new ClientVersion { Name = "10.52", DatSignature = 0x53E898BD, SprSignature = 0x53E898E5 },
            new ClientVersion { Name = "10.53", DatSignature = 0x53FAD76E, SprSignature = 0x53FAD799 },
            new ClientVersion { Name = "10.54", DatSignature = 0x540D3A47, SprSignature = 0x53E898E5 },
            new ClientVersion { Name = "10.55", DatSignature = 0x54128727, SprSignature = 0x54128755 },
            new ClientVersion { Name = "10.56", DatSignature = 0x542143B0, SprSignature = 0x542143DE },
            new ClientVersion { Name = "10.57", DatSignature = 0x542535F9, SprSignature = 0x54253627 },
            new ClientVersion { Name = "10.58", DatSignature = 0x542D12E7, SprSignature = 0x542D1315 },
            new ClientVersion { Name = "10.59", DatSignature = 0x5434084B, SprSignature = 0x54340879 },
            new ClientVersion { Name = "10.60", DatSignature = 0x5448D9C7, SprSignature = 0x5448DA10 },
            new ClientVersion { Name = "10.61", DatSignature = 0x5448D9C7, SprSignature = 0x5448DA10 },
            new ClientVersion { Name = "10.62", DatSignature = 0x54622638, SprSignature = 0x54622667 },
            new ClientVersion { Name = "10.63", DatSignature = 0x546B502A, SprSignature = 0x546B505E },
            new ClientVersion { Name = "10.64", DatSignature = 0x547F05BE, SprSignature = 0x547F0632 },
            new ClientVersion { Name = "10.70", DatSignature = 0x5481BB97, SprSignature = 0x5481BC06 },
            

            new ClientVersion { Name = "10.71", DatSignature = 0x334F, SprSignature = 0x548E9EFE },
            new ClientVersion { Name = "10.72", DatSignature = 0x3729, SprSignature = 0x54B37B99 },
            new ClientVersion { Name = "10.73", DatSignature = 0x374D, SprSignature = 0x54BC95AE },
            new ClientVersion { Name = "10.74", DatSignature = 0x375E, SprSignature = 0x54C5FAB2 },
            new ClientVersion { Name = "10.75", DatSignature = 0x3775, SprSignature = 0x54D85085 },
            new ClientVersion { Name = "10.76", DatSignature = 0x37DF, SprSignature = 0x54F03CE9 },
            new ClientVersion { Name = "10.77", DatSignature = 0x38DE, SprSignature = 0x5525213D },
            new ClientVersion { Name = "10.90", DatSignature = 0x3F26, SprSignature = 0x565EE171 },
            new ClientVersion { Name = "10.91", DatSignature = 0x3F81, SprSignature = 0x56BC8198 },
            new ClientVersion { Name = "10.92", DatSignature = 0x4086, SprSignature = 0x570742B8 },
            new ClientVersion { Name = "10.93 test", DatSignature = 0x40FF, SprSignature = 0x57161DEA },
            new ClientVersion { Name = "10.93", DatSignature = 0x413F, SprSignature = 0x5726E657 },
            new ClientVersion { Name = "10.94", DatSignature = 0x41E5, SprSignature = 0x57459D43 },
            new ClientVersion { Name = "10.95", DatSignature = 0x41F3, SprSignature = 0x575A84BD },
            new ClientVersion { Name = "10.98", DatSignature = 0x42A3, SprSignature = 0x57BBD603 },
            new ClientVersion { Name = "10.99", DatSignature = 0x4347, SprSignature = 0x57FF106B },
            
  
            new ClientVersion { Name = "12.86", DatSignature = 0x4A10, SprSignature = 0x59E48E02 }
        };


        public static ClientVersion Get(string version)
        {
            return _versions.FirstOrDefault(v => v.Name == version);
        }
        public static string DetectVersion(string datPath)
        {
            try 
            {
                using (var fs = new System.IO.FileStream(datPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                using (var reader = new System.IO.BinaryReader(fs))
                {
                    uint signature = reader.ReadUInt32();
                    var match = _versions.FirstOrDefault(v => v.DatSignature == signature);
                    return match?.Name;
                }
            }
            catch { return null; }
        }
        
        public static List<string> GetAllVersions()
        {
            return _versions.Select(v => v.Name).ToList();
        }
    }
}