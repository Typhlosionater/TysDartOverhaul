using Terraria;
using Terraria.ModLoader;

namespace TysDartOverhaul;

public class CalamityCompat : GlobalItem
{
    public override bool IsLoadingEnabled(Mod mod)
    {
        return ModLoader.HasMod("CalamityMod") || ModLoader.HasMod("FargowiltasSouls");
    }

    public override void SetDefaults(Item entity)
    {
        if (entity.ModItem?.Mod is TysDartOverhaul)
        {
            entity.damage = (int)(entity.damage * 1.2f);
        }
    }
}