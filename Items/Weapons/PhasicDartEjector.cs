using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TysDartOverhaul.Items.Weapons
{
	public class PhasicDartEjector : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phasic Dart Ejector");
			Tooltip.SetDefault("Charges up a blast of up to 9 darts");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 4f;

			Item.channel = true;

			Item.width = 46;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			//Item.noUseGraphic = true;

			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("TysDartOverhaul/Items/Weapons/PhasicDartEjector_Glowmask").Value;
			Main.spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		//Stops the player consuming ammo from channeling
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return false;
		}

		//Channels PhasicDartEjectorProjectile instead of firing darts
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Creates our PhasicDartEjectorProjectile if needed
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.PhasicDartEjectorProjectile>()] < 1)
			{
				Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.PhasicDartEjectorProjectile>(), Item.damage, Item.knockBack, player.whoAmI);
			}

			return false;
		}
	}
}