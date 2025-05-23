using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;


namespace TysDartOverhaul.Projectiles.AmmoDartEffects
{
	public class EctoplasmDartEctoboltProjectile : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;

            Projectile.tileCollide = false;

            Projectile.timeLeft = 90;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Projectile.localAI[0]++;

            if (Projectile.localAI[0] >= 2)
            {
                //Produces Dust in Flight
                for (int DustLoop = 0; DustLoop < 5; DustLoop++)
                {
                    Vector2 DustPosition = Projectile.Center - (Projectile.velocity * (0.2f * DustLoop));
                    int TrailDust = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit);
                    Main.dust[TrailDust].position = DustPosition;
                    Main.dust[TrailDust].alpha = Projectile.alpha;
                    Main.dust[TrailDust].velocity *= 0f;
                    Main.dust[TrailDust].scale *= 0.75f;
                    Main.dust[TrailDust].rotation = Main.rand.NextFloat(0, 4);
                    Main.dust[TrailDust].noGravity = true;
                }
            }

            if (Projectile.localAI[0] >= 10)
            {
                Projectile.friendly = true;
            }

            //eater homing
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 1.57f;
            float num280 = Projectile.position.X;
            float num281 = Projectile.position.Y;
            float num282 = 100000f;
            bool flag9 = false;
            if (Projectile.friendly)
            {
                for (int num283 = 0; num283 < 200; num283++)
                {
                    if (Main.npc[num283].CanBeChasedBy(this))
                    {
                        float num284 = Main.npc[num283].position.X + (float)(Main.npc[num283].width / 2);
                        float num285 = Main.npc[num283].position.Y + (float)(Main.npc[num283].height / 2);
                        float num286 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num284) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num285);
                        if (num286 < 800f && num286 < num282)
                        {
                            num282 = num286;
                            num280 = num284;
                            num281 = num285;
                            flag9 = true;
                        }
                    }
                }
            }
            if (!flag9)
            {
                num280 = Projectile.position.X + (float)(Projectile.width / 2) + Projectile.velocity.X * 100f;
                num281 = Projectile.position.Y + (float)(Projectile.height / 2) + Projectile.velocity.Y * 100f;
            }
            float num287 = 6f;
            float num288 = 0.1f;
            num287 = 9f;
            num288 = 0.2f;
            Vector2 vector25 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float num289 = num280 - vector25.X;
            float num290 = num281 - vector25.Y;
            float num291 = (float)Math.Sqrt(num289 * num289 + num290 * num290);
            float num292 = num291;
            num291 = num287 / num291;
            num289 *= num291;
            num290 *= num291;
            if (Projectile.velocity.X < num289)
            {
                Projectile.velocity.X += num288;
                if (Projectile.velocity.X < 0f && num289 > 0f)
                {
                    Projectile.velocity.X += num288 * 2f;
                }
            }
            else if (Projectile.velocity.X > num289)
            {
                Projectile.velocity.X -= num288;
                if (Projectile.velocity.X > 0f && num289 < 0f)
                {
                    Projectile.velocity.X -= num288 * 2f;
                }
            }
            if (Projectile.velocity.Y < num290)
            {
                Projectile.velocity.Y += num288;
                if (Projectile.velocity.Y < 0f && num290 > 0f)
                {
                    Projectile.velocity.Y += num288 * 2f;
                }
            }
            else if (Projectile.velocity.Y > num290)
            {
                Projectile.velocity.Y -= num288;
                if (Projectile.velocity.Y > 0f && num290 < 0f)
                {
                    Projectile.velocity.Y -= num288 * 2f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
        }
    }
}