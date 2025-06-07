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

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.knockBack = 2.5f;

			Item.width = 78;
			Item.height = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.autoReuse = true;
			//Item.UseSound = SoundID.Item36;

			Item.value = Item.sellPrice(0, 40, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override bool? UseItem(Player player)
		{
			player.GetModPlayer<ClockworkBallistaPlayer>().UsedItem();

			return base.UseItem(player);
		}
	}

	public class ClockworkBallistaPlayer : ModPlayer
	{
		private const int MaxRampUp = 8;
		private const int MaxInitialRampDownDelay = 45;
		private const int MaxRampDownDelay = 15;

		private int _rampUp = 0;
		private int _rampDownDelayTimer = 0;

		public void UsedItem()
		{
			_rampUp = int.Clamp(_rampUp + 1, 0, MaxRampUp);
			_rampDownDelayTimer = MaxInitialRampDownDelay;
		}

		public override void PostUpdateMiscEffects()
		{
			_rampDownDelayTimer--;
			if (_rampDownDelayTimer <= 0)
			{
				_rampUp = int.Clamp(_rampUp - 1, 0, MaxRampUp);
				_rampDownDelayTimer = MaxRampDownDelay;
			}

			//Main.NewText("ramp: " + _rampUp);
			//Main.NewText("delay: " + _rampDownDelayTimer);
		}

		public override float UseTimeMultiplier(Item item)
		{
			if (item.ModItem is not ClockworkBallista)
			{
				return base.UseTimeMultiplier(item);
			}

			return _rampUp switch
			{
				>= 0 and < 2 => 1f,
				>= 2 and < 4 => 0.85f,
				>= 4 and < 6 => 0.55f,
				>= 6 and < 8 => 0.35f,
				_ => 0.15f,
			};
		}
	}
}