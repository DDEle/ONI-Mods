﻿using System.Collections.Generic;
using UnityEngine;
using static EdiblesManager;

namespace CrittersDropBones.Items
{
    public class SurimiConfig : IEntityConfig
    {
        public const string ID = "CDB_Surimi";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.CDB_SURIMI.NAME,
                STRINGS.ITEMS.FOOD.CDB_SURIMI.DESC,
                1f,
                false,
                Assets.GetAnim("cdb_surimi_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.4f,
                true,
                0,
                SimHashes.Creature,
                null);


            FoodInfo foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                3200f * 1000f,
                TUNING.FOOD.FOOD_QUALITY_GOOD,
                TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
                TUNING.FOOD.SPOIL_TIME.DEFAULT,
                true)
                .AddEffects(new List<string>
                {
                    "SeafoodRadiationResistance"
                }, DlcManager.AVAILABLE_ALL_VERSIONS);

            GameObject gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
            ComplexRecipeManager.Get().GetRecipe(recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
            return gameObject;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
