using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;
using Terraria.Localization;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items
{
    public class TysDartOverhaulGlobalItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            //Things that increase the damage of dart weapons
            if (item.useAmmo == AmmoID.Dart)
            {
                //Damage increases from the players dart damage stat
                if (player.GetModPlayer<TysDartOverhaulPlayer>().DartDamage > 0f)
                {
                    damage *= player.GetModPlayer<TysDartOverhaulPlayer>().DartDamage;
                }
            }
        }
    }
}