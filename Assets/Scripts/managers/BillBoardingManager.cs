using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    // Make all billboards look at camera
    public class BillBoardingManager : MonoBehaviour
    {
        public static Camera mainCamera;
        private Transform mainCameraTransform;
        private static List<Transform> spritesTransform;
        private static List<Transform> textsTransform;

        private void Awake()
        {
            spritesTransform = new List<Transform>();
            textsTransform = new List<Transform>();
            mainCamera = Camera.main;
            mainCameraTransform = Camera.main.transform;
        }

        public static void AddSpriteTransform(Transform st)
        {
            spritesTransform.Add(st);
        }

        public static void AddTextTransform(Transform st)
        {
            textsTransform.Add(st);
        }

        private void Update()
        {
            Vector3 eulerAngles;

            foreach (Transform textTransform in textsTransform)
            {
                textTransform.LookAt(mainCameraTransform.position);
                eulerAngles = textTransform.eulerAngles;
                eulerAngles.y -= 180;
                eulerAngles.x = 0;
                textTransform.eulerAngles = eulerAngles;
            }

            foreach (Transform spriteTransform in spritesTransform)
            {
                spriteTransform.LookAt(mainCameraTransform.position);
                eulerAngles = spriteTransform.eulerAngles;
                eulerAngles.x = 0;
                spriteTransform.eulerAngles = eulerAngles;
            }
        }

        internal static void RemoveSpriteTransform(Transform transform)
        {
            spritesTransform.Remove(transform);
        }

    }

}