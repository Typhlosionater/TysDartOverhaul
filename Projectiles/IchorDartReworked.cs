using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Mono.Cecil;
using static System.Net.Mime.MediaTypeNames;


namespace TysDartOverhaul.Projectiles.AmmoDartProjectiles
{
	public class IchorDartReworked : ModProjectile
	{
        public override string Texture
        {
            get => $"Terraria/Images/Projectile_{ProjectileID.IchorDart}";
        }

        public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;

            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;

            AIType = ProjectileID.PoisonDartBlowgun;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        int HitEnemy = -1;

        int ProjTimer = 0;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Inflicts 7-15 seconds of ichor on hit enemies
			target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(7, 15));

            //Feeds enemy hit into split darts
            HitEnemy = target.whoAmI;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //Inflicts 7-15 seconds of ichor on hit players
            target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(7, 15));
        }

        public override void AI()
        {
            //Point forward
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (ProjTimer == 0)
            {
                if (Projectile.ai[2] != -1)
                {
                    Projectile.timeLeft = 12 + Main.rand.Next(6);
                }
            }
            ProjTimer++;
            Projectile.netUpdate = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            //Can't hit the enemy that caused the dart to break until 10 frames later, like a piercing projectile.
            if (Projectile.ai[1] == target.whoAmI && Projectile.ai[2] == -1 && ProjTimer < 10)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void OnKill(int timeLeft)
		{
			//Initial splits
			if (Projectile.ai[2] != -1)
			{
                //Sound
                SoundEngine.PlaySound(SoundID.Item17 with { Volume = 0.8f, Pitch = -0.8f}, Projectile.Center);

                //Dust Cone
                for (int dustamount = 0; dustamount < 10; dustamount++)
                {
                    Vector2 dustspeed = Projectile.velocity;
                    dustspeed.Normalize();
                    dustspeed *= 10 * Main.rand.NextFloat(0.7f, 1.3f);
                    dustspeed = dustspeed.RotatedByRandom(MathHelper.ToRadians(30));
                    int Ichordustcone = Dust.NewDust(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, 170, dustspeed.X, dustspeed.Y, 100);
                    Main.dust[Ichordustcone].noGravity = true;
                }

                //Fires a random spread of 3 to 5 darts in 20 degrees
                if (Projectile.owner == Main.myPlayer)
                {
                    float numberProjectiles = 3 + Main.rand.Next(2);
                    numberProjectiles += Main.rand.Next(2);
                    float SpreadAngle = MathHelper.ToRadians(30);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 PerturbedSpeed = Projectile.velocity.RotatedByRandom(SpreadAngle);
                        PerturbedSpeed = PerturbedSpeed * Main.rand.NextFloat(0.85f, 1.15f);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Projectile.velocity, PerturbedSpeed, ModContent.ProjectileType<IchorDartReworked>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, 0, HitEnemy, -1);
                    }
                }
            }
            else
            {
                //sound 
                SoundEngine.PlaySound(SoundID.NPCHit3 with { Volume = 0.8f }, Projectile.position);

                //dust
                for (int num640 = 0; num640 < 5; num640++)
                {
                    int num641 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 169, 0f, 0f, 100);
                    if (Main.rand.Next(2) == 0)
                    {
                        Dust dust225 = Main.dust[num641];
                        Dust dust3 = dust225;
                        dust3.scale *= 2f;
                        Main.dust[num641].noGravity = true;
                        dust225 = Main.dust[num641];
                        dust3 = dust225;
                        dust3.velocity *= 5f;
                    }
                }
            }
		}
	}
}