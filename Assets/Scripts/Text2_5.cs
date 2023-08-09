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

        private void Awake()
        {
            if (textMeshPro == null)
                textMeshPro = GetComponentInChildren<TextMeshPro>();

            if (textMeshPro != null)
                LookAtCameraManager.AddTextTransform(textMeshPro.transform);
            else
                LookAtCameraManager.AddTextTransform(this.transform);
        }
    }
}