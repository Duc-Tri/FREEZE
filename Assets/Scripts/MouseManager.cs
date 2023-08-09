using System;
using UnityEngine;
using UnityEngine.AI;

namespace IHateWinter
{
    public class MouseManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField][Range(1, 500)] private float distanceRaycast = 35f;

        public static Action<Vector3> OnClickOnFloor;
        public static Action<AResource> OnHoverOnResource;
        private Collider oldCollided;

        protected virtual void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
        }

        void Update()
        {
            if (GameManager.GameMode == GAME_MODE.IN_GAME)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distanceRaycast))
                {
                    //Debug.Log("HIT: " + hit.collider.gameObject.tag);

                    if (Input.GetMouseButtonDown(0))
                    {
                        // Left mouse button clicked ==============================================
                        if (hit.collider.CompareTag("Floor"))
                            OnClickOnFloor?.Invoke(hit.point);

                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        // Right mouse button clicked =============================================

                    }
                    else if (oldCollided != hit.collider)
                    {
                        // No mouse button clicked, just hovering =================================
                        if (hit.collider.CompareTag("Resource"))
                            OnHoverOnResource?.Invoke(hit.collider.gameObject.GetComponent<AResource>());
                        else
                            OnHoverOnResource?.Invoke(null);
                    }
                }
                else
                {
                    oldCollided = null;
                    OnHoverOnResource?.Invoke(null);
                }
            }

        }

    }
}