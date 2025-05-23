using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;
using Terraria.Localization;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Linq;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace TysDartOverhaul.Projectiles
{
	public class TysDartOverhaulGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		//Vanilla Dart Changes
        public override void SetDefaults(Projectile projectile)
        {
			if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
			{
                //Poison darts
                if (projectile.type == ProjectileID.PoisonDartBlowgun)
                {
                    projectile.penetrate = -1;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                }

                //Crystal darts use local immunity and grant 30 frames
                if (projectile.type == ProjectileID.CrystalDart)
				{
					projectile.usesLocalNPCImmunity = true;
					projectile.localNPCHitCooldown = 30;
                    projectile.penetrate = 5;
				}

				//Ichor darts prior to splitting use local immunity
				if (projectile.type == ProjectileID.IchorDart)
				{
					projectile.usesLocalNPCImmunity = true;
					projectile.localNPCHitCooldown = 10;
				}

				//Cursed dart flames no longer pierce
				if (projectile.type == ProjectileID.CursedDartFlame)
				{
                    projectile.penetrate = 1;
					projectile.usesLocalNPCImmunity = true;
					projectile.idStaticNPCHitCooldown = 10;
				}
			}
		}

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
			{
                //Poison darts explode if it hasn't yet
                if (projectile.type == ProjectileID.PoisonDartBlowgun && projectile.timeLeft > 3)
                {
                    projectile.timeLeft = 3;
                }

                //crystal darts reduce in damage when hitting enemies
                if (projectile.type == ProjectileID.CrystalDart)
				{
                    projectile.damage = (int)(projectile.damage * 0.9f);
                    projectile.netUpdate = true;
                }
			}
		}

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
            {
                //Poison darts explode if it hasn't yet
                if (projectile.type == ProjectileID.PoisonDartBlowgun && projectile.timeLeft > 3)
                {
                    projectile.timeLeft = 3;
                }

                //crystal darts reduce in damage when hitting players
                if (projectile.type == ProjectileID.CrystalDart)
                {
                    projectile.damage = (int)(projectile.damage * 0.9f);
                    projectile.netUpdate = true;
                }
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
            {
                //Poison darts explode if it hasn't yet
                if (projectile.type == ProjectileID.PoisonDartBlowgun && projectile.timeLeft > 3)
                {
                    projectile.timeLeft = 3;
                    projectile.tileCollide = false;
                    return false;
                }

                //crystal darts reduce in damage when bouncing, play a sound and make dust
                if (projectile.type == ProjectileID.CrystalDart && projectile.penetrate > 1)
                {
                    SoundEngine.PlaySound(SoundID.Item56 with { Volume = 0.3f }, projectile.position);
                    projectile.damage = (int)(projectile.damage * 0.9f);
                    projectile.netUpdate = true;

                    //make dust
                    for (int i = 0; i < 3; i++)
                    {
                        int num215 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 70);
                        Main.dust[num215].noGravity = true;
                        Dust dust57 = Main.dust[num215];
                        Dust dust3 = dust57;
                        dust57 = Main.dust[num215];
                        dust3 = dust57;
                        dust3.scale *= 0.9f;
                    }
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }

        public override void AI(Projectile projectile)
        {
            if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
            {
                //Poison darts explosion modify
                if (projectile.type == ProjectileID.PoisonDartBlowgun && projectile.timeLeft <= 3)
                {
                    projectile.tileCollide = false;
                    projectile.hide = true;
                    projectile.Resize(50, 50);
                    projectile.velocity *= 0;
                }
            }
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
            {
                //Poison darts explode effect
                if (projectile.type == ProjectileID.PoisonDartBlowgun)
                {
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.5f }, projectile.position);
                    for (int i = 0; i < 8; i++)
                    {
                        Dust dust1 = Dust.NewDustDirect(projectile.Center, 0, 0, 46);
                        dust1.noGravity = true;
                        dust1.fadeIn = 1.9f;
                        dust1.alpha = 120;
                        dust1.velocity *= 2f;

                    }
                    for (int i = 0; i < 8; i++)
                    {
                        int num328 = Dust.NewDust(projectile.Center, 0, 0, 256);
                        Main.dust[num328].noGravity = true;
                        Main.dust[num328].position = (Main.dust[num328].position + projectile.position) / 2f;
                        Main.dust[num328].velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                        Main.dust[num328].velocity.Normalize();
                        Main.dust[num328].velocity *= 2f;
                        Dust dust135 = Main.dust[num328];
                        Dust dust3 = dust135;
                        dust3.velocity *= (float)Main.rand.Next(1, 30) * 0.1f;
                        Main.dust[num328].alpha = 60;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        int num215 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31);
                    }

                    return false;
                }

                //Cursed darts no longer produce a flame on death and instead produce a little fire dust
                if (projectile.type == ProjectileID.CursedDart)
                {
                    //Spawns dust and plays sound
                    SoundEngine.PlaySound(SoundID.NPCHit3 with { Volume = 0.8f }, projectile.position);
                    for (int i = 0; i < 8; i++)
                    {
                        int num306 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, 0f, 0f, 100);
                        Main.dust[num306].rotation += Main.rand.NextFloat();
                        Dust dust63 = Main.dust[num306];
                        Dust dust189 = dust63;
                        dust189.scale += Main.rand.Next(50) * 0.01f;
                    }

                    return false;
                }
            }
            return base.PreKill(projectile, timeLeft);
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (ModContent.GetInstance<TysDartOverhaulConfig>().VanillaDartChanges)
            {
                //crystal darts produce dust on death
                if (projectile.type == ProjectileID.CrystalDart)
                {
                    //play sound and make dust
                    SoundEngine.PlaySound(SoundID.Dig, projectile.position);
                    for (int i = 0; i < 5; i++)
                    {
                        int num215 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 70);
                        Main.dust[num215].noGravity = true;
                        Dust dust57 = Main.dust[num215];
                        Dust dust3 = dust57;
                        dust57 = Main.dust[num215];
                        dust3 = dust57;
                        dust3.scale *= 0.9f;
                    }
                }
            }
        }
    }
}