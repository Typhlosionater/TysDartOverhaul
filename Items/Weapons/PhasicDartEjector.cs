using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using TysDartOverhaul.Helpers.Abstracts;
using Terraria.GameContent;

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
			Item.noUseGraphic = true;

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
		public override bool CanConsumeAmmo(Item ammo, Player player) => player.heldProj != -1;

		//Channels PhasicDartEjectorProjectile instead of firing darts
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<PhasicDartEjector_HeldProjectile>(), damage, knockback, player.whoAmI);

			return false;
		}
	}

	public class PhasicDartEjector_HeldProjectile : HeldProjectile
	{
		private const float MaxChargeFlashTime = 50f;

		public override void SetDefaults() {
			// Base stats
			Projectile.width = 46;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			// Weapon stats
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;

			// HeldProjectile stats
			HoldOutOffset = 16f;
			RotationOffset = 0f;

			MuzzleOffset = new Vector2(32f, -2f);
		}

		private int numCharges = 0;

		public override void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			numCharges++;
			numCharges = Math.Clamp(numCharges, 0, 9);
			if (numCharges == 9f && GlowmaskFrame != 3) {
				GlowmaskFrame = 3;
				VisualsTimer = MaxChargeFlashTime;
			}
		}
		
		private ref float VisualsTimer => ref Projectile.localAI[0];

		private int GlowmaskFrame {
			get => (int)Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public override void SafeAI() {
			Projectile.rotation = Projectile.AngleTo(Main.MouseWorld);

			if (numCharges >= 9) {
				VisualsTimer--;
				return;
			}
			
			float numChargesNormalised = numCharges / 9f;
			float timeForNextFrame = MathHelper.Lerp(15f, 4f, numChargesNormalised);

			if (VisualsTimer >= timeForNextFrame) {
				VisualsTimer = 0f;
				GlowmaskFrame++;
				if (GlowmaskFrame > 2) {
					GlowmaskFrame = 0;
				}
			}
			
			VisualsTimer++;
		}

		public override void Kill(int timeLeft) {
			if (numCharges < 3) {
                SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                return;
			}
			
			Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemID);
			for (int i = 0; i < numCharges; i++) {
				Vector2 velocity = Owner.Center.DirectionTo(Main.MouseWorld) * speed;
				velocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));
				velocity *= Main.rand.NextFloat(0.8f, 1f);
				Projectile.NewProjectile(Owner.GetSource_ItemUse_WithPotentialAmmo(Owner.HeldItem, usedAmmoItemID), Owner.Center, velocity, projToShoot, damage, knockback, Projectile.owner);
			}
            SoundEngine.PlaySound(SoundID.Item92, Projectile.position);
        }

		private Asset<Texture2D> _glowmask;
		private Asset<Texture2D> Glowmask => _glowmask ??= ModContent.Request<Texture2D>("TysDartOverhaul/Items/Weapons/PhasicDartEjector_HeldProjectile_Glowmask");
		private Asset<Texture2D> _flash;
		private Asset<Texture2D> Flash => _flash ??= ModContent.Request<Texture2D>("TysDartOverhaul/Items/Weapons/PhasicDartEjector_HeldProjectile_Flash");

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			Rectangle sourceRect = texture.Frame();
			Color drawColor = lightColor;
			if (numCharges == 9 && VisualsTimer > 0f) {
				float flashLerp = MathF.Pow(VisualsTimer / MaxChargeFlashTime, 2);
				drawColor = Color.Lerp(lightColor, Color.White, flashLerp);
			}
			float rotation = Projectile.rotation;
			Vector2 origin = sourceRect.Size() / 2f;
			float scale = Projectile.scale;
			SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			Main.EntitySpriteDraw(texture, drawPosition, sourceRect, drawColor, rotation, origin, scale, effects, 0);

			Texture2D glowTexture = Glowmask.Value;
			Rectangle glowSourceRect = glowTexture.Frame(1, 4, 0, GlowmaskFrame);
			Main.EntitySpriteDraw(glowTexture, drawPosition, glowSourceRect, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);

			if (numCharges == 9 && VisualsTimer > 0f) {
				Texture2D flashTexture = Flash.Value;
				Rectangle flashSourceRect = flashTexture.Frame();
				float flashLerp = MathF.Pow(VisualsTimer / MaxChargeFlashTime, 2);
				Color flashDrawColor = Color.White * flashLerp;
				Vector2 flashOrigin = flashSourceRect.Size() / 2f;
				Main.EntitySpriteDraw(flashTexture, drawPosition, flashSourceRect, flashDrawColor, Projectile.rotation, flashOrigin, Projectile.scale, effects, 0);
			}

			return false;
		}
	}
}