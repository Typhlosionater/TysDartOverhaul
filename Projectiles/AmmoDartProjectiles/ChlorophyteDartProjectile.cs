using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


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

		public override void AI()
		{
			//startup then countup
			if (Projectile.timeLeft == 600)
			{
				Projectile.frameCounter = Main.rand.Next(10);
			}
            else
            {
				Projectile.frameCounter++;
			}

			//Regularly produce spores
			if (Projectile.frameCounter >= 10)
			{
				Projectile.frameCounter = 0;
				Projectile.netUpdate = true;

				int SporeProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.SporeCloud, Projectile.damage / 2, Projectile.knockBack / 4, Projectile.owner);
				Main.projectile[SporeProj].DamageType = DamageClass.Ranged;
				Main.projectile[SporeProj].usesIDStaticNPCImmunity = true;
				Main.projectile[SporeProj].idStaticNPCHitCooldown = 10;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Spawns dust and plays sound
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Cactus);
			}
			SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
		}
	}
}