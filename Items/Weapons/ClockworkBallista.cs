using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

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
			Item.useTime = 6;
			Item.useAnimation = 6;
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
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.ClockworkBallistaProjectile>();
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.type == ModContent.ProjectileType<Projectiles.ClockworkBallistaProjectile>() && projectile.owner == player.whoAmI)
				{
					return Main.rand.NextFloat() >= .5f ? projectile.ai[1] == 1f : false;
				}
			}

			return false;
		}

		private int HeldProjectile => ModContent.ProjectileType<Projectiles.ClockworkBallistaProjectile>();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Get position
			Vector2 heldProjPosition = player.RotatedRelativePoint(player.MountedCenter, true);

			Projectile.NewProjectile(source, heldProjPosition, Vector2.Zero, HeldProjectile, damage, knockback, player.whoAmI);

			// Manually spawned our projectile so we need to return false
			return false;
		}
	}
}