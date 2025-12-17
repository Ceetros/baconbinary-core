using System;
using System.IO;
using System.Text;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Common;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Otb
{
    public class OtbReader
    {
        public ServerItemList Read(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("OTB file not found.", path);

            var items = new ServerItemList();
            
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var tree = new BinaryTreeReader(fileStream))
            {
                var root = tree.GetRootNode();
                if (root == null)
                    throw new InvalidDataException("Could not read root node from OTB file.");

                root.ReadByte(); // Node type
                uint flags = root.ReadUInt32();

                if ((flags & 0x01) != 0) // Has version info
                {
                    byte attr = root.ReadByte();
                    if ((RootAttribute)attr == RootAttribute.Version)
                    {
                        ushort datalen = root.ReadUInt16();
                        if (datalen != 140)
                            throw new InvalidDataException("Invalid OTB version header size.");

                        items.MajorVersion = root.ReadUInt32();
                        items.MinorVersion = root.ReadUInt32();
                        items.BuildNumber = root.ReadUInt32();
                        root.BaseStream.Seek(128, SeekOrigin.Current);
                    }
                }

                var itemNode = tree.GetChildNode();
                if (itemNode == null)
                    return items;

                do
                {
                    var item = new ServerItem();
                    var itemGroup = (ServerItemGroup)itemNode.ReadByte();
                    
                    switch (itemGroup)
                    {
                        case ServerItemGroup.None: item.Type = ServerItemType.None; break;
                        case ServerItemGroup.Ground: item.Type = ServerItemType.Ground; break;
                        case ServerItemGroup.Container: item.Type = ServerItemType.Container; break;
                        case ServerItemGroup.Splash: item.Type = ServerItemType.Splash; break;
                        case ServerItemGroup.Fluid: item.Type = ServerItemType.Fluid; break;
                        case ServerItemGroup.Deprecated: item.Type = ServerItemType.Deprecated; break;
                    }

                    var itemFlags = (ServerItemFlag)itemNode.ReadUInt32();
                    item.Unpassable = itemFlags.HasFlag(ServerItemFlag.Unpassable);
                    item.BlockMissiles = itemFlags.HasFlag(ServerItemFlag.BlockMissiles);
                    item.BlockPathfinder = itemFlags.HasFlag(ServerItemFlag.BlockPathfinder);
                    item.HasElevation = itemFlags.HasFlag(ServerItemFlag.HasElevation);
                    item.ForceUse = itemFlags.HasFlag(ServerItemFlag.ForceUse);
                    item.MultiUse = itemFlags.HasFlag(ServerItemFlag.MultiUse);
                    item.Pickupable = itemFlags.HasFlag(ServerItemFlag.Pickupable);
                    item.Movable = itemFlags.HasFlag(ServerItemFlag.Movable);
                    item.Stackable = itemFlags.HasFlag(ServerItemFlag.Stackable);
                    item.HasStackOrder = itemFlags.HasFlag(ServerItemFlag.StackOrder);
                    item.Readable = itemFlags.HasFlag(ServerItemFlag.Readable);
                    item.Rotatable = itemFlags.HasFlag(ServerItemFlag.Rotatable);
                    item.Hangable = itemFlags.HasFlag(ServerItemFlag.Hangable);
                    item.HookSouth = itemFlags.HasFlag(ServerItemFlag.HookSouth);
                    item.HookEast = itemFlags.HasFlag(ServerItemFlag.HookEast);
                    item.AllowDistanceRead = itemFlags.HasFlag(ServerItemFlag.AllowDistanceRead);
                    item.HasCharges = itemFlags.HasFlag(ServerItemFlag.ClientCharges);
                    item.IgnoreLook = itemFlags.HasFlag(ServerItemFlag.IgnoreLook);
                    item.FullGround = itemFlags.HasFlag(ServerItemFlag.FullGround);
                    item.IsAnimation = itemFlags.HasFlag(ServerItemFlag.IsAnimation);

                    while (itemNode.BaseStream.Position < itemNode.BaseStream.Length && itemNode.PeekChar() != -1)
                    {
                        var attribute = (ServerItemAttribute)itemNode.ReadByte();
                        ushort datalen = itemNode.ReadUInt16();

                        switch (attribute)
                        {
                            case ServerItemAttribute.ServerId: item.Id = itemNode.ReadUInt16(); break;
                            case ServerItemAttribute.ClientId: item.ClientId = itemNode.ReadUInt16(); break;
                            case ServerItemAttribute.GroundSpeed: item.GroundSpeed = itemNode.ReadUInt16(); break;
                            case ServerItemAttribute.Name: item.Name = Encoding.UTF8.GetString(itemNode.ReadBytes(datalen)); break;
                            case ServerItemAttribute.SpriteHash: item.SpriteHash = itemNode.ReadBytes(datalen); break;
                            case ServerItemAttribute.MinimaColor: item.MinimapColor = itemNode.ReadUInt16(); break;
                            case ServerItemAttribute.MaxReadWriteChars: item.MaxReadWriteChars = itemNode.ReadUInt16(); break;
                            case ServerItemAttribute.MaxReadChars: item.MaxReadChars = itemNode.ReadUInt16(); break;
                            case ServerItemAttribute.Light:
                                item.LightLevel = itemNode.ReadUInt16();
                                item.LightColor = itemNode.ReadUInt16();
                                break;
                            case ServerItemAttribute.StackOrder: item.StackOrder = (TileStackOrder)itemNode.ReadByte(); break;
                            case ServerItemAttribute.TradeAs: item.TradeAs = itemNode.ReadUInt16(); break;
                            default: itemNode.BaseStream.Seek(datalen, SeekOrigin.Current); break;
                        }
                    }

                    if (item.SpriteHash == null && item.Type != ServerItemType.Deprecated)
                    {
                        item.SpriteHash = new byte[16];
                    }

                    items.Add(item.Id, item);
                    itemNode = tree.GetNextNode();
                } while (itemNode != null);
            }

            return items;
        }
    }
}
