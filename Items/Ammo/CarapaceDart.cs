using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class CarapaceDart : ModItem
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
			Item.damage = 17;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 0, 0, 30);

			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.CarapaceDartProjectile>();
			Item.shootSpeed = 6f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.BeetleHusk, 1)
				.Register();
		}
	}
}