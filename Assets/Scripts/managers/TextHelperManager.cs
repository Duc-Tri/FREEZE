using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace IHateWinter
{
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
            if (resource != null && Vector3.Distance(GameManager.Player.transform.position, resource.transform.position) < Player.MAX_DISTANCE_TO_HARVEST)
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
}