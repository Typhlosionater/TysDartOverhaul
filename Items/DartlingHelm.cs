using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class DartlingHelm : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDarts;
        }

        public override void SetStaticDefaults() 
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
		
		public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
           	Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Lime;
            Item.vanity = true;
        }
    }
}