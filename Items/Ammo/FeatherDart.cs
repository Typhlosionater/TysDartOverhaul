using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class FeatherDart : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feather Dart");
			Tooltip.SetDefault("Unaffected by gravity");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;

			Item.maxStack = 999;
			Item.consumable = true;             
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(0, 0, 0, 1);

			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.FeatherDartProjectile>();
			Item.shootSpeed = 2f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.Feather, 1)
				.Register();
		}
	}
}