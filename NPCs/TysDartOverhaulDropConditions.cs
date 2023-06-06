using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace TysDartOverhaul.NPCs
{
    //Drop condition: drops if killed by standard dart
    public class KilledByDartDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return info.npc.GetGlobalNPC<TysDartOverhaulGlobalNPC>().KilledByDart;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "Drops if killed by a standard dart";
        }
    }
}