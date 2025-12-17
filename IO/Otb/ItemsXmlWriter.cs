using System;
using System.Xml.Linq;
using BaconBinary.Core.Enum;
using BaconBinary.Core.Models;

namespace BaconBinary.Core.IO.Otb
{
    public class ItemsXmlWriter
    {
        private readonly ServerItemList _items;

        public ItemsXmlWriter(ServerItemList items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public void Write(string filePath)
        {
            var root = new XElement("items");

            foreach (var item in _items.Values)
            {
                var itemElement = new XElement("item");
                itemElement.SetAttributeValue("id", item.Id);
                
                if (!string.IsNullOrEmpty(item.Name))
                    itemElement.SetAttributeValue("name", item.Name);

                if (!string.IsNullOrEmpty(item.Article))
                    itemElement.SetAttributeValue("article", item.Article);

                if (!string.IsNullOrEmpty(item.EditorSuffix))
                    itemElement.SetAttributeValue("editorsuffix", item.EditorSuffix);

                if (!string.IsNullOrEmpty(item.Description))
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "description"), new XAttribute("value", item.Description)));

                if (item.ItemType != ItemType.None)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "type"), new XAttribute("value", item.ItemType.ToString().ToLower())));
                
                if (item.SlotType != SlotType.None)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "slotType"), new XAttribute("value", item.SlotType.ToString().ToLower())));

                if (item.WeaponType != WeaponType.None)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "weaponType"), new XAttribute("value", item.WeaponType.ToString().ToLower())));

                if (item.AmmoType != AmmoType.None)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "ammoType"), new XAttribute("value", item.AmmoType.ToString().ToLower())));

                if (item.ShootType != ShootType.None)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "shootType"), new XAttribute("value", item.ShootType.ToString().ToLower())));

                if (item.Effect != ShootType.None)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "effect"), new XAttribute("value", item.Effect.ToString().ToLower())));

                if (item.Attack > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "attack"), new XAttribute("value", item.Attack)));

                if (item.Defense > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "defense"), new XAttribute("value", item.Defense)));

                if (item.Armor > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "armor"), new XAttribute("value", item.Armor)));

                if (item.Charges > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "charges"), new XAttribute("value", item.Charges)));

                if (item.Duration > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "duration"), new XAttribute("value", item.Duration)));

                if (item.DecayTo > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "decayTo"), new XAttribute("value", item.DecayTo)));

                if (item.RotateTo > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "rotateTo"), new XAttribute("value", item.RotateTo)));

                if (item.Range > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "range"), new XAttribute("value", item.Range)));

                if (item.Weight > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "weight"), new XAttribute("value", item.Weight)));

                if (item.Worth > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "worth"), new XAttribute("value", item.Worth)));

                if (item.Speed > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "speed"), new XAttribute("value", item.Speed)));

                if (item.HealthGain > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "healthGain"), new XAttribute("value", item.HealthGain)));

                if (item.HealthTicks > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "healthTicks"), new XAttribute("value", item.HealthTicks)));

                if (item.ManaGain > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "manaGain"), new XAttribute("value", item.ManaGain)));

                if (item.ManaTicks > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "manaTicks"), new XAttribute("value", item.ManaTicks)));

                if (item.MaxHitChance > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "maxHitChance"), new XAttribute("value", item.MaxHitChance)));

                if (item.HitChance > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "hitChance"), new XAttribute("value", item.HitChance)));

                if (item.MagicLevelPoints > 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "magiclevelpoints"), new XAttribute("value", item.MagicLevelPoints)));

                if (item.AbsorbPercentDeath != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentDeath"), new XAttribute("value", item.AbsorbPercentDeath)));

                if (item.AbsorbPercentDrown != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentDrown"), new XAttribute("value", item.AbsorbPercentDrown)));

                if (item.AbsorbPercentEarth != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentEarth"), new XAttribute("value", item.AbsorbPercentEarth)));

                if (item.AbsorbPercentEnergy != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentEnergy"), new XAttribute("value", item.AbsorbPercentEnergy)));

                if (item.AbsorbPercentFire != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentFire"), new XAttribute("value", item.AbsorbPercentFire)));

                if (item.AbsorbPercentHoly != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentHoly"), new XAttribute("value", item.AbsorbPercentHoly)));

                if (item.AbsorbPercentIce != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentIce"), new XAttribute("value", item.AbsorbPercentIce)));

                if (item.AbsorbPercentLifeDrain != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentLifeDrain"), new XAttribute("value", item.AbsorbPercentLifeDrain)));

                if (item.AbsorbPercentManaDrain != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentManaDrain"), new XAttribute("value", item.AbsorbPercentManaDrain)));

                if (item.AbsorbPercentMagic != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentMagic"), new XAttribute("value", item.AbsorbPercentMagic)));

                if (item.AbsorbPercentPhysical != 0)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "absorbPercentPhysical"), new XAttribute("value", item.AbsorbPercentPhysical)));

                if (item.SuppressDrunk)
                    itemElement.Add(new XElement("attribute", new XAttribute("key", "suppressDrunk"), new XAttribute("value", item.SuppressDrunk.ToString().ToLower())));

                if (item.Field != null)
                {
                    var fieldElement = new XElement("attribute", new XAttribute("key", "field"), new XAttribute("value", item.Field.Type.ToString().ToLower()));
                    if (item.Field.Damage > 0)
                        fieldElement.Add(new XElement("attribute", new XAttribute("key", "damage"), new XAttribute("value", item.Field.Damage)));
                    if (item.Field.Ticks > 0)
                        fieldElement.Add(new XElement("attribute", new XAttribute("key", "ticks"), new XAttribute("value", item.Field.Ticks)));
                    if (item.Field.Count > 0)
                        fieldElement.Add(new XElement("attribute", new XAttribute("key", "count"), new XAttribute("value", item.Field.Count)));
                    if (item.Field.Start > 0)
                        fieldElement.Add(new XElement("attribute", new XAttribute("key", "start"), new XAttribute("value", item.Field.Start)));
                    if (item.Field.InitDamage > 0)
                        fieldElement.Add(new XElement("attribute", new XAttribute("key", "initdamage"), new XAttribute("value", item.Field.InitDamage)));
                    itemElement.Add(fieldElement);
                }

                root.Add(itemElement);
            }

            var xml = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);
            xml.Save(filePath);
        }
    }
}
