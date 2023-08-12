using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace IHateWinter
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private bool cheatInvincible = false; // cheater !

        public const float MAX_DISTANCE_TO_HARVEST = 2.5f;

        public const float COOLING_TEMPERATURE = 20;
        public const float LOWEST_BODY_TEMPERATURE_BEARABLE = 10;

        public const float HEATING_TEMPERATURE = 30;
        public const float HIGHEST_BODY_TEMPERATURE_BEARABLE = 50;

        [SerializeField][Range(0.01f, 10f)] private float SPEED_FIRE_WARM = 10f;
        [SerializeField][Range(0.01f, 10f)] private float COOLING_FACTOR = 0.2f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;
        private NavMeshAgent agent;

        [SerializeField] private float warmEffect; // per second
        private float bodyTemperature;  // celsius
        private bool alive;

        public static Action<float> OnBodyTemperatureChange;
        public static Action OnPlayerStart;
        public static Action OnPlayerDead;

        private void Awake()
        {
            alive = true;
            bodyTemperature = 37;

            spriteTransform = spriteRenderer.transform;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        private void Start()
        {
            OnPlayerStart?.Invoke();

            agent = GetComponent<NavMeshAgent>();
            // after Awake of BillBoardingManager creations of objects
            BillBoardingManager.StartAddSpriteTransform(spriteTransform);

            // after the awake of Player2_5.OnBodyTemperatureChange += UpdatePlayerTemp
            OnBodyTemperatureChange?.Invoke(bodyTemperature);
        }

        public void MoveAgent(Vector3 pos)
        {
            agent?.SetDestination(pos);
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

            if (Mathf.Abs(bodyTemperature) < LOWEST_BODY_TEMPERATURE_BEARABLE)
            {
                if (!cheatInvincible)
                {
                    alive = false;
                    OnPlayerDead?.Invoke();
                }
            }

            float externTemp = TemperatureSystem.currentTemperature;
            if (externTemp < COOLING_TEMPERATURE)
            {
                if (warmEffect > 0)
                    bodyTemperature = MathF.Min(37, bodyTemperature + warmEffect * Time.deltaTime);
                else if (bodyTemperature > LOWEST_BODY_TEMPERATURE_BEARABLE)
                    bodyTemperature -= COOLING_FACTOR * Mathf.Sqrt(Mathf.Abs(externTemp - COOLING_TEMPERATURE)) * Time.deltaTime;
            }
            else
            {
                bodyTemperature += Mathf.Sqrt(externTemp - COOLING_TEMPERATURE) * Time.deltaTime;
            }

            OnBodyTemperatureChange?.Invoke(bodyTemperature);
        }

        internal void OutsideFireWarm()
        {
            warmEffect -= SPEED_FIRE_WARM;
        }

        internal void InsideFireWarm()
        {
            warmEffect += SPEED_FIRE_WARM;
        }
    }

}