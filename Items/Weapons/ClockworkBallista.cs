using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using TysDartOverhaul.Helpers.Abstracts;

namespace TysDartOverhaul.Items.Weapons
{
	public class ClockworkBallista : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clockwork Ballista");
			Tooltip.SetDefault("50% chance to not consume darts");

			ItemID.Sets.gunProj[Type] = true; // Seems like all this does is setup PickAmmo correctly

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.knockBack = 2.5f;
			Item.channel = true;

			Item.width = 78;
			Item.height = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			//Item.UseSound = SoundID.Item36;

			Item.value = Item.sellPrice(0, 40, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<ClockworkBallista_HeldProjectile>();
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player) => player.heldProj != -1;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ClockworkBallista_HeldProjectile>(), damage, knockback, player.whoAmI);
			
			return false;
		}
	}

	public class ClockworkBallista_HeldProjectile : HeldProjectile
	{
		public override void SetDefaults() {
			// Base stats
			Projectile.width = 78;
			Projectile.height = 36;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			// Weapon stats
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;

			// HeldProjectile stats
			HoldOutOffset = 32f;
			RotationOffset = 0f;
			
			MuzzleOffset = new Vector2(32f, -2f);
		}

		public override int? UseTimeOverride => AI_FrameCount switch {
			<= 30 => null,
			<= 60 => 28,
			<= 90 => 22,
			> 120 => 16
		};
	}
}