using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class PredatorsBandana : ModItem
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
            Item.width = 32;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Pink;       
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TysDartOverhaulPlayer>().DartDamage += 0.1f;
            player.GetModPlayer<TysDartOverhaulPlayer>().HuntsmanBandana = true;
            player.GetModPlayer<TysDartOverhaulPlayer>().PoisonedDarts = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HuntsmanBandana>(), 1)
                .AddIngredient(ItemID.Bezoar, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}