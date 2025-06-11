using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TysDartOverhaul.Items.Weapons
{
	public class Decapitator : ModItem
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
			Item.damage = 70;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.knockBack = 6f;
			Item.crit += 6;

			Item.width = 64;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item102;

			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 0);
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //Changes standard darts into necro darts
            if (type == ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>())
            {
                type = ModContent.ProjectileType<Projectiles.ConvertedDartProjectiles.NecroDartProjectile>();
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            //Find vector
            Vector2 BaseAngle = velocity;
			BaseAngle.Normalize();

			//Perpendicular Vector
			Vector2 PerpendicularOffset = new Vector2(BaseAngle.Y, -BaseAngle.X);
			if (PerpendicularOffset.Y < 0)
			{
				PerpendicularOffset = new Vector2(-BaseAngle.Y, BaseAngle.X);
			}
			PerpendicularOffset *= 8;

			//Dart 1
			Vector2 PositionOne = position;
			PositionOne.X += PerpendicularOffset.X;
			PositionOne.Y += PerpendicularOffset.Y;
			Projectile.NewProjectile(source, PositionOne, velocity, type, damage, knockback, player.whoAmI);

			//Dart 2
			Vector2 PositionTwo = position;
			PositionTwo.X -= PerpendicularOffset.X;
			PositionTwo.Y -= PerpendicularOffset.Y;
			Projectile.NewProjectile(source, PositionTwo, velocity, type, damage, knockback, player.whoAmI);

			return false;
		}
	}
}