using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.GameContent;

namespace TysDartOverhaul.Items.Weapons
{
	public class ClockworkBallista : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 6;
			Item.useAnimation = 6;
			Item.knockBack = 2.5f;

			Item.width = 78;
			Item.height = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

			Item.value = Item.sellPrice(0, 40, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

        public static bool CBConsumeAmmoFromProjHack = false;

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            bool ConsumeAmmo = false;
            if (CBConsumeAmmoFromProjHack)
            {
                ConsumeAmmo = !(Main.rand.NextFloat() >= .33f);
            }
            return ConsumeAmmo;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ClockworkBallista_HeldProjectile>(), 0, 0, player.whoAmI);

            return false;
        }
    }

    public class ClockworkBallista_HeldProjectile : ModProjectile
    {
        private Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 36;
            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;

            Projectile.ignoreWater = true;
        }

        private int FirerateStage = 6;

        private int FirerateTimer = 0;

        private bool WhirrUpNoise = false;

        public override void AI()
        {
            //Held Projectile Stuff
            Vector2 ownerCenter = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Vector2 toMouse = ownerCenter.DirectionTo(Main.MouseWorld);

            Projectile.Center = ownerCenter + toMouse * 25f;
            Projectile.rotation = toMouse.ToRotation();
            Projectile.spriteDirection = Projectile.direction = (toMouse.X > 0f).ToDirectionInt();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(180);
            }

            Projectile.timeLeft = 2;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);
            Owner.itemRotation = MathHelper.WrapAngle(float.Atan2(toMouse.Y * Projectile.direction, toMouse.X * Projectile.direction));

            if (Main.myPlayer == Projectile.owner && !Main.mouseLeft)
            {
                Projectile.Kill();
                return;
            }

            //Noise Stuff
            if (WhirrUpNoise == false)
            {
                SoundEngine.PlaySound(SoundID.Item149, Projectile.position);
                WhirrUpNoise = true;
            }

            //Firerate stuff
            int Firedelay = (int)(Owner.HeldItem.useTime * Owner.GetWeaponAttackSpeed(Owner.HeldItem)) * FirerateStage;

            if (FirerateTimer >= Firedelay)
            {
                SoundEngine.PlaySound(SoundID.Item102 with { Volume = 0.6f }, Projectile.position);

                ClockworkBallista.CBConsumeAmmoFromProjHack = true;
                Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemID);
                ClockworkBallista.CBConsumeAmmoFromProjHack = false;

                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 velocity = Owner.Center.DirectionTo(Main.MouseWorld) * speed;
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(9 - FirerateStage));
                    velocity *= Main.rand.NextFloat(0.9f, 1f);
                    Projectile.NewProjectile(Owner.GetSource_ItemUse_WithPotentialAmmo(Owner.HeldItem, usedAmmoItemID), Owner.Center, velocity, projToShoot, damage, knockback, Projectile.owner);
                }

                FirerateTimer = 0;
                if (FirerateStage > 1)
                {
                    FirerateStage--;
                }
            }
            else
            {
                FirerateTimer++;
            }
        }
    }
}