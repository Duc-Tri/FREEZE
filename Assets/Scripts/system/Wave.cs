using UnityEngine;

namespace IHateWinter
{
    // Scroll main texture based on time
    public class Wave : MonoBehaviour
    {
        [SerializeField] float scrollSpeed = 0.25f;
        [SerializeField] float sinusMultiplier = 4;

        Renderer render;
        Material material;

        private void Awake()
        {
            render = GetComponent<Renderer>();
            material = render.material;
        }

        void Update()
        {
            float t = Time.time;
            float matOffsetX = t * scrollSpeed;
            float matOffsetY = Mathf.Sin(matOffsetX * sinusMultiplier);
            float meshOffsetY = Mathf.Cos(t) / 200f;

            material.SetTextureOffset("_MainTex", new Vector2(matOffsetX, matOffsetY));
            //render.material.EnableKeyword("_NORMALMAP");
            //material.SetTextureOffset("_BumpMap", new Vector2(-matOffsetY, matOffsetX));

            transform.localPosition += Vector3.up * meshOffsetY;
        }
    }
}
