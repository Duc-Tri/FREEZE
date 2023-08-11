using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IHateWinter
{

    public class Text2_5 : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro textMeshPro;

        static Sprite[] trees;

        private void Start()
        {
            if (textMeshPro == null)
                textMeshPro = GetComponentInChildren<TextMeshPro>();

            if (textMeshPro != null)
                BillBoardingManager.AddTextTransform(textMeshPro.transform);
            else
                BillBoardingManager.AddTextTransform(this.transform);
        }
    }
}