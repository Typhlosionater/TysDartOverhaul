using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles.AmmoDartEffects
{
	public class BoneDartShrapnelProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.timeLeft = 15;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
			Projectile.alpha = 0;

			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.scale = 1f;
		}

		public override void AI()
		{
			//Startup
			if (Projectile.timeLeft == 15)
			{
				Projectile.frame = Main.rand.Next(6);
				Projectile.rotation += Main.rand.NextFloat(0, 4);
				Projectile.scale = 1.1f;

				Projectile.ai[0] = (float)(Main.rand.NextFloat(-0.2f, 0.2f));
			}

			//Rotation
			Projectile.rotation += Projectile.ai[0];

			//Actual projectile mode
			if (Projectile.timeLeft <= 12)
			{
				//Active hitbox
				Projectile.friendly = true;

				//Slow Down + Shrink
				Projectile.velocity *= 0.95f;
				Projectile.scale *= 0.95f;
			}
		}

        public override bool? CanHitNPC(NPC target)
        {
			//Can't hit the enemy that caused the shrapnel until 10 frames later, like a piercing projectile.
			if (Projectile.ai[1] == target.whoAmI && Projectile.timeLeft > 5)
            {
				return false;
            }
            return base.CanHitNPC(target);
        }
    }
}