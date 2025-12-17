using System;
using System.IO;
using BaconBinary.Core.Enum;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Common
{
    public class BinaryTreeWriter : IDisposable
    {
        private readonly BinaryWriter writer;

        public BinaryTreeWriter(Stream stream)
        {
            writer = new BinaryWriter(stream);
            Disposed = false;
        }

        public bool Disposed { get; private set; }

        public void CreateNode(byte type)
        {
            WriteByte((byte)SpecialChar.NodeStart, false);
            WriteByte(type);
        }

        public void WriteByte(byte value)
        {
            WriteBytes(new byte[1] { value }, true);
        }

        public void WriteByte(byte value, bool unescape)
        {
            WriteBytes(new byte[1] { value }, unescape);
        }

        public void WriteUInt16(ushort value)
        {
            WriteBytes(BitConverter.GetBytes(value), true);
        }

        public void WriteUInt16(ushort value, bool unescape)
        {
            WriteBytes(BitConverter.GetBytes(value), unescape);
        }

        public void WriteUInt32(uint value)
        {
            WriteBytes(BitConverter.GetBytes(value), true);
        }

        public void WriteUInt32(uint value, bool unescape)
        {
            WriteBytes(BitConverter.GetBytes(value), unescape);
        }

        public void WriteProp(ServerItemAttribute attribute, MemoryStream propertyStream)
        {
            propertyStream.Position = 0;
            byte[] bytes = new byte[propertyStream.Length];
            propertyStream.Read(bytes, 0, (int)propertyStream.Length);
            propertyStream.Position = 0;
            propertyStream.SetLength(0);

            WriteProp((byte)attribute, bytes);
        }

        public void WriteProp(RootAttribute attribute, MemoryStream propertyStream)
        {
            propertyStream.Position = 0;
            byte[] bytes = new byte[propertyStream.Length];
            propertyStream.Read(bytes, 0, (int)propertyStream.Length);
            propertyStream.Position = 0;
            propertyStream.SetLength(0);

            WriteProp((byte)attribute, bytes);
        }

        public void WriteBytes(byte[] bytes, bool unescape)
        {
            foreach (byte b in bytes)
            {
                if (unescape && (b == (byte)SpecialChar.NodeStart || b == (byte)SpecialChar.NodeEnd || b == (byte)SpecialChar.EscapeChar))
                {
                    writer.BaseStream.WriteByte((byte)SpecialChar.EscapeChar);
                }

                writer.BaseStream.WriteByte(b);
            }
        }

        public void CloseNode()
        {
            WriteByte((byte)SpecialChar.NodeEnd, false);
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                Disposed = true;
            }
        }

        private void WriteProp(byte attr, byte[] bytes)
        {
            WriteByte(attr);
            WriteUInt16((ushort)bytes.Length);
            WriteBytes(bytes, true);
        }
    }
}
