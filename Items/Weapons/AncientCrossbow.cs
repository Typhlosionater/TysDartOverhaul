using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Mono.Cecil;

namespace TysDartOverhaul.Items.Weapons
{
	public class AncientCrossbow : ModItem
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
			Item.damage = 46;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 5.25f;
            Item.crit += 4;

            Item.width = 38;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item102;

			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 14f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //Changes standard darts into necro darts
            if (type == ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>())
            {
                type = ModContent.ProjectileType<Projectiles.ConvertedDartProjectiles.NecroDartProjectile>();
            }
        }
	}
}