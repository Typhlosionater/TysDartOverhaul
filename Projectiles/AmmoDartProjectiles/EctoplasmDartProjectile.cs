using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class EctoplasmDartProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ectoplasm Dart");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 
            Projectile.hostile = false;

            Projectile.timeLeft = 600 * 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 6;
			Projectile.Opacity = 0.2f;

			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			AIType = ProjectileID.PoisonDartBlowgun;

			Projectile.extraUpdates = 1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//Damage gets increased after each hit
			Projectile.damage = (int)(Projectile.damage * 1.2f);
        }

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			//Damage gets increased after each hit
			Projectile.damage = (int)(Projectile.damage * 1.2f);
        }

		public override void AI()
		{
			//Gets more visible as it pierces
			Projectile.Opacity = 0.25f + ((6 - Projectile.penetrate) / 20f);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			//Uneffected by lighting
			return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void Kill(int timeLeft)
		{
			//Spawns dust on death
			for (int i = 0; i < (2 + Math.Ceiling((6 - Projectile.penetrate) / 2f)); i++)
			{
				int ImpactDust = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit);
				Main.dust[ImpactDust].velocity *= 1.5f;
				Main.dust[ImpactDust].scale *= 1.5f;
				Main.dust[ImpactDust].rotation = Main.rand.NextFloat(0, 4);
				Main.dust[ImpactDust].noGravity = true;
			}
		}
	}
}