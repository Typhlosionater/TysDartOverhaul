using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class VenomDartProjectile : ModProjectile
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

			AIType = ProjectileID.Bullet;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Inflicts 10 seconds on venom on hit enemies
			target.AddBuff(BuffID.Venom, 10 * 60);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //Inflicts 10 seconds on venom on hit players
            target.AddBuff(BuffID.Venom, 10 * 60);
		}

		public override void AI()
		{
			//Spawns secondary dart
			if (Projectile.ai[0] == 0 && Projectile.owner == Main.myPlayer)
            {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<VenomDartProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<VenomDartProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1);

				Projectile.Kill();
			}

			//Helix Velocity Movement
			float InitialVelocityMagnitude;
			float InitialVelocityRotation;
			float WaveAmplitude = 40;
			float WaveFrequency = 0.15f;

			WaveAmplitude *= Projectile.ai[0];
			float SineWaveX = Projectile.ai[1] * WaveFrequency;
			float SineWaveY = (float)Math.Sin(SineWaveX) * WaveAmplitude;

			if (Projectile.ai[1] == 0)
			{
				InitialVelocityMagnitude = Projectile.velocity.Length();
				InitialVelocityRotation = Utils.ToRotation(Projectile.velocity);
			}
            else
            {
				float VelocityModifier = SineWaveY - (float)Math.Sin(SineWaveX - WaveFrequency) * WaveAmplitude;
				InitialVelocityMagnitude = (float)Math.Sqrt(Projectile.velocity.LengthSquared() - VelocityModifier * VelocityModifier);
				InitialVelocityRotation = Utils.ToRotation(Utils.RotatedBy(Projectile.velocity, -Utils.ToRotation(new Vector2(InitialVelocityMagnitude, VelocityModifier)), default(Vector2)));
			}

			Projectile.velocity = Utils.RotatedBy(new Vector2(InitialVelocityMagnitude, (float)Math.Sin(SineWaveX + WaveFrequency) * WaveAmplitude - SineWaveY), InitialVelocityRotation, default(Vector2));
			Projectile.ai[1]++;
		}

		public override void OnKill(int timeLeft)
		{
			//Initial dart doesnt have death anim
			if (Projectile.ai[0] != 0)
			{
				//Play sound
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

				//Venom Arrow Impact
				for (int num639 = 0; num639 < 6; num639++)
				{
					int num640 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 171, 0f, 0f, 100);
					Main.dust[num640].scale = (float)Main.rand.Next(1, 10) * 0.1f;
					Main.dust[num640].noGravity = true;
					Main.dust[num640].fadeIn = 1.5f;
					Dust dust = Main.dust[num640];
					dust.velocity *= 0.75f;
				}
			}
		}
	}
}