using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;


namespace TysDartOverhaul.Projectiles.ConvertedDartProjectiles
{
	public class StingerDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true; 

            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 255;

			Projectile.scale = 0.9f;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Inflicts 10 seconds on poisoned on hit enemies
			target.AddBuff(BuffID.Poisoned, 10 * 60);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //Inflicts 10 seconds on poisoned on hit players
            target.AddBuff(BuffID.Poisoned, 10 * 60);
		}

        public override void AI()
        {
			//Point Forward
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

			//Fade in
			if (Projectile.alpha > 0)
            {
				Projectile.alpha -= 25;
            }

            if (Main.rand.Next(2) == 0)
            {
                int num103 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 18, 0f, 0f, 0, default(Color), 0.9f);
                Main.dust[num103].noGravity = true;
                Dust obj45 = Main.dust[num103];
                obj45.velocity *= 0.5f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 6;
			height = 6;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Spawns dust and plays sound
			for (int i = 0; i < 3; i++)
			{
				int StingerDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Pumpkin);
				Main.dust[StingerDust].scale = 0.9f;
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}
	}
}