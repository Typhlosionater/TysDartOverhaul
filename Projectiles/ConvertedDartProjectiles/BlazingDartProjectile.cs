using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.ConvertedDartProjectiles
{
	public class BlazingDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true; 

            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
			Projectile.alpha = 80;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			//Uneffected by lighting
			return new Color(200, 200, 200, 25);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Inflicts 15 seconds of hellfire on hit enemies
			target.AddBuff(BuffID.OnFire3, 15 * 60);

			//Explode if it has not yet
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3;
			}
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //Inflicts 15 seconds of hellfire on hit players
            target.AddBuff(BuffID.OnFire3, 15 * 60);

			//Explode if it has not yet
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//Explode if it has not yet
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3;
			}

			return false;
		}

		public override void AI()
		{
			//Point Forward
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

			//Light
			Lighting.AddLight(Projectile.Center, 0.8f, 0.32f, 0.08f);

			//Explodes in water
			if (Projectile.wet && Projectile.timeLeft > 3)
            {
				Projectile.timeLeft = 3;
			}

			//Produces fire dust in flight
			if (Projectile.timeLeft > 3 && Projectile.timeLeft <= 597)
            {
				for (int i = 0; i < 2; i++)
				{
					int FireDust = Dust.NewDust(Projectile.position, Projectile.width - 3, Projectile.height - 3, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[FireDust].position += Projectile.velocity * Main.rand.NextFloat(0f, 1f);
                    Main.dust[FireDust].rotation += Main.rand.NextFloat(5f);
					Main.dust[FireDust].noGravity = true;
					Main.dust[FireDust].velocity.X *= 0.3f;
					Main.dust[FireDust].velocity.Y *= 0.3f;
				}
			}

			//Explode Effect
			if (Projectile.timeLeft <= 3)
            {
				Projectile.tileCollide = false;
				Projectile.hide = true;
				Projectile.Resize(150, 150);
				Projectile.velocity *= 0;
				Projectile.knockBack = 8f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			//Sound
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			//Adapted inferno fork explosion
			for (int i = 0; i < 150; i++)
			{
				float num375 = Main.rand.Next(-10, 11);
				float num376 = Main.rand.Next(-10, 11);
				float num377 = Main.rand.Next(3, 9);
				float num378 = (float)Math.Sqrt(num375 * num375 + num376 * num376);
				num378 = num377 / num378;
				num375 *= num378;
				num376 *= num378;
				int ExplosionDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				Main.dust[ExplosionDust].noGravity = true;
				Main.dust[ExplosionDust].position.X = Projectile.Center.X;
				Main.dust[ExplosionDust].position.Y = Projectile.Center.Y;
				Main.dust[ExplosionDust].position.X += Main.rand.Next(-10, 11);
				Main.dust[ExplosionDust].position.Y += Main.rand.Next(-10, 11);
				Main.dust[ExplosionDust].velocity.X = num375 * 1.25f;
				Main.dust[ExplosionDust].velocity.Y = num376 * 1.25f;
			}
		}
	}
}