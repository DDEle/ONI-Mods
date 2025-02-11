﻿using FUtility;
using TUNING;
using UnityEngine;
using static Artable;
using static TwelveCyclesOfChristmas.STRINGS.BUILDINGS.PREFABS.TWELVECYCLESOFCHRISTMAS_SNOWSCULPTURE;

namespace TwelveCyclesOfChristmas.Buildings.SnowSculpture
{
    public class SnowSculptureConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.Prefix("SnowSculpture");
        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.FURNITURE, "GlassFurnishings", MarbleSculptureConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               3,
               "snowsculpture_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               MATERIALS.TRANSPARENTS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               new EffectorValues(20, 8),
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "slab";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isArtable = true;
            go.AddTag(GameTags.Decoration);
            go.AddComponent<SnowDog>();
            //go.AddTag(ModAssets.Tags.noPaintTag);
            //go.AddComponent<Fabulous>().offset = new Vector3(.5f, .5f, .4f);
        }


        public override void DoPostConfigureComplete(GameObject go)
        {
            Artable artable = go.AddComponent<Sculpture>();

            artable.stages.Add(new Stage("Default", NAME, "slab", 0, false, Status.Ready));
            //artable.stages.Add(new Stage("Bad", POORQUALITYNAME, "crap_1", 5, false, Status.Ugly));
            //artable.stages.Add(new Stage("Average", AVERAGEQUALITYNAME, "good_1", 10, false, Status.Okay));
            artable.stages.Add(new Stage("Good1", EXCELLENTQUALITYNAME, "amazing_1", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good2", DOG, "amazing_2", 15, true, Status.Great));
        }
    }
}
