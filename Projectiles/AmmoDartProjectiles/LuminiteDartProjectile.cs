using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class LuminiteDartProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 

            Projectile.timeLeft = 1800;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 255;

			AIType = ProjectileID.PoisonDartBlowgun;

			Projectile.extraUpdates = 2;
		}

        private static VertexStrip vertexStrip = new();

        public override bool PreDraw(ref Color lightColor)
        {
            Color StripColors(float progressOnStrip)
            {
                float num = 1f - progressOnStrip;
                Color result = new Color(33, 160, 141) * (num * num * num * num) * 0.5f;
                result.A = 0;
                return result;
            }

            float StripWidth(float progressOnStrip) => 6f * (1 - progressOnStrip);

            MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(2f);
            miscShaderData.Apply();
            vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos.Skip(1).ToArray(), Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2f);
            vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        public override Color? GetAlpha(Color lightColor)
		{
			//Uneffected by lighting
			return Color.White;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Lunar Explosion effect
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			int LunarExplosionProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.LunarFlare, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, -1f);
			Main.projectile[LunarExplosionProj].DamageType = DamageClass.Ranged;
			Main.projectile[LunarExplosionProj].rotation = Main.rand.NextFloat(0f, 10f);
		}
	}
}