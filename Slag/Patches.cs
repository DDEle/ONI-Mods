﻿using Database;
using Harmony;
using ProcGen;
using Slag.Buildings;
using Slag.Critter;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Slag
{
    class Patches
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                GameTags.MaterialBuildingElements.Add(ModAssets.slagWoolTag);
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {

            public static void Prefix()
            {
                string ManualInsulatedDoorID = ManualInsulatedDoorConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ManualInsulatedDoorID}.NAME", ManualInsulatedDoorConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ManualInsulatedDoorID}.EFFECT", ManualInsulatedDoorConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ManualInsulatedDoorID}.DESC", ManualInsulatedDoorConfig.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", ManualInsulatedDoorConfig.ID);


                string MechanizedInsulatedDoorID = InsulatedMechanizedAirlockConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MechanizedInsulatedDoorID}.NAME", InsulatedMechanizedAirlockConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MechanizedInsulatedDoorID}.EFFECT", InsulatedMechanizedAirlockConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MechanizedInsulatedDoorID}.DESC", InsulatedMechanizedAirlockConfig.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", InsulatedMechanizedAirlockConfig.ID);

                string SpinnedID = SpinnerConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SpinnedID}.NAME", SpinnerConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SpinnedID}.EFFECT", SpinnerConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SpinnedID}.DESC", SpinnerConfig.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", SpinnerConfig.ID);

                ModUtil.AddBuildingToPlanScreen("Base", FiltrationTileConfig.ID);
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var myRandom = new SeededRandom(0);
                var refinedMetalOptions = new List<WeightedMetalOption>()
                {
                    new WeightedMetalOption(SimHashes.Aluminum,             .8f),
                    new WeightedMetalOption(SimHashes.Copper,               .8f),
                    new WeightedMetalOption(SimHashes.Gold,                 .3f),
                    new WeightedMetalOption(SimHashes.Iron,                 1f),
                    new WeightedMetalOption(SimHashes.Lead,                 .5f),
                    new WeightedMetalOption(SimHashes.Mercury,              .6f),
                    new WeightedMetalOption(SimHashes.Niobium,              .02f),
                    new WeightedMetalOption(SimHashes.Steel,                .05f),
                    new WeightedMetalOption(SimHashes.TempConductorSolid,   .01f),
                    new WeightedMetalOption(SimHashes.Tungsten,             .03f),
                };

                List<string> results = new List<string>();
                for (var i = 0; i < 1000; i++)
                {
                    var chosenMetal = WeightedRandom.Choose(refinedMetalOptions, myRandom);
                    //Console.WriteLine(chosenMetal.element.ToString());
                    results.Add(chosenMetal.element.ToString());
                }

                var query = results
                    .GroupBy(s => s)
                    .Select(g => new { Name = g.Key, Count = g.Count() });

              
                foreach (var result in query)
                {
                    Console.WriteLine("Name: {0}, Count: {1}", result.Name, result.Count);
                }


                var techList = new List<string>(Techs.TECH_GROUPING["TemperatureModulation"]) {
                        ManualInsulatedDoorConfig.ID,
                        InsulatedMechanizedAirlockConfig.ID };

                Techs.TECH_GROUPING["TemperatureModulation"] = techList.ToArray();
            }
        }
    }
}
