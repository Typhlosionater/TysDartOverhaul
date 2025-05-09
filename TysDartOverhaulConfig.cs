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
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddNewDartguns { get; set; }

        //Add Modded Darts
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddNewDarts { get; set; }

        //Add Modded Dart Accessories
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AddNewAccessories { get; set; }

        //Vanilla Dart Changes
        [DefaultValue(true)]
        [ReloadRequired]
        public bool VanillaDartChanges { get; set; }
    }
}
