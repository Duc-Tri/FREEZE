using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace IHateWinter
{

    public class Player2_5 : MonoBehaviour
    {
        public static float MAX_DISTANCE_TO_HARVEST = 2.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;
        private NavMeshAgent agent;

        private float bodyTemperature;  // celsius
        private bool alive;

        public static Action<float> OnBodyTemperatureChange;
        public static Action OnPlayerDead;

        private void Awake()
        {
            alive = true;
            bodyTemperature = 37;

            spriteTransform = spriteRenderer.transform;

            agent = GetComponent<NavMeshAgent>();
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        private void Start()
        {
            // after Awake of BillBoardingManager creations of objects
            BillBoardingManager.AddSpriteTransform(spriteTransform);

            // after the awake of Player2_5.OnBodyTemperatureChange += UpdatePlayerTemp
            OnBodyTemperatureChange?.Invoke(bodyTemperature);
        }

        public void MoveAgent(Vector3 pos)
        {
            agent.SetDestination(pos);
        }

        internal void ActOnResource(AResource resource)
        {
            if (Vector3.Distance(transform.position, resource.transform.position) < MAX_DISTANCE_TO_HARVEST && resource is IHarvestable i)
            {
                if (Inventory.Instance.TryAdd(resource))
                    PoolerGameobjects.Instance.SaveToPool(resource.gameObject);
            }
        }

        private void Update()
        {
            if (!alive) return;

            if (Mathf.Abs(bodyTemperature) < 1f)
            {
                alive = false;
                OnPlayerDead?.Invoke();
            }

            float temp = TemperatureSystem.currentTemperature;
            if (temp < 20)
            {
                bodyTemperature -= Mathf.Sqrt(Mathf.Abs(temp - 20)) * Time.deltaTime;
            }
            else
            {
                bodyTemperature += Mathf.Sqrt(temp - 20) * Time.deltaTime;
            }

            OnBodyTemperatureChange?.Invoke(bodyTemperature);
        }

    }

}