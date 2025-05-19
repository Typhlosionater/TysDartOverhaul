using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;


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

            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 255;

			AIType = ProjectileID.PoisonDartBlowgun;

            //for sticking
            Projectile.penetrate = 2;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
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

        //javelin code
        float StickingTimer; //was Projectile.localAI[0]


        public bool IsStickingToTarget
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public float StickTimer
        {
            get => StickingTimer;
            set => StickingTimer = value;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
        }

        public override bool PreAI()
        {
            if (IsStickingToTarget)
            {
                StickyAI();
                return false;
            }
            else
            {
                return base.PreAI();
            }
        }

        private const int StickTime = 60 * 5; // 5 seconds
        private void StickyAI()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            StickTimer += 1f;

            // Every 30 ticks, the javelin will perform a hit effect
            bool hitEffect = StickTimer % 30f == 0f;
            int npcTarget = TargetWhoAmI;
            if (StickTimer >= StickTime || npcTarget < 0 || npcTarget >= 200)
            { // If the index is past its limits, kill it
                Projectile.Kill();
            }
            else if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
            {
                // If the target is active and can take damage
                // Set the projectile's position relative to the target's center
                Projectile.Center = Main.npc[npcTarget].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                if (hitEffect)
                {
                    // Perform a hit effect here, causing the npc to react as if hit.
                    // Note that this does NOT damage the NPC, the damage is done through the debuff.
                    Main.npc[npcTarget].HitEffect(0, 1.0);
                }
            }
            else
            { // Otherwise, kill the projectile
                Projectile.Kill();
            }
        }

        private const int MaxStickingDarts = 10; // This is the max amount of javelins able to be attached to a single NPC
        private readonly Point[] stickingDarts = new Point[MaxStickingDarts]; // The point array holding for sticking javelins

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            IsStickingToTarget = true; // we are sticking to a target
            TargetWhoAmI = target.whoAmI; // Set the target whoAmI
            Projectile.velocity = (target.Center - Projectile.Center) *
                0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            Projectile.netUpdate = true; // netUpdate this javelin
            Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

            // ExampleJavelinBuff handles the damage over time (DoT)
            target.AddBuff(ModContent.BuffType<Buffs.CarapaceDartDebuff>(), 60 * 5); //5 seconds
            Projectile.timeLeft = 60 * 5; //sets timeleft to sticking time

            // KillOldestJavelin will kill the oldest projectile stuck to the specified npc.
            // It only works if ai[0] is 1 when sticking and ai[1] is the target npc index, which is what IsStickingToTarget and TargetWhoAmI correspond to.
            KillOldestDart(Projectile.whoAmI, Type, target.whoAmI, stickingDarts);
        }

        private void KillOldestDart(int protectedProjectileIndex, int projectileType, int targetNPCIndex, Point[] bufferForScan)
        {
            int num = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (i != protectedProjectileIndex && Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projectileType && Main.projectile[i].ai[1] == 1f && Main.projectile[i].ai[2] == (float)targetNPCIndex)
                {
                    bufferForScan[num++] = new Point(i, Main.projectile[i].timeLeft);
                    if (num >= bufferForScan.Length)
                    {
                        break;
                    }
                }
            }
            if (num < bufferForScan.Length)
            {
                return;
            }
            int num2 = 0;
            for (int j = 1; j < bufferForScan.Length; j++)
            {
                if (bufferForScan[j].Y < bufferForScan[num2].Y)
                {
                    num2 = j;
                }
            }
            Main.projectile[bufferForScan[num2].X].Kill();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // By shrinking target hitboxes by a small amount, this projectile only hits if it more directly hits the target.
            // This helps the javelin stick in a visually appealing place within the target sprite.
            if (targetHitbox.Width > 10 && targetHitbox.Height > 10)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 10, -targetHitbox.Height / 10);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            // If attached to an NPC, draw behind tiles (and the npc) if that NPC is behind tiles, otherwise just behind the NPC.
            if (IsStickingToTarget)
            {
                int npcIndex = TargetWhoAmI;
                if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
                {
                    if (Main.npc[npcIndex].behindTiles)
                    {
                        behindNPCsAndTiles.Add(index);
                    }
                    else
                    {
                        behindNPCsAndTiles.Add(index);
                    }

                    return;
                }
            }
            // Since we aren't attached, add to this list
            behindNPCsAndTiles.Add(index);
        }


    }
}