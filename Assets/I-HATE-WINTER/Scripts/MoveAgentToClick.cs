using UnityEngine;
using UnityEngine.AI;

namespace IHateWinter
{
    public class MoveAgentToClick : MonoBehaviour
    {
        private Camera mainCamera;
        protected NavMeshAgent agent;

        private void OnEnable()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1000, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
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
                        if (hit.collider.CompareTag("Resource"))
                        {
                            AResource resource = hit.collider.GetComponent<AResource>();
                            Debug.Log($"Click on {resource.tag} / {resource.type} / {resource.name}");
                        }
                    }

                }
            }
        }

    }

}