using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TysDartOverhaul.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class ShroomiteVisor : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<TysDartOverhaulConfig>().AddShroomiteVisor;
        }
        public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Shroomite Visor");
            Tooltip.SetDefault("15% increased dart damage\n5% increased ranged critical strike chance");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		
		public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
           	Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<TysDartOverhaulPlayer>().DartDamage += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.ShroomiteBreastplate && legs.type == ItemID.ShroomiteLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Terraria.Localization.Language.GetTextValue("ArmorSetBonus.Shroomite");
            player.shroomiteStealth = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}