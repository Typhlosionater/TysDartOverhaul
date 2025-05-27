using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TysDartOverhaul.Items.Weapons
{
	public class DartlingGun : ModItem
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
			Item.damage = 36;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.knockBack = 2f;

			Item.width = 44;
			Item.height = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item98;

			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 15f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 5);
		}

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
			return Main.rand.NextFloat() >= .5f;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			//Align Darts
			position.Y += 5f * player.gravDir;

			//inaccuracy
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10f));
		}
	}
}