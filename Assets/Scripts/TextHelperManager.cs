using IHateWinter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TextHelperManager : MonoBehaviour
{
    static TextMeshPro text;
    static Transform staticTransform;
    private void Awake()
    {
        staticTransform = this.transform;
        text = GetComponent<TextMeshPro>();
    }


    public static void TextHover(AResource resource)
    {
        if (resource != null)
        {
            Vector3 v = resource.transform.position;
            v.y += resource.GetComponent<NavMeshObstacle>().size.y;
            staticTransform.position = v;

            text.text = $"{resource.name} / {resource.type} / {resource.tag}";
        }
        else
        {
            staticTransform.position = Vector3.one * -1000;
        }
    }
}
