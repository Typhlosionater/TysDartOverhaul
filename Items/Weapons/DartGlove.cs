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
	public class DartGlove : ModItem
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
			Item.damage = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 3.5f;

			Item.width = 16;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;

			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 8f;
			Item.useAmmo = AmmoID.Dart;
		}
    }

    public class DartGlovePlayerLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> DartGloveTexture;

        public override bool IsHeadLayer => false;

        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HandOnAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<DartGlove>();

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            if (DartGloveTexture == null)
            {
                DartGloveTexture = ModContent.Request<Texture2D>("TysDartOverhaul/Items/Weapons/DartGlove_HandsOn");
            }

            Vector2 position = drawInfo.Center - Main.screenPosition + new Vector2(-5f * drawPlayer.direction, -3f);
            position = new Vector2((int)position.X, (int)position.Y);

            drawInfo.DrawDataCache.Add(new DrawData(
                DartGloveTexture.Value,
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