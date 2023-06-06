using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles
{
	public class ClockworkBallistaProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clockwork Ballista");
		}

		public override void SetDefaults()
		{
			Projectile.width = 78;
			Projectile.height = 36;

			Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			Projectile.hide = true;
		}

		public override bool? CanDamage()
		{
			return new bool?(false);
		}

		private float holdOutOffset = 26f; // Change this to fit your projectile

		public int MinimumDelay = 6;
		public int MaximumDelayMultiplier = 4;
		public int DelayStep = 1;

		public int CurrentDelay = 30;
		public int DelayCounter = 0;

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			Vector2 toMouse = Main.MouseWorld - owner.MountedCenter;
			toMouse.Normalize();

			//Setup
			if (Projectile.ai[1] == 0)
			{
				MinimumDelay = owner.HeldItem.useTime;

				CurrentDelay = MinimumDelay * MaximumDelayMultiplier;

				Projectile.ai[1] = 1;
			}

			if (owner.PickAmmo(owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId) && owner.channel && !owner.noItems && !owner.CCed)
			{
				//If the delay timer is more than the current delay
				if (DelayCounter >= CurrentDelay)
				{
					//Calculate projectile velocity
					Vector2 velocity = Main.MouseWorld - owner.Center;
					velocity.Normalize();
					velocity *= speed;
					velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
					velocity *= Main.rand.NextFloat(0.8f, 1f);

					//Fire Projectile
					Projectile.NewProjectile(Projectile.GetSource_ItemUse_WithPotentialAmmo(owner.HeldItem, usedAmmoItemId), owner.Center, velocity, projToShoot, damage, knockBack, Projectile.owner);
					SoundEngine.PlaySound(SoundID.Item102, Projectile.position);

					//Reset counter and reduce delay
					DelayCounter = 0;
					if (CurrentDelay > MinimumDelay)
					{
						CurrentDelay = CurrentDelay - DelayStep;
					}

					if (MinimumDelay > CurrentDelay)
					{
						CurrentDelay = MinimumDelay;
					}

				}
				else
				{
					//Counts between shots
					DelayCounter++;
				}
			}
			else
			{
				Projectile.Kill();
			}

			// Set our direction and rotation
			Projectile.direction = 1;
			if (Math.Sign(Main.MouseWorld.X - owner.Center.X) == -1)
				Projectile.direction = -1;
			float rotationOffset = Projectile.direction == -1 ? MathHelper.Pi : 0f;
			Projectile.rotation = toMouse.ToRotation() + rotationOffset;
			Projectile.spriteDirection = Projectile.direction;


			// Set our position and velocity
			Projectile.Center = owner.RotatedRelativePoint(owner.MountedCenter, false, false) + toMouse * holdOutOffset;
			Projectile.velocity = Vector2.Zero;

			// Set timeLeft so our projectile doesnt die unexpectedly 
			Projectile.timeLeft = 2;

			// Set some player fields in our owner
			owner.ChangeDir(Projectile.direction);
			owner.heldProj = Projectile.whoAmI;
			owner.SetDummyItemTime(2);
			owner.itemRotation = Projectile.DirectionFrom(owner.MountedCenter).ToRotation();
			if (Projectile.Center.X < owner.MountedCenter.X)
			{
				owner.itemRotation += (float)Math.PI;
			}
			owner.itemRotation = MathHelper.WrapAngle(owner.itemRotation);
		}
	}
}