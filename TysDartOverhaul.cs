using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;

namespace TysDartOverhaul
{
	public class TysDartOverhaul : Mod
	{
		public override void PostSetupContent()
		{
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
				{
					bossChecklist.Call("AddToBossLoot", "Terraria", "Martian Madness", ModContent.ItemType<Items.Weapons.PhasicDartEjector>());
				}
			}
		}
	}
}