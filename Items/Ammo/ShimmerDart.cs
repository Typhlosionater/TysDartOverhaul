using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Ammo
{
	public class ShimmerDart : ModItem
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
			Item.width = 18;
			Item.height = 26;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 3f;
			Item.value = Item.buyPrice(0, 0, 0, 10);

			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.ShimmerDartProjectile>();
			Item.shootSpeed = 1f;
			Item.ammo = AmmoID.Dart;
		}
	}
}