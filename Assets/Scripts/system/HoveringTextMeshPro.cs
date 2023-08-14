using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IHateWinter
{
    public class HoveringTextMeshPro : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMeshPro;
        [SerializeField] private float MAX_DURATION;
        float duration = 0f; //float.MaxValue;

        //private void OnEnable()        {duration = MAX_DURATION;        }

        private void Awake()
        {
            if (textMeshPro == null) textMeshPro = GetComponent<TextMeshPro>();
        }

        // Update is called once per frame
        void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void WakeUp(string message)
        {
            textMeshPro.text = message;
            this.gameObject.SetActive(true);
            duration = MAX_DURATION;
        }

        internal void Off()
        {
            duration = 0;
        }

    }

}
