using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IHateWinter
{
    public class AmountBar : MonoBehaviour
    {
        [SerializeField] private float maxValue; // in seconds
        [SerializeField] private SpriteRenderer bar;

        [SerializeField] private float currentValue;
        private Vector3 scale;

        public bool IsOver => currentValue <= 0;

        private void Awake()
        {
            scale = Vector3.one;
            //currentValue = maxValue;
            bar.transform.localScale = scale;

            UpdateValue(currentValue);
        }

        public void UpdateValue(float v)
        {
            currentValue = v;
            scale.x = currentValue / maxValue;
            bar.transform.localScale = scale;
            bar.color = (currentValue <= 0) ? Color.clear : Color.yellow;
        }

        internal void UpdateDeltaTime(float deltaTime)
        {
            UpdateValue(currentValue - deltaTime);
        }

        internal void Init(float durationSeconds)
        {
            currentValue = maxValue = durationSeconds;
        }

        //private void Update()
        //{
        //    UpdateValue(Random.value * maxValue);
        //}

    }

}
