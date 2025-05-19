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
    public class TysDartOverhaulGlobalItem : GlobalItem
    {
        //Similar to molten quiver: If the player has a predator's bandana, basic darts they shoot are converted into poison darts
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>() && player.GetModPlayer<TysDartOverhaulPlayer>().PoisonedDarts)
            {
                type = ProjectileID.PoisonDartBlowgun;
                damage += 2;
            }
        }
    }
}
