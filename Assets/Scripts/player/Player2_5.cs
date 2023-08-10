using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IHateWinter
{

    public class Player2_5 : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;
        private Camera mainCamera;
        private Transform mainCameraTransform;
        private NavMeshAgent agent;

        private void Awake()
        {
            mainCamera = Camera.main;
            mainCameraTransform = Camera.main.transform;
            spriteTransform = spriteRenderer.transform;
            agent = GetComponent<NavMeshAgent>();

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas))
                transform.position = hit.position;

            BillBoardingManager.AddSpriteTransform(spriteTransform);
        }

        public void MoveAgent(Vector3 pos)
        {
            agent.SetDestination(pos);
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

        internal void ActOnResource(AResource resource)
        {
            if (Vector3.Distance(transform.position, resource.transform.position) < 2.5f && resource is IInventoryItem i)
            {
                if (Inventory.Instance.TryAdd(i))
                    PoolerGameobjects.Instance.SaveToPool(resource.gameObject);
            }
        }
    }

}