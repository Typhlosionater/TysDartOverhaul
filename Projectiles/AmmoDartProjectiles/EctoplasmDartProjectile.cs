using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class EctoplasmDartProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true; 

            Projectile.timeLeft = 600 * 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 6;
			Projectile.alpha = 80;

			AIType = ProjectileID.PoisonDartBlowgun;

			Projectile.extraUpdates = 1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Damage gets increased after each hit
			Projectile.damage = (int)(Projectile.damage * 1.2f);
            Projectile.velocity *= 1.1f;
			Projectile.netUpdate = true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			//Damage gets increased after each hit
			Projectile.damage = (int)(Projectile.damage * 1.2f);
			Projectile.velocity *= 1.1f;
            Projectile.netUpdate = true;
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnKill(int timeLeft)
		{
			//Spawns dust on death
			for (int i = 0; i < 4; i++)
			{
				int ImpactDust = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit);
				Main.dust[ImpactDust].velocity *= 1.5f;
				Main.dust[ImpactDust].scale *= 1.5f;
				Main.dust[ImpactDust].rotation = Main.rand.NextFloat(0, 4);
				Main.dust[ImpactDust].noGravity = true;
			}
		}
	}
}