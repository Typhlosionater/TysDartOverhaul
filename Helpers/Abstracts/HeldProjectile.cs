using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TysDartOverhaul.Helpers.Abstracts;

public abstract class HeldProjectile : ModProjectile
{
	/// <summary>Returns Main.player[Projectile.owner]</summary>
	public Player Owner => Main.player[Projectile.owner];

	/// <summary>How far away this projectile will appear from the player</summary>
	public float HoldOutOffset { get; set; }

	/// <summary>How much this projectile should be rotated when it points to the mouse</summary>
	public float RotationOffset { get; set; }

	/// <summary>How far from the center the projectile will be fired from, assuming the projectile looks like its sprite (facing to the right)</summary>
	public Vector2 MuzzleOffset { get; set; }

	/// <summary>This property acts as a frame counter</summary>
	public int AI_FrameCount { get; set; }

	/// <summary>The sound this held projectile will play when shooting a projectile</summary>
	public SoundStyle? ShootSound { get; set; } = null;

	/// <summary>The amount of random spread this weapon has</summary>
	public float TotalRandomSpread { get; set; } = 0f;

	private int AI_Lifetime { get; set; }
	
	public virtual int? UseTimeOverride { get => null; }

	// Helper property that applies attack speed for us
	public int UseTimeAfterBuffs {
		get {
			int useTime = UseTimeOverride ?? Owner.HeldItem.useTime;
			return (int)(useTime / Owner.GetWeaponAttackSpeed(Owner.HeldItem));
		}
	}

	public virtual void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Collision.CanHit(player.Center, 0, 0, position, 0, 0)) {
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
	}

	public virtual void SafeAI() { }
	public sealed override void AI() {
		Vector2 toMouse = Main.MouseWorld - Projectile.Center;
		toMouse.Normalize();
		bool hasAmmo = Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out _, true);
		bool doShoot = false; // Used so we can delay the calling of our actual shoot hook since we need rotation set early

		// Kill the projectile if we stop using it or can't use it
		if (((!Owner.channel || !hasAmmo) && AI_Lifetime <= 1) || Owner.CCed) {
			Projectile.Kill();
			return;
		}

		// Set our rotation and doShoot if we are shooting this frame
		if (AI_FrameCount % UseTimeAfterBuffs == 0) {
			// Set rotation
			Projectile.DirectionTo(Main.MouseWorld);
			Projectile.rotation = toMouse.ToRotation() + RotationOffset + Main.rand.NextFloat(-TotalRandomSpread / 2f, TotalRandomSpread / 2f);
			doShoot = true;
		}

		// Set some stuff based on our rotation
		Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + Projectile.rotation.ToRotationVector2() * HoldOutOffset;
		Projectile.velocity = Vector2.Zero;
		Projectile.direction = Math.Sign(Projectile.Center.X - Owner.Center.X);
		Projectile.spriteDirection = Projectile.direction;

		// Set some values on our player
		Owner.ChangeDir(Projectile.direction);
		Owner.heldProj = Projectile.whoAmI;
		Owner.itemRotation = Projectile.DirectionFrom(Owner.MountedCenter).ToRotation();
		if (Projectile.Center.X < Owner.MountedCenter.X) {
			Owner.itemRotation += (float)Math.PI;
		}

		Owner.itemRotation = MathHelper.WrapAngle(Owner.itemRotation);

		// Actually call our shoot hook
		if (doShoot && Projectile.owner == Main.myPlayer) {
			Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemId);

			// Get some other params
			EntitySource_ItemUse_WithAmmo source = new(Owner, Owner.HeldItem, usedAmmoItemId);
			Vector2 velocity = Projectile.rotation.ToRotationVector2().RotatedBy(-RotationOffset) * speed;
			Vector2 offset = new(MuzzleOffset.X, MuzzleOffset.Y * Projectile.direction);
			offset = offset.RotatedBy(Projectile.rotation);
			Vector2 position = Projectile.Center + offset;
			int amount = 1;
			float spread = 0f;

			for (int i = 0; i < amount; i++) {
				Vector2 perturbedVelocity = velocity.RotatedByRandom(spread);
				Shoot(Owner, source, position, perturbedVelocity, projToShoot, damage, knockback);
			}

			// This makes it so we keep our item used until the potential next shot
			Owner.SetDummyItemTime(UseTimeAfterBuffs + 1);

			AI_Lifetime = UseTimeAfterBuffs + 1;
			Projectile.netUpdate = true;
		}

		// Set timeleft
		Projectile.timeLeft = 2;

		AI_FrameCount++;
		AI_Lifetime--;

		SafeAI();
	}

	private Asset<Texture2D> _baseTexture;
	private Asset<Texture2D> BaseTexture => _baseTexture ??= ModContent.Request<Texture2D>(Texture);

	public override bool PreDraw(ref Color lightColor) {
		Vector2 position = Projectile.Center - Main.screenPosition;
		Rectangle frame = new(0, 0, BaseTexture.Width(), BaseTexture.Height());
		Color drawColor = lightColor;
		float rotation = Projectile.rotation;
		Vector2 origin = frame.Size() / 2f;
		float scale = Projectile.scale;
		SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

		Main.EntitySpriteDraw(BaseTexture.Value, position, frame, drawColor, rotation, origin, scale, effects, 0);
		
		return false;
	}

	// Sends and receives our ai fields, not even sure if we need this but w/e
	public sealed override void SendExtraAI(BinaryWriter writer) {
		writer.Write(AI_FrameCount);
		writer.Write(AI_Lifetime);
	}

	public sealed override void ReceiveExtraAI(BinaryReader reader) {
		AI_FrameCount = reader.ReadInt32();
		AI_Lifetime = reader.ReadInt32();
	}
}