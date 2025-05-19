using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TysDartOverhaul.Items.Ammo
{
	public class EctoplasmDart : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
		}

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 26;

			Item.maxStack = 9999;
			Item.consumable = true;             
			Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(0, 0, 0, 20);

			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.EctoplasmDartProjectile>();
			Item.shootSpeed = 3f;
			Item.ammo = AmmoID.Dart;

			Item.alpha = 80;
			Item.color = new Color(255, 255, 255, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.Ectoplasm, 1)
				.Register();
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
            Vector2 drawOrigin = itemFrame.Size() / 2f;
            Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
            spriteBatch.Draw(itemTexture, drawPosition, itemFrame, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}