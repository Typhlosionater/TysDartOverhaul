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
using Terraria.Audio;
using System.Linq;

namespace TysDartOverhaul.Projectiles
{
	public class TysDartOverhaulGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		//Vanilla Dart Changes
        public override void SetDefaults(Projectile projectile)
        {
			if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
			{
				//crystal darts use local immunity
				if (projectile.type == ProjectileID.CrystalDart)
				{
					projectile.usesLocalNPCImmunity = true;
					projectile.localNPCHitCooldown = 30;
				}

				//Ichor darts prior to splitting use local immunity
				if (projectile.type == ProjectileID.IchorDart)
				{
					projectile.usesLocalNPCImmunity = true;
					projectile.localNPCHitCooldown = 10;
				}

				//Cursed dart flames use static immunity
				if (projectile.type == ProjectileID.CursedDartFlame)
				{
					projectile.usesIDStaticNPCImmunity = true;
					projectile.idStaticNPCHitCooldown = 10;
				}
			}
		}
	}
}