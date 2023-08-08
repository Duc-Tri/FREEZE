using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraManager : MonoBehaviour
{
    public static Camera mainCamera;
    private Transform mainCameraTransform;
    private static List<Transform> spritesTransform = new List<Transform>();

    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraTransform = Camera.main.transform;
    }

    public static void AddSpriteTransform(Transform st)
    {
        spritesTransform.Add(st);
    }

    private void Update()
    {
        Vector3 eulerAngles;
        foreach (Transform spriteTransform in spritesTransform)
        {
            spriteTransform.LookAt(mainCameraTransform.position);
            eulerAngles = spriteTransform.eulerAngles;
            eulerAngles.x = 0;
            spriteTransform.eulerAngles = eulerAngles;
        }
    }
}
