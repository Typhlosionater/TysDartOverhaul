using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TysDartOverhaul.Items.Weapons
{
	public class StingOperation : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 54;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 1.5f;

			Item.width = 58;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item17;

			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 15f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<TheStinger>(), 1)
				.AddIngredient(ItemID.Hive, 30)
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= .33f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			//Changes to queen's stingers
			type = ModContent.ProjectileType<Projectiles.ConvertedDartProjectiles.QueensStingerDartProjectile>();

			//inaccuracy
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f));
			velocity = velocity * Main.rand.NextFloat(0.8f, 1f);
		}
	}
}