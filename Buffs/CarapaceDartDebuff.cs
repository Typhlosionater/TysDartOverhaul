using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TysDartOverhaul.Projectiles.AmmoDartProjectiles;
using TysDartOverhaul.NPCs;

namespace TysDartOverhaul.Buffs
{
    public class CarapaceDartDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // NPCs will automatically be immune to this buff if they are immune to BoneJavelin. SkeletronHead and SkeletronPrime are immune to BoneJavelin.
            BuffID.Sets.GrantImmunityWith[Type].Add(BuffID.BoneJavelin);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<CarapaceDartDoTGlobalNPC>().CarapaceDartDebuff = true;
        }
    }
}

namespace TysDartOverhaul.NPCs
{
    public class CarapaceDartDoTGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool CarapaceDartDebuff;

        public override void ResetEffects(NPC npc)
        {
            CarapaceDartDebuff = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (CarapaceDartDebuff)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                // Count how many ExampleJavelinProjectile are attached to this npc.
                int CarapaceDartCount = 0;
                foreach (var p in Main.ActiveProjectiles)
                {
                    if (p.type == ModContent.ProjectileType<CarapaceDartProjectile>() && p.ai[1] == 1f && p.ai[2] == npc.whoAmI)
                    {
                        CarapaceDartCount++;
                    }
                }
                // Remember, lifeRegen affects the actual life loss, damage is just the text.
                // The logic shown here matches how vanilla debuffs stack in terms of damage numbers shown and actual life loss.
                npc.lifeRegen -= CarapaceDartCount * 25 * 2;
                if (damage < CarapaceDartCount * 25)
                {
                    damage = CarapaceDartCount * 25;
                }
            }
        }
    }
}