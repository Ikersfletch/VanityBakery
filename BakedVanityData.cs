using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Chat;
using Terraria.Localization;
using Microsoft.Xna.Framework;
namespace VanityBakery
{
    public class BakedVanityData : ModPlayer
    {

        public static void SetItemAppearance(
          Player self,
          Item armorItem,
          Item dyeItem,
          ref bool setupWings,
          int slot = 3)
        {
            bool dyeItemValid = dyeItem != null && !dyeItem.IsAir && dyeItem.dye >= 0;

           // ChatHelper.DisplayMessageOnClient(NetworkText.FromLiteral($"{armorItem.type}"), Color.White, -1);

            if (armorItem.IsAir)
                return;

            if (self.head <= 0 && armorItem.headSlot > 0)
            {
                if (dyeItemValid) self.cHead = dyeItem.dye;
                self.head = armorItem.headSlot;
            }
            if (self.body <= 0 && armorItem.bodySlot > 0)
            {
                if (dyeItemValid) self.cBody = dyeItem.dye;
                self.body = armorItem.bodySlot;
            }
            if (self.legs <= 0 && armorItem.legSlot > 0)
            {
                if (dyeItemValid) self.cLegs = dyeItem.dye;
                self.legs = armorItem.legSlot;
            }


            if (self.handon <= 0 && armorItem.handOnSlot > (sbyte)0) 
            {
                if (dyeItemValid) self.cHandOn = (int)dyeItem.dye;
                self.handon = armorItem.handOnSlot;
            }
            if (self.handoff <= 0 && armorItem.handOffSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cHandOff = (int)dyeItem.dye;
                self.handoff = armorItem.handOffSlot;
            }
            if (self.back <= 0 && armorItem.backSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cBack = (int)dyeItem.dye;
                self.back = armorItem.backSlot;
            }
            if (self.front <= 0 && armorItem.frontSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cFront = (int)dyeItem.dye;
                self.front = armorItem.frontSlot;
            }
            if (self.shoe <= 0 && armorItem.shoeSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cShoe = (int)dyeItem.dye;
                self.shoe = armorItem.shoeSlot;
            }
            if (self.waist <= 0 && armorItem.waistSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cWaist = (int)dyeItem.dye;
                self.waist = armorItem.waistSlot;
            }
            if (self.shield <= 0 && armorItem.shieldSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cShield = (int)dyeItem.dye;
                self.shield = armorItem.shieldSlot;
            }
            if (self.neck <= 0 && armorItem.neckSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cNeck = (int)dyeItem.dye;
                self.neck = armorItem.neckSlot;
            }
            if (self.face <= 0 && armorItem.faceSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cFace = (int)dyeItem.dye;
                self.face = armorItem.faceSlot;
            }
            if (self.balloon <= 0 && armorItem.balloonSlot > (sbyte)0)
            {
                if (dyeItemValid) self.cBalloon = (int)dyeItem.dye;
                self.balloon = armorItem.balloonSlot;
            }
            if (!setupWings && armorItem.wingSlot > (sbyte)0)
            {
                // wings are wack, because the priority should go as follows:
                // 1) wings equipped in social
                // 2) wings equipped in functional
                // 3) baked wings

                // but wings still display when hidden (which is dumb)
                // which means the priority for them needs to be changed
                // 1) wings equipped in social
                // 2) non-hidden wings equipped in functional 
                // 3) baked wings

                // that small distinction means you can't just compare the self.wings value to see if the wings should be replaced, because the functional wings would overwrite it, even if hidden.
                // at least this 'always visible' crap only really applies to wings (and shields, damn it)
                bool useBakedWings = true;

                for (int i = 13; i < 20; i++)
                {
                    if (!self.IsAValidEquipmentSlotForIteration(i)) continue;
                    Item thisItem = self.armor[i];
                    if (thisItem == null || thisItem.IsAir) continue;
                    if (thisItem.wingSlot >= 0)
                    {
                        useBakedWings = false;
                        break;
                    }
                }

                for (int i = 3; i < 10; i++)
                {
                    if (!self.IsAValidEquipmentSlotForIteration(i) || self.hideVisibleAccessory[i]) continue;
                    Item thisItem = self.armor[i];
                    if (thisItem == null || thisItem.IsAir) continue;
                    if (thisItem.wingSlot >= 0)
                    {
                        useBakedWings = false;
                        break;
                    }
                }

                if (useBakedWings)
                {
                    if (dyeItemValid) self.cWings = (int)dyeItem.dye;
                    self.wings = armorItem.wingSlot;
                    setupWings = true;
                }
            }
            if (!self.carpet && armorItem.type == ItemID.FlyingCarpet)
            {
                if (self.cCarpet < 0 && dyeItemValid) self.cCarpet = (int)dyeItem.dye;
                self.carpet = true;
            }
            if (armorItem.type == ItemID.FloatingTube)
            {
                if (self.cFloatingTube < 0 && dyeItemValid) self.cFloatingTube = (int)dyeItem.dye;
                self.hasFloatingTube = true;
            }
            if (self.cPortalbeStool < 0 && armorItem.type == ItemID.PortableStool)
            {
                if (dyeItemValid) self.cPortalbeStool = (int)dyeItem.dye; // PORTALBE STOOL PORTALBE STOOL PORTALBE STOOL
            }
            if (!self.hasUnicornHorn && armorItem.type == ItemID.UnicornHornHat)
            {
                if (self.cUnicornHorn < 0 && dyeItemValid) self.cUnicornHorn = (int)dyeItem.dye;
                self.hasUnicornHorn = true;
            }
            if (self.cMinion < 0 && armorItem.type == ItemID.CritterShampoo)
            {
                if (dyeItemValid) self.cMinion = (int)dyeItem.dye;
            }
            if (!self.leinforsHair && armorItem.type == ItemID.LeinforsAccessory)
            {
                if (self.cLeinShampoo < 0 && dyeItemValid) self.cLeinShampoo = (int)dyeItem.dye;
                self.leinforsHair = true;
            }
            if (armorItem.type == ItemID.Yoraiz0rWings && self.yoraiz0rEye <= 0)
            {
                self.yoraiz0rEye = slot - 2;
            }
            if (armorItem.type == ItemID.Yoraiz0rDarkness)
            {
                self.yoraiz0rDarkness = true;
            }

        }


        public List<Tuple<Item,Item,int>> Appearance = new List<Tuple<Item,Item,int>>();
        public override void SaveData(TagCompound tag)
        {
            List<Item> BakedArmor = new List<Item>();
            List<Item> BakedDye = new List<Item>();
            List<int> BakedSlots = new List<int>();

            Appearance.ForEach(set => {
                BakedArmor.Add(set.Item1);
                BakedDye.Add(set.Item2);
                BakedSlots.Add(set.Item3);
            });

            tag.Add("BakedArmor", BakedArmor.Select(ItemIO.Save).ToList());
            tag.Add("BakedDye", BakedDye.Select(ItemIO.Save).ToList());
            tag.Add("BakedSlots", BakedSlots.ToList());
        }
        public override void LoadData(TagCompound tag)
        {
            Appearance.Clear();

            List<Item> BakedArmor = tag.GetList<TagCompound>("BakedArmor").Select(ItemIO.Load).ToList();
            List<Item> BakedDye = tag.GetList<TagCompound>("BakedDye").Select(ItemIO.Load).ToList();
            List<int> BakedSlots = tag.GetList<int>("BakedSlots").ToList();

            for (int i = 0; i < BakedArmor.Count; i++)
            {
                Item item = BakedArmor[i];
                Item dye = BakedDye.IndexInRange(i) ? BakedDye[i] : null;
                int slot = BakedSlots.IndexInRange(i) ? BakedSlots[i] : 3;

                Appearance.Add(new Tuple<Item, Item, int>(item,dye,slot));
            }
        }

        static public List<T> CloneList<T>(List<T> list)
        {
            List<T> clone = new List<T>();
            list.ForEach(item => clone.Add(item));
            return clone;
        }

        static public void ClearVanity(Player From)
        {
            if (From.GetModPlayer<BakedVanityData>().Appearance == null) return;
            From.GetModPlayer<BakedVanityData>().Appearance.Clear();
        }

        // transfer the appearance of one player to the other
        static public void BakeVanities(Player From, Player To)
        {
            List<Tuple<Item, Item, int>> VanityCache = 
                From.GetModPlayer<BakedVanityData>().Appearance == null ? 
                new List<Tuple<Item, Item, int>>() : CloneList(From.GetModPlayer<BakedVanityData>().Appearance);

            // the 'armor' array comprises
            // 1) the main 3 armor pieces
            // 2) the functional 7 accessories
            // 3) the 3 social armor pieces
            // 4) the 7 social accessories

            // the 'dye' array contains
            // 1) the 3 armor dye slots
            // 2) the 7 accessory dye slots


            // fill out the armor first, it should be lowest.
            for (int i = 0; i < 3; i ++)
            {
                bool socialEquippedHere = From.armor[i + 10] != null && !From.armor[i + 10].IsAir;
                bool functionalEquippedHere = From.armor[i] != null && !From.armor[i].IsAir;

                if (!socialEquippedHere && !functionalEquippedHere) continue; // nothing equipped? alright


                Item visualItem = From.armor[socialEquippedHere ? i + 10 : i];

                Tuple<Item, Item, int> set = new Tuple<Item, Item, int>(visualItem.Clone(), From.dye[i] == null ? null : From.dye[i].Clone(), i); // the null check is because Clone requires an instance to work

                VanityCache.Add(set);
            }


            // now the functional accessories
            for (int i = 3; i < 10; i ++)
            {
                Item equippedItem = From.armor[i];
                if (equippedItem == null || equippedItem.IsAir) continue; // skip this if the slot is empty
                if (From.hideVisibleAccessory[i] || !equippedItem.FitsAccessoryVanitySlot) continue; // if this is a functional accessory that is hidden or invisible, skip

                Tuple<Item, Item, int> set = new Tuple<Item, Item, int>(equippedItem.Clone(), From.dye[i] == null ? null : From.dye[i].Clone(), i);

                VanityCache.Add(set);
            }

            // now the social accessories
            for (int i = 13; i < 20; i ++)
            {
                Item equippedItem = From.armor[i];
                if (equippedItem == null || equippedItem.IsAir) continue; // skip this if the slot is empty

                Tuple<Item, Item, int> set = new Tuple<Item, Item, int>(equippedItem.Clone(),From.dye[i - 10] == null ? null : From.dye[i - 10].Clone(), i - 10);

                VanityCache.Add(set);
            }


            To.GetModPlayer<BakedVanityData>().Appearance.Clear();
            To.GetModPlayer<BakedVanityData>().Appearance = VanityCache;

        }
        public override void UpdateVisibleVanityAccessories()
        {
            bool SetupWings = false;
            for (int i = Appearance.Count - 1; i >= 0; i--)
            {
                SetItemAppearance(Player, Appearance[i].Item1, Appearance[i].Item2, ref SetupWings, Appearance[i].Item3);
            }
        }
    }
}
