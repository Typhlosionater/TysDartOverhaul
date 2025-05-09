using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class BoneDart : ModItem
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
			Item.damage = 9;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 26;

			Item.maxStack = 999;
			Item.consumable = true;             
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 0, 0, 3);

			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.BoneDartProjectile>();
			Item.shootSpeed = 1.5f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.FossilOre, 1)
				.Register();
		}
	}
}