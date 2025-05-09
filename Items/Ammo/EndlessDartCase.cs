using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class EndlessDartCase : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 36;
			Item.height = 32;

			Item.consumable = false;             
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(0, 1, 0, 0);

			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>();
			Item.shootSpeed = 1f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Dart>(), 3996)
				.AddTile(TileID.CrystalBall)
				.Register();
		}
	}
}