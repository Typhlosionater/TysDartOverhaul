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
using Terraria.GameContent;

namespace TysDartOverhaul.Items.Weapons
{
	public class PhasicDartEjector : ModItem
	{
		public static bool ConsumeAmmoFromProjHack = false;

		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns;
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
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

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return ConsumeAmmoFromProjHack;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("TysDartOverhaul/Items/Weapons/PhasicDartEjector_Glowmask").Value;
			Vector2 drawPos = Item.position - Main.screenPosition + (Item.Size / 2f);
			Main.spriteBatch.Draw(texture, drawPos, texture.Frame(), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<PhasicDartEjector_HeldProjectile>(), 0, 0, player.whoAmI);

			return false;
		}
	}

	public class PhasicDartEjector_HeldProjectile : ModProjectile
	{
		private const float MaxChargeFlashTime = 50f;

		private Player Owner => Main.player[Projectile.owner];

		public override void SetDefaults() {
			Projectile.width = 46;
			Projectile.height = 28;
			Projectile.aiStyle = -1;

			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;

			Projectile.ignoreWater = true;
		}

		private int numCharges = 0;
		private int chargeTimer = 0;

		private ref float VisualsTimer => ref Projectile.localAI[0];

		private int GlowmaskFrame {
			get => (int)Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public override void AI() {
			Vector2 ownerCenter = Owner.RotatedRelativePoint(Owner.MountedCenter);
			Vector2 toMouse = ownerCenter.DirectionTo(Main.MouseWorld);

			Projectile.Center = ownerCenter + toMouse * 20f;
			Projectile.rotation = toMouse.ToRotation();
			Projectile.spriteDirection = Projectile.direction = (toMouse.X > 0f).ToDirectionInt();

			Projectile.timeLeft = 2;
			Owner.ChangeDir(Projectile.direction);
			Owner.heldProj = Projectile.whoAmI;
			Owner.SetDummyItemTime(2);
			Owner.itemRotation = MathHelper.WrapAngle(float.Atan2(toMouse.Y * Projectile.direction, toMouse.X * Projectile.direction));

			int timeTillNextCharge = (int)(Owner.HeldItem.useTime * Owner.GetWeaponAttackSpeed(Owner.HeldItem));
			chargeTimer++;
			if (chargeTimer >= timeTillNextCharge && numCharges < 9) {
				numCharges++;
				chargeTimer = 0;

                if (numCharges % 2 != 0)
                {
                    SoundEngine.PlaySound(SoundID.Item15 with { Pitch = Utils.Remap(numCharges, 0f, 9f, -0.2f, 0.2f)}, Projectile.position);
                }

                if (numCharges == 9)
				{
					VisualsTimer = MaxChargeFlashTime;
                    SoundEngine.PlaySound(SoundID.Item115, Projectile.position);
                }
			}

			if (Main.myPlayer == Projectile.owner && !Main.mouseLeft) {
				Projectile.Kill();
				return;
			}

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

		public override void OnKill(int timeLeft) {
			if (numCharges < 3) {
                SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                return;
			}

            SoundEngine.PlaySound(SoundID.Item92, Projectile.position);

			PhasicDartEjector.ConsumeAmmoFromProjHack = true;
			Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemID);
			PhasicDartEjector.ConsumeAmmoFromProjHack = false;

			if (Projectile.owner != Main.myPlayer)
			{
				return;
			}

			for (int i = 0; i < numCharges; i++) {
				Vector2 velocity = Owner.Center.DirectionTo(Main.MouseWorld) * speed;
				velocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));
				velocity *= Main.rand.NextFloat(0.8f, 1f);
				Projectile.NewProjectile(Owner.GetSource_ItemUse_WithPotentialAmmo(Owner.HeldItem, usedAmmoItemID), Owner.Center, velocity, projToShoot, damage, knockback, Projectile.owner);
			}
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