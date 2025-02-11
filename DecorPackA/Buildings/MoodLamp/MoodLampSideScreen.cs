﻿using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
    class MoodLampSideScreen : SideScreenContent
    {
        [SerializeField]
        private RectTransform buttonContainer;

        private GameObject stateButtonPrefab;
        private GameObject debugVictoryButton;
        private GameObject flipButton;
        private readonly List<GameObject> buttons = new List<GameObject>();
        private MoodLamp target;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<MoodLamp>() != null;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            // the monument screen used here has 2 extra buttons that are not needed, disabling them
            flipButton.SetActive(false);
            debugVictoryButton.SetActive(false);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";
            stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
            buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
            debugVictoryButton = transform.Find("Butttons/Button").gameObject;
            flipButton = transform.Find("Butttons/FlipButton").gameObject;
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
            this.target = target.GetComponent<MoodLamp>();
            gameObject.SetActive(true);
            GenerateStateButtons();
        }

        // Creates clickable card buttons for all the lamp types + a randomizer button
        private void GenerateStateButtons()
        {
            ClearButtons();
            KAnimFile animFile = target.GetComponent<KBatchedAnimController>().AnimFiles[0];

            // random button
            AddButton(animFile, "random_ui", STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP.VARIANT.RANDOM, () => target.SetVariant(target.GetRandom()));

            // all the types
            foreach (KeyValuePair<string, MoodLamp.Variant> variant in MoodLamp.variants)
            {
                AddButton(animFile, variant.Key + "_ui", variant.Value.description, () => target.SetVariant(variant.Key));
            }
        }


        private void AddButton(KAnimFile animFile, string animName, LocString tooltip, System.Action onClick)
        {
            GameObject gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

            if (gameObject.TryGetComponent(out KButton button))
            {
                button.onClick += onClick;
                button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, animName);
            }

            FUtility.FUI.Helper.AddSimpleToolTip(gameObject, tooltip, true);
            buttons.Add(gameObject);
        }

        private void ClearButtons()
        {
            foreach (GameObject button in buttons)
            {
                Util.KDestroyGameObject(button);
            }

            buttons.Clear();

            flipButton.SetActive(false);
            debugVictoryButton.SetActive(false);
        }
    }
}