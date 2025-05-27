using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class VenomDart : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Venom Dart");
			// Tooltip.SetDefault("Splits into twin helical darts");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 28;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 3.2f;
			Item.value = Item.sellPrice(0, 0, 0, 8);

			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.VenomDartProjectile>();
			Item.shootSpeed = 5f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.VialofVenom, 1)
				.Register();
		}
	}
}