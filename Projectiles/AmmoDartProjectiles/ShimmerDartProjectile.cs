using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Drawing;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class ShimmerDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 

            Projectile.timeLeft = 600 * 2;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 255;

			AIType = ProjectileID.PoisonDartBlowgun;
        }

        int BouncesRemaining = 8;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 6;
			height = 6;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If collide with tile, reduce the penetrate.
            // So the projectile can reflect at most 5 times
            BouncesRemaining--;
            if (BouncesRemaining <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
        }

        public override void AI()
        {
            //shimmer arrow dust
            if (Main.rand.Next(8) == 0)
            {
                Dust dust11 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(4f, 4f), 306, Projectile.velocity * 0.5f, 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), 1f + Main.rand.NextFloat() * 0.4f);
                dust11.noGravity = true;
                dust11.fadeIn = dust11.scale + 0.05f;
                Dust dust12 = Dust.CloneDust(dust11);
                dust12.color = Color.White;
                dust12.scale -= 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
		{
            //Shimmer arrow death
            Vector2 center;
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            float num686 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
            for (float num687 = 0f; num687 < 1f; num687 += 1f)
            {
                float num688 = num686 + (float)Math.PI * 2f * num687;
                Vector2 unitX2 = Vector2.UnitX;
                double radians43 = num688;
                center = default(Vector2);
                Vector2 val108 = unitX2.RotatedBy(radians43, center);
                Vector2 center26 = Projectile.Center;
                float num689 = 0.4f;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
                {
                    PositionInWorld = center26,
                    MovementVector = val108 * num689
                }, Projectile.owner);
            }
        }
	}
}