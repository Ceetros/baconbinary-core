using System;
using System.IO;
using BaconBinary.Core.Enum;

namespace BaconBinary.Core.IO.Common
{
    public class BinaryTreeReader : IDisposable
    {
        private BinaryReader reader;
        private long currentNodePosition;
        private uint currentNodeSize;

        public BinaryTreeReader(Stream stream)
        {
            reader = new BinaryReader(stream);
            Disposed = false;
        }

        public bool Disposed { get; private set; }

        public BinaryReader GetRootNode()
        {
            return GetChildNode();
        }

        public BinaryReader GetChildNode()
        {
            Advance();
            return GetNodeData();
        }

        public BinaryReader GetNextNode()
        {
            reader.BaseStream.Seek(currentNodePosition, SeekOrigin.Begin);

            SpecialChar value = (SpecialChar)reader.ReadByte();
            if (value != SpecialChar.NodeStart)
            {
                return null;
            }

            value = (SpecialChar)reader.ReadByte();

            int level = 1;
            while (true)
            {
                value = (SpecialChar)reader.ReadByte();
                if (value == SpecialChar.NodeEnd)
                {
                    --level;
                    if (level == 0)
                    {
                        if (reader.BaseStream.Position == reader.BaseStream.Length)
                            return null;
                            
                        value = (SpecialChar)reader.ReadByte();
                        if (value == SpecialChar.NodeEnd)
                        {
                            return null;
                        }
                        else if (value != SpecialChar.NodeStart)
                        {
                            return null;
                        }
                        else
                        {
                            currentNodePosition = reader.BaseStream.Position - 1;
                            return GetNodeData();
                        }
                    }
                }
                else if (value == SpecialChar.NodeStart)
                {
                    ++level;
                }
                else if (value == SpecialChar.EscapeChar)
                {
                    reader.ReadByte();
                }
            }
        }

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
                Disposed = true;
            }
        }

        private BinaryReader GetNodeData()
        {
            reader.BaseStream.Seek(currentNodePosition, SeekOrigin.Begin);

            byte value = reader.ReadByte();

            if ((SpecialChar)value != SpecialChar.NodeStart)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream(200);

            currentNodeSize = 0;
            while (true)
            {
                value = reader.ReadByte();
                if ((SpecialChar)value == SpecialChar.NodeEnd || (SpecialChar)value == SpecialChar.NodeStart)
                {
                    break;
                }
                else if ((SpecialChar)value == SpecialChar.EscapeChar)
                {
                    value = reader.ReadByte();
                }

                currentNodeSize++;
                ms.WriteByte(value);
            }

            reader.BaseStream.Seek(currentNodePosition, SeekOrigin.Begin);
            ms.Position = 0;
            return new BinaryReader(ms);
        }

        private bool Advance()
        {
            try
            {
                long seekPos = 0;
                if (currentNodePosition == 0)
                {
                    seekPos = 4;
                }
                else
                {
                    seekPos = currentNodePosition;
                }

                reader.BaseStream.Seek(seekPos, SeekOrigin.Begin);

                SpecialChar value = (SpecialChar)reader.ReadByte();
                if (value != SpecialChar.NodeStart)
                {
                    return false;
                }

                if (currentNodePosition == 0)
                {
                    currentNodePosition = reader.BaseStream.Position - 1;
                    return true;
                }
                else
                {
                    value = (SpecialChar)reader.ReadByte();

                    while (true)
                    {
                        value = (SpecialChar)reader.ReadByte();
                        if (value == SpecialChar.NodeEnd)
                        {
                            return false;
                        }
                        else if (value == SpecialChar.NodeStart)
                        {
                            currentNodePosition = reader.BaseStream.Position - 1;
                            return true;
                        }
                        else if (value == SpecialChar.EscapeChar)
                        {
                            reader.ReadByte();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
