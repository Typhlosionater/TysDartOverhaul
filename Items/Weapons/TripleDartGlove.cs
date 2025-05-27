using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TysDartOverhaul.Items.Weapons
{
	public class TripleDartGlove : ModItem
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
			Item.damage = 30;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.knockBack = 3.8f;

			Item.width = 16;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;

			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//Fires an even spread of 3 darts in 20 degrees
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(20);

			float AngleBetween = rotation / (numberProjectiles - 1);
			float BaseAngle = -(rotation / 2);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 PerturbedSpeed = velocity.RotatedBy(BaseAngle + (i * AngleBetween));
				Projectile.NewProjectile(source, position, PerturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
    }


	public class TripleDartGlovePlayerLayer : PlayerDrawLayer
	{
		private Asset<Texture2D> TripleDartGloveTexture;

		public override bool IsHeadLayer => false;

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HandOnAcc);

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<TripleDartGlove>();

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (TripleDartGloveTexture == null)
			{
				TripleDartGloveTexture = ModContent.Request<Texture2D>("TysDartOverhaul/Items/Weapons/TripleDartGlove_HandsOn");
			}

			Vector2 position = drawInfo.Center - Main.screenPosition + new Vector2(-5f * drawPlayer.direction, -3f);
			position = new Vector2((int)position.X, (int)position.Y);

			drawInfo.DrawDataCache.Add(new DrawData(
				TripleDartGloveTexture.Value,
				position,
				drawInfo.compFrontArmFrame,
				drawInfo.colorArmorBody,
				drawPlayer.bodyRotation + drawInfo.compositeFrontArmRotation,
				drawInfo.bodyVect + new Vector2(-5 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f),
				1f,
				drawInfo.playerEffect,
				0
			));
		}
	}
}