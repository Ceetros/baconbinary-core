using System;
using System.IO;
using System.Text;

namespace BaconBinary.Core.IO
{
    public class ClientBinaryReader : BinaryReader
    {
        public ClientBinaryReader(Stream input) : base(input) { }
        
        public ushort ReadU16()
        {
            return ReadUInt16();
        }
        
        public string ReadStringU16()
        {
            ushort length = ReadUInt16();
            if (length == 0) return string.Empty;
            
            byte[] bytes = ReadBytes(length);
            return Encoding.Latin1.GetString(bytes); 
        }
        public string ReadStringU16(ushort len)
        {
            byte[] bytes = ReadBytes(len);
            return Encoding.Latin1.GetString(bytes); 
        }
    }
}