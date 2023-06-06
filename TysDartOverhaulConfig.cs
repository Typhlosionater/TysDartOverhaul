using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace TysDartOverhaul
{
    [Label("Mod Config")]
    class TysDartOverhaulConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        //Add Modded Dartguns
        [Label("Add new modded dartguns")]
        [Tooltip("Adds new modded dartguns to various points in progression. (Enabled by default)")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddNewDartguns { get; set; }

        //Add Modded Darts
        [Label("Add new modded darts")]
        [Tooltip("Adds a variety of new modded dart ammunition types. (Enabled by default)")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddNewDarts { get; set; }

        //Add Modded Dart Accessories
        [Label("Add new modded dart accessories")]
        [Tooltip("Adds new modded dart subclass accessories. (Enabled by default)")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddNewAccessories { get; set; }

        //Add Shroomite Helmet
        [Label("Add a new modded Shroomite Headpiece")]
        [Tooltip("Adds a new modded Shroomite Headpiece for the dart subclass. (Enabled by default)")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddShroomiteVisor { get; set; }

        //Vanilla Dart Changes
        [Label("Changes to vanilla darts")]
        [Tooltip("Changes ichor dart and crystal darts to use local immunity frames and cursed dart flames to use static immunity frames. (Enabled by default)")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool VanillaDartChanges { get; set; }
    }
}
