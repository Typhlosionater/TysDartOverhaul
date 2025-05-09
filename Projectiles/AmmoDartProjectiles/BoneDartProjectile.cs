using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class BoneDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 

            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 255;

			AIType = ProjectileID.PoisonDartBlowgun;
		}

		int HitEnemy = -1;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			//Feeds enemy that caused shrapnel into shrapnel projectile
			HitEnemy = target.whoAmI;
        }

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 6;
			height = 6;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Shrapnel effect
			int numberProjectiles = 2 + Main.rand.Next(2);
			numberProjectiles = numberProjectiles + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(18));
				perturbedSpeed *= Main.rand.NextFloat(0.6f, 0.8f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<AmmoDartEffects.BoneDartShrapnelProjectile>(), Projectile.damage / 2, Projectile.knockBack / 3, Projectile.owner, 0, HitEnemy);
			}

			//Spawns dust and plays sound
			for (int i = 0; i < 5; i++)
			{
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(22));
				perturbedSpeed *= Main.rand.NextFloat(0.8f, 1f);
				int ShrapnelDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Tin);
				Main.dust[ShrapnelDust].noGravity = true;
				Main.dust[ShrapnelDust].velocity = perturbedSpeed;
			}
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
		}
	}
}