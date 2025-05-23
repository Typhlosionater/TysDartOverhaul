using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Net.Mime.MediaTypeNames;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class ChlorophyteDartProjectile : ModProjectile
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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

        public override void AI()
        {
            //chlorophyte arrow dust
            if (Main.rand.Next(2) == 0)
            {
                int num68 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 40);
                Main.dust[num68].noGravity = true;
                Main.dust[num68].scale = 1.3f;
                Dust obj27 = Main.dust[num68];
                obj27.velocity *= 0.5f;
            }
        }

        public override void OnKill(int timeLeft)
		{
            //Spawns dust and plays sound
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            for (int i = 0; i < 6; i++)
			{
				int DeathDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 74);
                Main.dust[DeathDust].velocity *= 0.75f;
                Main.dust[DeathDust].scale *= 0.60f;
            }

			//spawn slash
			Vector2 SlashSpawnlocation = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + (SlashSpawnlocation * 75), -SlashSpawnlocation * 5, ModContent.ProjectileType<AmmoDartEffects.ChlorophyteDartSlashProjectile>(), Projectile.damage / 3, 0f, Projectile.owner, Projectile.Center.X, Projectile.Center.Y);
        }
	}
}