using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace IHateWinter
{
    public class MouseManager : MonoBehaviour
    {
        [SerializeField][Range(1, 500)] private float distanceRaycast = 100f;

        private Camera mainCamera;
        public static Action<Vector3> OnClickOnWater;
        public static Action<Vector3> OnClickOnFloor;
        public static Action<AResource> OnHoverOnResource;
        public static Action<AResource> OnActOnResource;
        public static Action<float> OnMouseWheel;
        private Collider oldCollided;

        protected virtual void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
        }

        RaycastHit raycastHit;
        NavMeshHit navmeshHit;

        void Update()
        {
            if (GameManager.GameState == GAME_STATE.IN_GAME && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.mouseScrollDelta.y != 0)
                {
                    OnMouseWheel?.Invoke(Input.mouseScrollDelta.y);
                }

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, distanceRaycast))
                {
                    //Debug.Log("HIT: " + hit.collider.gameObject.tag);

                    if (Input.GetMouseButtonDown(0))
                    {
                        // Left mouse button clicked ==============================================
                        if (raycastHit.collider.CompareTag("Floor"))
                            OnClickOnFloor?.Invoke(raycastHit.point);
                        else if (NavMesh.SamplePosition(raycastHit.point, out navmeshHit, 9, NavMesh.AllAreas))
                            OnClickOnFloor?.Invoke(navmeshHit.position);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        // Right mouse button clicked =============================================
                        if (raycastHit.collider.CompareTag("Resource"))
                            OnActOnResource?.Invoke(raycastHit.collider.gameObject.GetComponent<AResource>());
                        else if (raycastHit.collider.CompareTag("Water"))
                            OnClickOnWater?.Invoke(raycastHit.point);
                    }
                    else if (oldCollided != raycastHit.collider)
                    {
                        // No mouse button clicked, just hovering =================================
                        if (raycastHit.collider.CompareTag("Resource"))
                            OnHoverOnResource?.Invoke(raycastHit.collider.gameObject.GetComponent<AResource>());
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