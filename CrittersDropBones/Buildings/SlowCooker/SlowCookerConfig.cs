﻿using FUtility;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using static ComplexRecipe;
using static CrittersDropBones.Settings.RecipesConfig;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class SlowCookerConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.Prefix("Cooker");

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.FOOD, Consts.TECH.FOOD.FOOD_REPURPOSING);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               2,
               3,
               "cdb_cooker_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
               MATERIALS.ALL_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER2,
               BuildLocationRule.OnFloor,
               DECOR.PENALTY.TIER0,
               NOISE_POLLUTION.NONE
           );

            def.Floodable = true;
            def.Overheatable = false;

            def.AudioCategory = Consts.AUDIO_CATEGORY.HOLLOWMETAL;
            def.ViewMode = OverlayModes.Power.ID;

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = .40f;
            def.SelfHeatKilowattsWhenActive = 1f;

            def.DefaultAnimState = "off";

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;

            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

            go.AddOrGet<Stirrable>();

            StirrableWorkable stirrableWorkable = go.AddOrGet<StirrableWorkable>();
            stirrableWorkable.workTime = 20f;
            stirrableWorkable.overrideAnims = new KAnimFile[]
            {
                //Assets.GetAnim("anim_interacts_compost_kanim")
                Assets.GetAnim("anim_interacts_cookingpot_kanim")
            };
            stirrableWorkable.AnimOffset = new Vector3(-1f, 0f, 0f);

            ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
            complexFabricator.heatedTemperature = 353.15f;
            complexFabricator.duplicantOperated = false;
            complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

            go.AddOrGet<FabricatorIngredientStatusManager>();

            BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);

            ConfigureRecipes();

            // emit Steam
            BuildingElementEmitter emitter = go.AddComponent<BuildingElementEmitter>();
            emitter.emitRate = 0.01f;
            emitter.temperature = 378.15f;
            emitter.element = SimHashes.Steam;
            emitter.modifierOffset = new Vector2(2f, 3f);

            Prioritizable.AddRef(go);
        }

        // These are read in from a .json
        private void ConfigureRecipes()
        {
            // TODO: sanity checks
            foreach (FRecipe recipe in Mod.Recipes.Recipes)
            {
                RecipeElement[] inputs = recipe.Inputs.Select(i => new RecipeElement(i.ID, i.Amount)).ToArray();
                RecipeElement[] outputs = recipe.Outputs.Select(o => new RecipeElement(o.ID, o.Amount)).ToArray();
                
                var result = AddRecipe(ID, inputs, outputs, recipe.Description);
            }
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGet<LoopingSounds>();

            //go.AddOrGetDef<PoweredController.Def>();
        }

        public ComplexRecipe AddRecipe(string fabricatorID, RecipeElement[] input, RecipeElement[] output, string desc, float time = 40f)
        {
            string recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

            var recipe = new ComplexRecipe(recipeID, input, output)
            {
                time = time,
                description = desc,
                nameDisplay = RecipeNameDisplay.Result,
                fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
            };

            return recipe;
        }
    }
}
