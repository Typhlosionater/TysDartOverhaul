using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace TysDartOverhaul
{
    public class TysDartOverhaulPlayer : ModPlayer
    {
        //Initialise Special triggers
        public float DartDamage;

        //Reset Triggers
        public override void ResetEffects()
        {
            DartDamage = 1f;
        }

        //Fishes up poison dart frog
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            //If modded dartguns are available
            if (ModContent.GetInstance<TysDartOverhaulConfig>().AddNewDartguns)
            {
                if (!attempt.inLava && !attempt.inHoney && Player.ZoneJungle && attempt.legendary && Main.hardMode && Main.rand.Next(2) == 0)
                {
                    npcSpawn = -1;
                    itemDrop = ModContent.ItemType<Items.Weapons.PoisonDartFrog>();
                }
            }
        }
    }
}
