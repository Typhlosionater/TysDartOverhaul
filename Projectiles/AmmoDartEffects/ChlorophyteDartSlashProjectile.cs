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
	public class ChlorophyteDartSlashProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.scale = 1f + (float)Main.rand.Next(30) * 0.01f;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 10 * 3;
            Projectile.alpha = 255;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //centre texture
            var texture = TextureAssets.Projectile[Type].Value;
            var position = (Projectile.Center - Main.screenPosition).Floor();
            var origin = texture.Size() / 2f;
            Main.EntitySpriteDraw(texture, position, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            //Afterimage
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + (Projectile.Size / 2);
                Color color = Projectile.GetAlpha(lightColor) * (((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.6f);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void AI()
        {
            //Point forward
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.alpha = 145;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            //If this was not a second slash: sawns second slash and plays sound
            if (Projectile.ai[0] != -1 && Projectile.ai[1] != -1)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                Vector2 TargetCentre = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Vector2 SlashSpawnlocation = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), TargetCentre + (SlashSpawnlocation * 75), -SlashSpawnlocation * 5, ModContent.ProjectileType<AmmoDartEffects.ChlorophyteDartSlashProjectile>(), Projectile.damage, 0f, Projectile.owner, -1, -1);
            }
        }
    }
}