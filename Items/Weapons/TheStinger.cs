using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TysDartOverhaul.Items.Weapons
{
	public class TheStinger : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Stinger");
			Tooltip.SetDefault("Rapidly fires poisonous stinger darts\n25% chance to not consume ammo");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 1f;

			Item.width = 44;
			Item.height = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item17;

			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 12.5f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 1);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.IllegalGunParts, 1)
				.AddIngredient(ItemID.BeeWax, 12)
				.AddIngredient(ItemID.Stinger, 8)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= .25f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			//Changes to stingers
			type = ModContent.ProjectileType<Projectiles.ConvertedDartProjectiles.StingerDartProjectile>();

			//inaccuracy
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f));
			velocity = velocity * Main.rand.NextFloat(0.8f, 1f);
		}
	}
}