using System;
using System.IO;
using System.Text;
using BaconBinary.Core.Enum;
using BaconBinary.Core.IO.Common;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Otb
{
    public class OtbWriter
    {
        private readonly ServerItemList _items;

        public OtbWriter(ServerItemList items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public void Write(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryTreeWriter(fileStream))
            {
                writer.WriteUInt32(0, false); // Placeholder for root node size

                writer.CreateNode(0);
                writer.WriteUInt32(1, true); // Flags: Has version info

                var vi = new OtbVersionInfo
                {
                    MajorVersion = _items.MajorVersion,
                    MinorVersion = _items.MinorVersion,
                    BuildNumber = _items.BuildNumber,
                    CSDVersion = $"OTB {_items.MajorVersion}.{_items.MinorVersion}.{_items.BuildNumber}-{_items.ClientVersion / 100}.{_items.ClientVersion % 100}"
                };

                using (var ms = new MemoryStream())
                {
                    using (var property = new BinaryWriter(ms, Encoding.Default, true))
                    {
                        property.Write(vi.MajorVersion);
                        property.Write(vi.MinorVersion);
                        property.Write(vi.BuildNumber);
                        byte[] csdVersionBytes = Encoding.ASCII.GetBytes(vi.CSDVersion);
                        Array.Resize(ref csdVersionBytes, 128);
                        property.Write(csdVersionBytes);
                    }
                    writer.WriteProp(RootAttribute.Version, ms);

                    foreach (ServerItem item in _items.Values)
                    {
                        ServerItemGroup group;
                        switch (item.Type)
                        {
                            case ServerItemType.Container: group = ServerItemGroup.Container; break;
                            case ServerItemType.Fluid: group = ServerItemGroup.Fluid; break;
                            case ServerItemType.Ground: group = ServerItemGroup.Ground; break;
                            case ServerItemType.Splash: group = ServerItemGroup.Splash; break;
                            case ServerItemType.Deprecated: group = ServerItemGroup.Deprecated; break;
                            default: group = ServerItemGroup.None; break;
                        }
                        writer.CreateNode((byte)group);

                        uint flags = 0;
                        if (item.Unpassable) flags |= (uint)ServerItemFlag.Unpassable;
                        if (item.BlockMissiles) flags |= (uint)ServerItemFlag.BlockMissiles;
                        if (item.BlockPathfinder) flags |= (uint)ServerItemFlag.BlockPathfinder;
                        if (item.HasElevation) flags |= (uint)ServerItemFlag.HasElevation;
                        if (item.ForceUse) flags |= (uint)ServerItemFlag.ForceUse;
                        if (item.MultiUse) flags |= (uint)ServerItemFlag.MultiUse;
                        if (item.Pickupable) flags |= (uint)ServerItemFlag.Pickupable;
                        if (item.Movable) flags |= (uint)ServerItemFlag.Movable;
                        if (item.Stackable) flags |= (uint)ServerItemFlag.Stackable;
                        if (item.HasStackOrder) flags |= (uint)ServerItemFlag.StackOrder;
                        if (item.Readable) flags |= (uint)ServerItemFlag.Readable;
                        if (item.Rotatable) flags |= (uint)ServerItemFlag.Rotatable;
                        if (item.Hangable) flags |= (uint)ServerItemFlag.Hangable;
                        if (item.HookSouth) flags |= (uint)ServerItemFlag.HookSouth;
                        if (item.HookEast) flags |= (uint)ServerItemFlag.HookEast;
                        if (item.HasCharges) flags |= (uint)ServerItemFlag.ClientCharges;
                        if (item.IgnoreLook) flags |= (uint)ServerItemFlag.IgnoreLook;
                        if (item.AllowDistanceRead) flags |= (uint)ServerItemFlag.AllowDistanceRead;
                        if (item.IsAnimation) flags |= (uint)ServerItemFlag.IsAnimation;
                        if (item.FullGround) flags |= (uint)ServerItemFlag.FullGround;
                        writer.WriteUInt32(flags, true);

                        using (var propMs = new MemoryStream())
                        {
                            using (var propertyWriter = new BinaryWriter(propMs, Encoding.Default, true))
                            {
                                propertyWriter.Write(item.Id);
                            }
                            writer.WriteProp(ServerItemAttribute.ServerId, propMs);
                        }

                        if (item.Type != ServerItemType.Deprecated)
                        {
                            using (var propMs = new MemoryStream())
                            {
                                using (var propertyWriter = new BinaryWriter(propMs, Encoding.Default, true))
                                {
                                    propertyWriter.Write(item.ClientId);
                                }
                                writer.WriteProp(ServerItemAttribute.ClientId, propMs);
                            }

                            if (item.SpriteHash != null)
                            {
                                using (var propMs = new MemoryStream())
                                {
                                    using (var propertyWriter = new BinaryWriter(propMs, Encoding.Default, true))
                                    {
                                        propertyWriter.Write(item.SpriteHash);
                                    }
                                    writer.WriteProp(ServerItemAttribute.SpriteHash, propMs);
                                }
                            }
                            
                            if (!string.IsNullOrEmpty(item.Name))
                            {
                                using (var propMs = new MemoryStream())
                                {
                                    using (var propertyWriter = new BinaryWriter(propMs, Encoding.Default, true))
                                    {
                                        propertyWriter.Write(Encoding.UTF8.GetBytes(item.Name));
                                    }
                                    writer.WriteProp(ServerItemAttribute.Name, propMs);
                                }
                            }
                            
                            if (item.GroundSpeed > 0)
                            {
                                using (var propMs = new MemoryStream())
                                {
                                    using (var propertyWriter = new BinaryWriter(propMs, Encoding.Default, true))
                                    {
                                        propertyWriter.Write(item.GroundSpeed);
                                    }
                                    writer.WriteProp(ServerItemAttribute.GroundSpeed, propMs);
                                }
                            }
                            
                            if (item.LightLevel > 0 || item.LightColor > 0)
                            {
                                using (var propMs = new MemoryStream())
                                {
                                    using (var propertyWriter = new BinaryWriter(propMs, Encoding.Default, true))
                                    {
                                        propertyWriter.Write(item.LightLevel);
                                        propertyWriter.Write(item.LightColor);
                                    }
                                    writer.WriteProp(ServerItemAttribute.Light, propMs);
                                }
                            }
                        }
                        writer.CloseNode();
                    }
                }
                writer.CloseNode();
            }
        }
    }
}
