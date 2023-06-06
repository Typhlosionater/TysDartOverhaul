using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TysDartOverhaul.Items.Weapons
{
	public class Atlatl : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlatl");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.knockBack = 2.5f;

			Item.width = 32;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;

			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RichMahogany, 10)
				.AddIngredient(ItemID.Vine, 2)
				.AddIngredient(ItemID.JungleSpores, 8)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			//Fires from atlatl head on swing
			position.Y -= 45f * player.gravDir;
			position.X -= 15f * player.direction;

			//Corrected velocity
			Vector2 Target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (player.gravDir == -1)
            {
				Target = Main.screenPosition + new Vector2(Main.mouseX, Main.screenHeight - Main.mouseY);
			}
			Vector2 NewVelocity = Target - position;
			NewVelocity.Normalize();
			velocity = NewVelocity * velocity.Length();

			//inaccuracy
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8f));
		}
	}
}