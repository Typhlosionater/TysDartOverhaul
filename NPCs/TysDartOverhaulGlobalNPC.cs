using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TysDartOverhaul.Items;
using System.Linq;
using System.Collections.Generic;

using TysDartOverhaul.Items.Weapons;
using TysDartOverhaul.Items.Ammo;

namespace TysDartOverhaul.NPCs
{
	public class TysDartOverhaulGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public bool KilledByDart;

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if ((projectile.type == ModContent.ProjectileType<Projectiles.AmmoDartProjectiles.DartProjectile>()) && (damage > npc.life))
			{
				KilledByDart = true;
			}
		}

        //New Loot Drops
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			//If modded darts are available
            if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts)
            {
				//Windy balloon drops dartling helm if killed by a standard dart
				if (npc.type == NPCID.WindyBalloon)
				{
					IItemDropRule KilledByStandardDart = new LeadingConditionRule(new KilledByDartDropCondition());
					KilledByStandardDart.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DartlingHelm>()));
					npcLoot.Add(KilledByStandardDart);
				}
			}

			//If modded dartguns are available
			if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
			{
				//Cursed skull drops Ancient crossbow at 6.67%, gets 1 extra roll in expert
				if (npc.type == NPCID.CursedSkull)
				{
					npcLoot.Add(ItemDropRule.ExpertGetsRerolls(ModContent.ItemType<AncientCrossbow>(), 15, 1));
				}

				//All ghoul types drop S.H.R.E.D.R at 2.5%, chances are increased to 3.33% in expert
				if (npc.type == NPCID.DesertGhoul || npc.type == NPCID.DesertGhoulCorruption || npc.type == NPCID.DesertGhoulCrimson || npc.type == NPCID.DesertGhoulHallow)
				{
					npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SHREDR>(), 40, 30));
				}

				//Lava bat drops Hellfire Blaster at 3.33%
				if (npc.type == NPCID.Lavabat)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HellfireBlaster>(), 30));
				}

				//Giant cursed skull drops Decapitator at 6.67%, gets 1 extra roll in expert
				if (npc.type == NPCID.GiantCursedSkull)
				{
					npcLoot.Add(ItemDropRule.ExpertGetsRerolls(ModContent.ItemType<Decapitator>(), 15, 1));
				}

				//Phasic Dart Ejector drops according to the same rule as the charged blaster cannon
				if (npc.type == NPCID.MartianWalker || npc.type == NPCID.MartianOfficer || npc.type == NPCID.GigaZapper || npc.type == NPCID.GrayGrunt || npc.type == NPCID.RayGunner || npc.type == NPCID.BrainScrambler || npc.type == NPCID.ScutlixRider || npc.type == NPCID.MartianEngineer)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhasicDartEjector>(), 800));
				}
			}
		}

		//New Sold Items
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			//If modded darts are available
			if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts)
			{
				//Merchant sells basic darts
				if (type == NPCID.Merchant)
				{
					//Create List
					List<Item> inventory = shop.item.ToList();

					//Find Slot to insert into
					Item NewItemSlot = inventory.FirstOrDefault(i => i.type == ItemID.WoodenArrow); //Change target
					int index = 11; //Change Default
					if (NewItemSlot != null)
						index = inventory.IndexOf(NewItemSlot) + 1;

					//Insert item into slot
					inventory.Insert(index, new(ModContent.ItemType<Dart>())); //Changes Item
					inventory[index].isAShopItem = true;
					nextSlot++;

					//Bruh Moment
					shop.item = inventory.ToArray();
				}
			}

			//If modded dartguns are available
			if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
			{
				//Arms dealer sells air rifle
				if (type == NPCID.ArmsDealer)
				{
					//Create List
					List<Item> inventory = shop.item.ToList();

					//Find Slot to insert into
					Item NewItemSlot = inventory.FirstOrDefault(i => i.type == ItemID.Minishark); //Change target
					int index = 6; //Change Default
					if (NewItemSlot != null)
						index = inventory.IndexOf(NewItemSlot) + 1;

					//Insert item into slot
					inventory.Insert(index, new(ModContent.ItemType<AirRifle>())); //Changes Item
					inventory[index].isAShopItem = true;
					nextSlot++;

					//Bruh Moment
					shop.item = inventory.ToArray();
				}

				//Arms dealer sells dartling gun post Plantera
				if (type == NPCID.ArmsDealer && Main.hardMode && NPC.downedPlantBoss)
				{
					//Create List
					List<Item> inventory = shop.item.ToList();

					//Find Slot to insert into
					Item NewItemSlot = inventory.FirstOrDefault(i => i.type == ItemID.Shotgun); //Change target
					int index = 8; //Change Default
					if (NewItemSlot != null)
						index = inventory.IndexOf(NewItemSlot) + 1;

					//Insert item into slot
					inventory.Insert(index, new(ModContent.ItemType<DartlingGun>())); //Changes Item
					inventory[index].isAShopItem = true;
					nextSlot++;

					//Bruh Moment
					shop.item = inventory.ToArray();
				}

				//Steampunker sells Clockwork Ballista post Golem
				if (type == NPCID.Steampunker && NPC.downedGolemBoss)
				{
					//Create List
					List<Item> inventory = shop.item.ToList();

					//Find Slot to insert into
					Item NewItemSlot = inventory.FirstOrDefault(i => i.type == ItemID.SteampunkWings); //Change target
					int index = 1; //Change Default
					if (NewItemSlot != null)
						index = inventory.IndexOf(NewItemSlot) + 1;

					//Insert item into slot
					inventory.Insert(index, new(ModContent.ItemType<ClockworkBallista>())); //Changes Item
					inventory[index].isAShopItem = true;
					nextSlot++;

					//Bruh Moment
					shop.item = inventory.ToArray();
				}
			}
		}

		//Travelling merchant sold items
		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			//Rare Items List
			int[] RareItemIds = new int[]
			{
			ItemID.PaintingTheSeason,
			ItemID.PaintingSnowfellas,
			ItemID.PaintingCursedSaint,
			ItemID.PaintingColdSnap,
			ItemID.PaintingAcorns,
			//ItemID.MoonmanandCompany,
			ItemID.PaintingTheTruthIsUpThere,
			ItemID.PaintingMartiaLisa,
			ItemID.PaintingCastleMarsberg,
			ItemID.MoonLordPainting,
			ItemID.Code1,
			ItemID.Code2,
			//ItemID.ZapinatorGray,
			//ItemID.ZapinatorOrange,
			ItemID.UnicornHornHat,
			ItemID.HeartHairpin,
			ItemID.StarHairpin,
			ItemID.Fedora,
			//ItemID.GoblorcEar,
			//ItemID.VulkelfEar,
			ItemID.PandaEars,
			ItemID.DevilHorns,
			ItemID.DemonHorns,
			ItemID.Gi,
			ItemID.GypsyRobe,
			ItemID.MagicHat,
			ItemID.Fez,
			ItemID.Revolver
			};

			//If modded dartguns are available
			if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
			{
				//Travelling merchant sells dart glove during pre-Hardmode after defeating the Eoc, EoW, BoC, Queen Bee, or Skeletron
				if (!Main.hardMode && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee))
				{
					for (int i = 0; i < shop.Length; i++)
					{
						if (RareItemIds.Contains(shop[i]) && Main.rand.NextBool(RareItemIds.Length + 1))
						{
							shop[i] = ModContent.ItemType<DartGlove>();
							return;
						}
					}
				}

				//Travelling merchant sells triple dart glove during Hardmode
				if (Main.hardMode)
				{
					for (int i = 0; i < shop.Length; i++)
					{
						if (RareItemIds.Contains(shop[i]) && Main.rand.NextBool(RareItemIds.Length + 1))
						{
							shop[i] = ModContent.ItemType<TripleDartGlove>();
							return;
						}
					}
				}
			}
		}
	}
}