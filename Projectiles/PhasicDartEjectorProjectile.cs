using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace TysDartOverhaul.Projectiles
{
	public class PhasicDartEjectorProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phasic Dart Ejector");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

            Projectile.hide = true;
		}

		public int MinimumDarts = 3;
		public int MaximumDarts = 9;
        public int TimePerDart = 0;

        public int ChargeCounter = 0;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            Projectile.Center = owner.Center;

            TimePerDart = owner.HeldItem.useTime;

            //Fires when the player is nolonger channeling
            if (!owner.ItemAnimationActive)
            {
                if (ChargeCounter < (TimePerDart * MinimumDarts))
                {
                    //Fizzle Sound
                    SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                }
                else
                {
                    //Get Projectile Numbers
                    owner.PickAmmo(owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemID);

                    //Projectile Number
                    int ProjectileNumber = (ChargeCounter / TimePerDart) > MaximumDarts ? MaximumDarts : ChargeCounter / TimePerDart;

                    // Shoot projectiles
                    for (int i = 0; i < ProjectileNumber; i++)
                    {
                        Vector2 velocity = Main.MouseWorld - owner.Center;
                        velocity.Normalize();
                        velocity *= speed;
                        velocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));
                        velocity *= Main.rand.NextFloat(0.8f, 1f);
                        Projectile.NewProjectile(Projectile.GetSource_ItemUse_WithPotentialAmmo(owner.HeldItem, usedAmmoItemID), owner.Center, velocity, projToShoot, damage, knockback, Projectile.owner);
                    }

                    //Fire Sound
                    SoundEngine.PlaySound(SoundID.Item92, Projectile.position);

                    //Kill projectile so this code isnt called again
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.timeLeft = 2;
            }

            ChargeCounter++;
        }
    }
}