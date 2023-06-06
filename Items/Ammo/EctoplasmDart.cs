using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;

namespace TysDartOverhaul.Items.Ammo
{
	public class EctoplasmDart : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ectoplasm Dart");
			Tooltip.SetDefault("Increases in damage as it pierces");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 26;

			Item.maxStack = 999;
			Item.consumable = true;             
			Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(0, 0, 0, 20);

			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.EctoplasmDartProjectile>();
			Item.shootSpeed = 3f;
			Item.ammo = AmmoID.Dart;

			Item.alpha = 80;
			Item.color = new Color(255, 255, 255, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.Ectoplasm, 1)
				.Register();
		}
	}
}