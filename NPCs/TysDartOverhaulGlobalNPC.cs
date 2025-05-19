using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TysDartOverhaul.Items;
using System.Linq;
using System.Collections.Generic;

using TysDartOverhaul.Items.Weapons;
using TysDartOverhaul.Items.Ammo;
using static System.Net.Mime.MediaTypeNames;
using TysDartOverhaul.Items.Accessories;

namespace TysDartOverhaul.NPCs
{
	public class TysDartOverhaulGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

        //New Loot Drops
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
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

			//If modded accessories are available
			if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewAccessories)
			{
                //Giant Flying Fox drops Huntsman Bandana at 2.5%
                if (npc.type == NPCID.GiantFlyingFox)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HuntsmanBandana>(), 40));
				}
			}
		}

        //New Sold Items
        public override void ModifyShop(NPCShop shop)
        {
			//If modded darts are available
            if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts)
            {
				//Merchant sells darts
                if (shop.NpcType == NPCID.Merchant)
                {
                    shop.InsertAfter(ItemID.WoodenArrow, ModContent.ItemType<Dart>());
                }
            }

			//If modded dartguns are available
            if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
            {
				//Arms dealer sells air rifle
                if (shop.NpcType == NPCID.ArmsDealer)
                {
                    shop.InsertAfter(ItemID.Minishark, ModContent.ItemType<AirRifle>());
                }

                //Arms dealer sells dartling gun post plantera
                if (shop.NpcType == NPCID.ArmsDealer)
                {
                    shop.InsertAfter(ItemID.Shotgun, ModContent.ItemType<DartlingGun>(), Condition.DownedPlantera);
                }

                //Steampunker sells clockwork ballista post golem
                if (shop.NpcType == NPCID.Steampunker)
                {
                    shop.InsertAfter(ItemID.SteampunkWings, ModContent.ItemType<ClockworkBallista>(), Condition.DownedGolem);
                }
            }
        }

        //Travelling merchant sold items
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			//Rare Items List
			int[] RareItemIds = new int[]
			{
			ItemID.Code1,
			ItemID.Code2,
			ItemID.ZapinatorGray,
			ItemID.ZapinatorOrange,
			ItemID.UnicornHornHat,
			ItemID.HeartHairpin,
			ItemID.StarHairpin,
			ItemID.Fedora,
			ItemID.GoblorcEar,
			ItemID.VulkelfEar,
			ItemID.PandaEars,
			ItemID.DevilHorns,
			ItemID.DemonHorns,
			ItemID.Gi,
			ItemID.GypsyRobe,
			ItemID.MagicHat,
			ItemID.Fez,
			ItemID.Revolver,
			ItemID.Pho
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