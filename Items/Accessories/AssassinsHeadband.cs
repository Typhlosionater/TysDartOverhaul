using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class AssassinsHeadband : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<TysDartOverhaulConfig>().AddNewAccessories;
        }

        public override void SetStaticDefaults() 
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		
		public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Pink;       
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.aggro -= 400;
            player.GetModPlayer<TysDartOverhaulPlayer>().DartDamage += 0.1f;
            player.GetModPlayer<TysDartOverhaulPlayer>().HuntsmanBandana = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HuntsmanBandana>(), 1)
                .AddIngredient(ItemID.PutridScent, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}