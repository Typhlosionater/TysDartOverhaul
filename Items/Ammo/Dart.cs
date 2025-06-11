using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TysDartOverhaul.Items.Weapons;

namespace TysDartOverhaul.Items.Ammo
{
	public class Dart : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShimmerDart>();

        }

		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 24;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 1f;
			Item.value = Item.buyPrice(0, 0, 0, 6);

			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>();
			Item.shootSpeed = 1f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe(150)
				.AddIngredient(ItemID.SilverBar, 1)
				.AddTile(TileID.Anvils)
                .Register();

			CreateRecipe(150)
				.AddIngredient(ItemID.TungstenBar, 1)
                .AddTile(TileID.Anvils)
                .Register();
		}
	}
}