using UnityEngine;
using UnityEngine.AI;

namespace IHateWinter
{
    public class MoveToClick : MonoBehaviour
    {

        private Camera mainCamera;
        protected NavMeshAgent agent;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 500))
                {
                    //Debug.Log("HIT: " + hit.collider.gameObject.tag);

                    if (!hit.collider.CompareTag("Obstacle"))
                    {
                        agent.SetDestination(hit.point);
                    }
                }
            }
        }

    }

}