using UnityEngine;

namespace IHateWinter
{
    // Scroll main texture based on time
    public class Wave : MonoBehaviour
    {
        [SerializeField] float scrollSpeed = 0.25f;
        [SerializeField] float sinusMultiplier = 4;

        Renderer rend;
        Material material;

        void Start()
        {
            rend = GetComponent<Renderer>();
            material = rend.material;
        }

        void Update()
        {
            float offsetX = Time.time * scrollSpeed;
            float offsetY = Mathf.Sin(offsetX * sinusMultiplier);

            material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
        }
    }
}
