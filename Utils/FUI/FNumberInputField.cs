﻿using System;
using UnityEngine;

namespace FUtility.FUI
{
    public class FNumberInputField : FInputField
    {
        public int maxValue = int.MaxValue;
        public int minValue = int.MinValue;

        public float GetFloat
        {
            get
            {
                float.TryParse(inputField.text, out float val);
                val = Mathf.Clamp(val, minValue, maxValue);
                return val;
            }
        }

        // approximate
        public T GetValue<T>()
        {
            if (float.TryParse(inputField.text, out float val))
            {
                val = Mathf.Clamp(val, minValue, maxValue);
                T result = (T)Convert.ChangeType(val, typeof(T));
                return result;
            }

            return default;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        protected override void ProcessInput(object input)
        {
            string inputString = input.ToString();
            if (!inputString.IsNullOrWhiteSpace()) 
                base.ProcessInput(GetValue<int>());
        }
    }
}
