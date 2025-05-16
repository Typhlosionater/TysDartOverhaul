using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class LuminiteDart : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 28;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 0, 0, 2);

			Item.rare = ItemRarityID.Cyan;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.LuminiteDartProjectile>();
			Item.shootSpeed = 4f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe(333)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}