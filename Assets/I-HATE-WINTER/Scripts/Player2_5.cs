using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_5 : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Transform spriteTransform;
    private Camera mainCamera;
    private Transform mainCameraTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraTransform = Camera.main.transform;
        spriteTransform = spriteRenderer.transform;
        LookAtCameraManager.AddSpriteTransform(spriteTransform);
    }

    private void Update()
    {
        /*
        spriteTransform.LookAt(mainCameraTransform.position);
        Vector3 eulerAngles = spriteTransform.eulerAngles;
        eulerAngles.x = 0;
        spriteTransform.eulerAngles = eulerAngles;
        */
    }

}
