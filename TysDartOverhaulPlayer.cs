using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace TysDartOverhaul
{
    public class TysDartOverhaulPlayer : ModPlayer
    {
        //Initialise Special triggers
        public float DartDamage;

        public bool HuntsmanBandana;

        public bool PoisonedDarts;

        //Reset Triggers
        public override void ResetEffects()
        {
            DartDamage = 0f;

            HuntsmanBandana = false;

            PoisonedDarts = false;
        }

        //While dart damage != 0, increase damage of weapons that fire darts
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (DartDamage != 0f && item.useAmmo == AmmoID.Dart)
            {
                damage += DartDamage;
            }
        } 

        //While wearing a bandana accessory, dart weapons deal 2 extra knockback
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (HuntsmanBandana && item.useAmmo == AmmoID.Dart)
            {
                knockback.Flat += 2f;
            }
        }

        //While wearing a bandana accessory, dart ammo consumption is reduced by 20%
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (HuntsmanBandana && weapon.useAmmo == AmmoID.Dart && Main.rand.Next(5) == 0)
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo);
        }

        //Fishes up poison dart frog
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            //If modded dartguns are available
            if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
            {
                if (!attempt.inLava && !attempt.inHoney && Player.ZoneJungle && attempt.legendary && Main.hardMode && Main.rand.Next(2) == 0)
                {
                    npcSpawn = -1;
                    itemDrop = ModContent.ItemType<Items.Weapons.PoisonDartFrog>();
                }
            }
        }
    }
}
