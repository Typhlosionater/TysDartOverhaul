using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TysDartOverhaul.Items.Weapons
{
	public class PoisonDartFrog : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Dart Frog");
			Tooltip.SetDefault("Fires a spread of poisonous darts");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 5f;

			Item.width = 30;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.NPCDeath13;

			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//Changes standard darts into poison darts
			if (type == ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>())
			{
				type = ProjectileID.PoisonDartBlowgun;
			}

			//Fires a random spread of 4 darts in 20 degrees
			float numberProjectiles = 4;
			float SpreadAngle = MathHelper.ToRadians(20);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 PerturbedSpeed = velocity.RotatedByRandom(SpreadAngle);
				PerturbedSpeed = PerturbedSpeed * Main.rand.NextFloat(0.75f, 1f);
				Projectile.NewProjectile(source, position, PerturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}