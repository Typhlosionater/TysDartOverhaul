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
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 0, 0, 1);

			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.FeatherDartProjectile>();
			Item.shootSpeed = 4f;
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