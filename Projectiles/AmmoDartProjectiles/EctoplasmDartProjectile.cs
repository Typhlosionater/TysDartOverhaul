using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class EctoplasmDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 

            Projectile.timeLeft = 600 * 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
			Projectile.alpha = 80;

			AIType = ProjectileID.PoisonDartBlowgun;

			Projectile.extraUpdates = 1;
		}

        public override Color? GetAlpha(Color lightColor)
		{
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
        }

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Spawns dust on death
			for (int i = 0; i < 4; i++)
			{
				int ImpactDust = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit);
				Main.dust[ImpactDust].velocity *= 1.5f;
				Main.dust[ImpactDust].scale *= 1.5f;
				Main.dust[ImpactDust].rotation = Main.rand.NextFloat(0, 4);
				Main.dust[ImpactDust].noGravity = true;
			}

			//Spawns 1-3 ectobolts
			if (Projectile.owner == Main.myPlayer)
			{
				int numberProjectiles = 1 + Main.rand.Next(2);
				numberProjectiles = numberProjectiles + Main.rand.Next(2);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 EctoshardAngle = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
					Vector2 EctoshardSpeed = (EctoshardAngle * 10) * Main.rand.NextFloat(0.7f, 0.9f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, EctoshardSpeed, ModContent.ProjectileType<AmmoDartEffects.EctoplasmDartEctoboltProjectile>(), Projectile.damage / 3, 0f, Projectile.owner);
				}
			}
        }
	}
}