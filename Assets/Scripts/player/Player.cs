using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace IHateWinter
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private bool cheatInvincible = false; // cheater !

        public const float DISTANCE_TO_HARVEST = 2.5f;
        public const float DISTANCE_TO_MOVE_HARVEST = 20f;

        public const float COOLING_TEMPERATURE = 30;
        public const float LOWEST_BODY_TEMPERATURE_BEARABLE = 9;

        public const float HEATING_TEMPERATURE = 30;
        public const float HIGHEST_BODY_TEMPERATURE_BEARABLE = 50;

        [SerializeField][Range(0.01f, 10f)] private float SPEED_FIRE_WARM = 10f;
        [SerializeField][Range(0.01f, 10f)] private float COOLING_FACTOR = 0.2f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;
        private NavMeshAgent agent;

        [SerializeField] private float fireWarmEffect; // per second
        private float bodyTemperature;  // celsius
        private bool alive;
        private Vector3 targetPosition;

        public static Action<float> OnBodyTemperatureChange;
        public static Action OnPlayerStart;
        public static Action OnPlayerDead;
        private AResource resourceToReach;



        private void Awake()
        {
            alive = true;
            bodyTemperature = 37;
            targetPosition = Vector3.down;

            spriteTransform = spriteRenderer.transform;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        private void Start()
        {
            OnPlayerStart?.Invoke();

            agent = GetComponent<NavMeshAgent>();
            agent.isStopped = true;

            // after Awake of BillBoardingManager creations of objects
            BillBoardingManager.StartAddSpriteTransform(spriteTransform);

            // after the awake of Player2_5.OnBodyTemperatureChange += UpdatePlayerTemp
            OnBodyTemperatureChange?.Invoke(bodyTemperature);
        }

        public void MoveAgent(Vector3 pos)
        {
            Debug.Log("MoveAgent ►►► " + pos);

            targetPosition = pos;
            if (agent != null)
            {
                agent.SetDestination(pos);
                agent.isStopped = false;
            }
        }

        internal void ActOnResource(AResource resource)
        {
            if (Commons.NearEnoughXZ(transform.position, resource.transform.position, DISTANCE_TO_HARVEST))
            {
                Debug.Log("ActOnResource NEAR --- " + resource.name);
                if (resource is IHarvestable i && Inventory.Instance.TryAdd(resource))
                    PoolerGameobjects.Instance.SaveToPool(resource.gameObject);
            }
            else if (Commons.NearEnoughXZ(transform.position, resource.transform.position, DISTANCE_TO_MOVE_HARVEST))
            {
                Debug.Log("ActOnResource FAR === " + resource.name);
                MoveAgent(resource.transform.position);
                resourceToReach = resource;
            }
        }

        private void Update()
        {
            if (!alive) return;

            if (Mathf.Abs(bodyTemperature) < LOWEST_BODY_TEMPERATURE_BEARABLE)
            {
                if (!cheatInvincible)
                {
                    alive = false;
                    OnPlayerDead?.Invoke();
                }
            }
            else
            {
                if (resourceToReach != null)
                {
                    if (Commons.NearEnoughXZ(transform.position, resourceToReach.transform.position, DISTANCE_TO_HARVEST))
                    {
                        ActOnResource(resourceToReach);
                        resourceToReach = null;
                    }
                }
                else if (targetPosition != Vector3.down && Commons.NearEnoughXZ(transform.position, targetPosition, 0.2f))
                {
                    targetPosition = Vector3.down;
                    agent.isStopped = true;
                }

                float externTemp = TemperatureSystem.currentTemperature;
                if (fireWarmEffect > 0)
                    // NEAR A FIRE
                    bodyTemperature = MathF.Min(37, bodyTemperature + fireWarmEffect * Time.deltaTime);
                else if (externTemp < COOLING_TEMPERATURE)
                    // BODY TEMP DROP OFF
                    bodyTemperature -= COOLING_FACTOR * Mathf.Sqrt(Mathf.Abs(externTemp - COOLING_TEMPERATURE)) * Time.deltaTime;
                else
                    // BODY TEMP INCREASE ONLY BECAUSE OF ENVIRONMENT, NEVER HAPPENS IN THIS GAME !
                    bodyTemperature += Mathf.Sqrt(externTemp - COOLING_TEMPERATURE) * Time.deltaTime;

                OnBodyTemperatureChange?.Invoke(bodyTemperature);
            }
        }

        internal void OutsideFireWarm()
        {
            fireWarmEffect -= SPEED_FIRE_WARM;
        }

        internal void InsideFireWarm()
        {
            fireWarmEffect += SPEED_FIRE_WARM;
        }

    }

}