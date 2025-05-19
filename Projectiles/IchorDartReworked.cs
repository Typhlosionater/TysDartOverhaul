using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class IchorDartReworked : ModProjectile
	{
        public override string Texture
        {
            get => $"Terraria/Images/Projectile_{ProjectileID.IchorDart}";
        }

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

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Inflicts 7-15 seconds of ichor on hit enemies
			target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(7, 15));
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //Inflicts 7-15 seconds of ichor on hit players
            target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(7, 15));
        }

		public override void AI()
		{
			
		}

		public override void OnKill(int timeLeft)
		{
			//Initial dart doesnt have death anim
			if (Projectile.ai[0] != 0)
			{
				//Play sound
				SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);

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