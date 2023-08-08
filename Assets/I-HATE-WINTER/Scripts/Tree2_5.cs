
using UnityEngine;


namespace IHateWinter
{

    public class Tree2_5 : AResource
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        static Sprite[] trees;

        private void Awake()
        {
            type = RESOURCE.TREE;
            trees = Resources.LoadAll<Sprite>("trees");

            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            spriteRenderer.sprite = trees[Random.Range(0, trees.Length)];

            LookAtCameraManager.AddSpriteTransform(spriteRenderer.transform);
        }

    }
}