
using UnityEngine;

public class Tree2_5 : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    static Sprite[] trees;

    private void Awake()
    {
        trees = Resources.LoadAll<Sprite>("trees");

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = trees[Random.Range(0, trees.Length)];

        LookAtCameraManager.AddSpriteTransform(spriteRenderer.transform);
    }

}
