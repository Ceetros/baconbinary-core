using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using BaconBinary.Core.Enum;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Otb
{
    public class ItemsXmlReader
    {
        public bool Read(string filePath, ServerItemList items)
        {
            if (!File.Exists(filePath))
                return false;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            try
            {
                XDocument xml = XDocument.Load(filePath);
                foreach (XElement element in xml.Root.Elements("item"))
                {
                    if (element.Attribute("id") != null)
                    {
                        ushort id = ushort.Parse(element.Attribute("id").Value);
                        if (items.TryGetValue(id, out ServerItem item))
                            ParseItem(item, element);
                    }
                    else if (element.Attribute("fromid") != null && element.Attribute("toid") != null)
                    {
                        ushort fromid = ushort.Parse(element.Attribute("fromid").Value);
                        ushort toid = ushort.Parse(element.Attribute("toid").Value);
                        for (ushort id = fromid; id <= toid; id++)
                        {
                            if (items.TryGetValue(id, out ServerItem item))
                                ParseItem(item, element);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        protected virtual bool ParseItem(ServerItem item, XElement element)
        {
            if (element.Attribute("name") != null)
                item.Name = element.Attribute("name").Value;
            
            if (element.Attribute("article") != null)
                item.Article = element.Attribute("article").Value;
            
            if (element.Attribute("editorsuffix") != null)
                item.EditorSuffix = element.Attribute("editorsuffix").Value;

            foreach (var attr in element.Elements("attribute"))
            {
                string key = attr.Attribute("key")?.Value;
                string value = attr.Attribute("value")?.Value;
                if (key == null || value == null) continue;

                switch (key)
                {
                    case "description":
                        item.Description = value;
                        break;
                    case "type":
                        item.ItemType = System.Enum.Parse<ItemType>(value, true);
                        break;
                    case "slotType":
                        item.SlotType = System.Enum.Parse<SlotType>(value, true);
                        break;
                    case "weaponType":
                        item.WeaponType = System.Enum.Parse<WeaponType>(value, true);
                        break;
                    case "ammoType":
                        item.AmmoType = System.Enum.Parse<AmmoType>(value, true);
                        break;
                    case "shootType":
                        item.ShootType = System.Enum.Parse<ShootType>(value, true);
                        break;
                    case "effect":
                        item.Effect = System.Enum.Parse<ShootType>(value, true);
                        break;
                    case "attack":
                        item.Attack = int.Parse(value);
                        break;
                    case "defense":
                        item.Defense = int.Parse(value);
                        break;
                    case "armor":
                        item.Armor = int.Parse(value);
                        break;
                    case "charges":
                        item.Charges = int.Parse(value);
                        break;
                    case "duration":
                        item.Duration = int.Parse(value);
                        break;
                    case "decayTo":
                        item.DecayTo = int.Parse(value);
                        break;
                    case "rotateTo":
                        item.RotateTo = int.Parse(value);
                        break;
                    case "range":
                        item.Range = int.Parse(value);
                        break;
                    case "weight":
                        item.Weight = int.Parse(value);
                        break;
                    case "worth":
                        item.Worth = int.Parse(value);
                        break;
                    case "speed":
                        item.Speed = int.Parse(value);
                        break;
                    case "healthGain":
                        item.HealthGain = uint.Parse(value);
                        break;
                    case "healthTicks":
                        item.HealthTicks = uint.Parse(value);
                        break;
                    case "manaGain":
                        item.ManaGain = uint.Parse(value);
                        break;
                    case "manaTicks":
                        item.ManaTicks = uint.Parse(value);
                        break;
                    case "maxHitChance":
                        item.MaxHitChance = int.Parse(value);
                        break;
                    case "hitChance":
                        item.HitChance = int.Parse(value);
                        break;
                    case "magiclevelpoints":
                        item.MagicLevelPoints = int.Parse(value);
                        break;
                    case "absorbPercentDeath":
                        item.AbsorbPercentDeath = int.Parse(value);
                        break;
                    case "absorbPercentDrown":
                        item.AbsorbPercentDrown = int.Parse(value);
                        break;
                    case "absorbPercentEarth":
                        item.AbsorbPercentEarth = int.Parse(value);
                        break;
                    case "absorbPercentEnergy":
                        item.AbsorbPercentEnergy = int.Parse(value);
                        break;
                    case "absorbPercentFire":
                        item.AbsorbPercentFire = int.Parse(value);
                        break;
                    case "absorbPercentHoly":
                        item.AbsorbPercentHoly = int.Parse(value);
                        break;
                    case "absorbPercentIce":
                        item.AbsorbPercentIce = int.Parse(value);
                        break;
                    case "absorbPercentLifeDrain":
                        item.AbsorbPercentLifeDrain = int.Parse(value);
                        break;
                    case "absorbPercentManaDrain":
                        item.AbsorbPercentManaDrain = int.Parse(value);
                        break;
                    case "absorbPercentMagic":
                        item.AbsorbPercentMagic = int.Parse(value);
                        break;
                    case "absorbPercentPhysical":
                        item.AbsorbPercentPhysical = int.Parse(value);
                        break;
                    case "suppressDrunk":
                        item.SuppressDrunk = bool.Parse(value);
                        break;
                    case "field":
                        item.Field = new ItemField { Type = (FieldType)System.Enum.Parse(typeof(FieldType), value, true) };
                        foreach (var fieldAttr in attr.Elements("attribute"))
                        {
                            string fieldKey = fieldAttr.Attribute("key")?.Value;
                            string fieldValue = fieldAttr.Attribute("value")?.Value;
                            if (fieldKey == null || fieldValue == null) continue;

                            switch (fieldKey)
                            {
                                case "damage":
                                    item.Field.Damage = int.Parse(fieldValue);
                                    break;
                                case "ticks":
                                    item.Field.Ticks = int.Parse(fieldValue);
                                    break;
                                case "count":
                                    item.Field.Count = int.Parse(fieldValue);
                                    break;
                                case "start":
                                    item.Field.Start = int.Parse(fieldValue);
                                    break;
                                case "initdamage":
                                    item.Field.InitDamage = int.Parse(fieldValue);
                                    break;
                            }
                        }
                        break;
                }
            }

            return true;
        }
    }
}
