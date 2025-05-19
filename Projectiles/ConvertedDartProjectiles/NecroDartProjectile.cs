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
	public class NecroDartProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
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
			Projectile.extraUpdates = 1;
			Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                //Color color = new Color(255, 255, 255, 155) * (((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.8f);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return base.PreDraw(ref lightColor);
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
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int num636 = 0; num636 < 5; num636++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 26, 0f, 0f, 0, default(Color), 0.9f);
            }
        }
	}
}