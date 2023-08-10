using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    // An abstract resource (clickable on screen)
    public abstract class AResource : MonoBehaviour
    {
        public static Dictionary<RESOURCE, string> resourcesPath = new Dictionary<RESOURCE, string> {
            { RESOURCE.TREE, "trees" },
            { RESOURCE.FLINT, "flints" },
            { RESOURCE.STONE, "stones" } };

        public static Dictionary<RESOURCE, Sprite[]> spritesResources = new Dictionary<RESOURCE, Sprite[]> {
            { RESOURCE.TREE, null },
            { RESOURCE.FLINT, null },
            { RESOURCE.STONE, null } };

        public RESOURCE type = RESOURCE.NONE;

        [SerializeField] private SpriteRenderer spriteRenderer;

        protected void Init(RESOURCE t)
        {
            type = t;
            tag = "Resource";
            if (spritesResources[type] == null)
                spritesResources[type] = Resources.LoadAll<Sprite>(resourcesPath[type]);

            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            spriteRenderer.sprite = spritesResources[type][Random.Range(0, spritesResources[type].Length)];

            BillBoardingManager.AddSpriteTransform(spriteRenderer.transform);
        }

    }
}