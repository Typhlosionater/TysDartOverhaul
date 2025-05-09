using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class CarapaceDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 
            Projectile.hostile = false;

            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
			Projectile.alpha = 255;

			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			AIType = ProjectileID.PoisonDartBlowgun;

			Projectile.ArmorPenetration = 50;
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Spawns dust and plays sound
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ebonwood, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}
	}
}