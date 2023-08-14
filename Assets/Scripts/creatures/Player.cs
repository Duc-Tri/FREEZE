using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace IHateWinter
{
    public class Player : MonoBehaviour
    {
        public enum PLAYER_STATE : byte { NONE = 0, DEAD, WANDERING, GO_TO_FISHING, FISHING }
        public PLAYER_STATE playerState;

        public const float DISTANCE_TO_HARVEST = 2.5f;
        public const float DISTANCE_TO_MOVE_HARVEST = 20f;

        public const float COOLING_TEMPERATURE = 30;
        public const float LOWEST_BODY_TEMPERATURE_BEARABLE = 9;

        public const float HEATING_TEMPERATURE = 30;
        public const float HIGHEST_BODY_TEMPERATURE_BEARABLE = 50;

        [SerializeField][Range(0.01f, 10f)] private float SPEED_FIRE_WARM = 10f;
        [SerializeField][Range(0.01f, 5f)] private float COOLING_FACTOR = 0.2f;
        [SerializeField] private HoveringTextMeshPro hoveringTMP;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;
        private NavMeshAgent agent;

        [SerializeField] private float fireWarmEffect; // per second
        private float bodyTemperature;  // in celsius
        //private bool alive;
        private Vector3 targetPosition;
        private AResource resourceToReach; // distance resource

        public static Action<float> OnBodyTemperatureChange;
        public static Action OnPlayerStart;
        public static Action OnPlayerDead;

        static NavMeshHit hit;

        // fishing ================================================================================
        [SerializeField] private Transform fishingLookAt;
        Vector3 fishingSpot;
        private AmountBar fishingAmountBar;
        [SerializeField][Range(0.1f, 99f)] private float FISHING_DURATION; // in seconds
        [SerializeField] private GameObject fishPrefab;
        [SerializeField][Range(0, 1)] private float ROD_BREAKING_CHANCE = 0.7f; // 0f..1f use Random.value

        private void Awake()
        {
            //alive = true;
            playerState = PLAYER_STATE.WANDERING;
            bodyTemperature = 37;
            targetPosition = Vector3.down;

            spriteTransform = spriteRenderer.transform;

            if (NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas))
                transform.position = hit.position;

            fishingLookAt.gameObject.SetActive(false);
            fishingAmountBar = GetComponentInChildren<AmountBar>();
            fishingAmountBar.gameObject.SetActive(false);
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
            //Debug.Log("MoveAgent ►►► " + pos);

            targetPosition = pos;
            if (agent != null)
            {
                agent.SetDestination(pos);
                agent.isStopped = false;
            }

            if (playerState == PLAYER_STATE.GO_TO_FISHING || playerState == PLAYER_STATE.FISHING)
            {
                fishingAmountBar.gameObject.SetActive(false);
                fishingLookAt.gameObject.SetActive(false);
            }

            playerState = PLAYER_STATE.WANDERING;
        }

        internal void ActOnResource(AResource resource)
        {
            if (Commons.NearEnoughXZ(transform.position, resource.transform.position, DISTANCE_TO_HARVEST))
            {
                Debug.Log("ActOnResource NEAR --- " + resource.name);
                if (resource is IHarvestable i && Inventory.Instance.TryAddResource(resource))
                    PoolerGameobjects.Instance.SaveToPool(resource.gameObject);
            }
            else if (Commons.NearEnoughXZ(transform.position, resource.transform.position, DISTANCE_TO_MOVE_HARVEST))
            {
                Debug.Log("ActOnResource FAR === " + resource.name);
                resourceToReach = resource;
                MoveAgent(resource.transform.position);
            }
        }

        private void Update()
        {
            if (playerState == PLAYER_STATE.DEAD) return;

            if (Mathf.Abs(bodyTemperature) < LOWEST_BODY_TEMPERATURE_BEARABLE)
            {
                // DEAD BY FROZEN ! ===============================================================
                spriteRenderer.color = Color.cyan;
                if (!GameManager.Instance.PlayerInvincible)
                {
                    //alive = false;
                    playerState = PLAYER_STATE.DEAD;
                    OnPlayerDead?.Invoke();
                }
                fishingAmountBar.gameObject.SetActive(false);
                fishingLookAt.gameObject.SetActive(false);
            }
            else
            {
                if (resourceToReach != null)
                {
                    // REACHING A RESOURCE ========================================================

                    //Debug.Log("resourceToReach:::: " + resourceToReach.name + " / " + resourceToReach.type);
                    if (Commons.NearEnoughXZ(transform.position, resourceToReach.transform.position, DISTANCE_TO_HARVEST))
                    {
                        ActOnResource(resourceToReach);
                        resourceToReach = null;
                    }
                }
                else if (targetPosition == Vector3.down)
                {
                    // ALREADY STOPPED, DOING BUSSINESS ! =========================================
                    if (playerState == PLAYER_STATE.FISHING)
                    {
                        fishingAmountBar.UpdateDeltaTime(Time.deltaTime);

                        // get a FISH !
                        if (fishingAmountBar.IsOver)
                        {
                            playerState = PLAYER_STATE.NONE;
                            fishingLookAt.gameObject.SetActive(false);
                            fishingAmountBar.gameObject.SetActive(false);
                            Inventory.Instance.TryAddResource(fishPrefab.GetComponent<AResource>());

                            // break the fishing rod ?
                            if (UnityEngine.Random.value < ROD_BREAKING_CHANCE)
                            {
                                Inventory.Instance.ConsumeTool(TOOL.FISHING_ROD);
                                //hoveringTMP.gameObject.SetActive(true);
                                hoveringTMP.WakeUp("broke my fishing rod!");
                            }
                        }
                    }
                }
                else if (targetPosition != Vector3.down && Commons.NearEnoughXZ(transform.position, targetPosition, 0.15f))
                {
                    // NOT HARVESTING RESOURCE, STOPPING AT DESTINATION ============================
                    targetPosition = Vector3.down;
                    agent.isStopped = true;

                    // arrive at fishint spot ?
                    if (playerState == PLAYER_STATE.GO_TO_FISHING)
                    {
                        playerState = PLAYER_STATE.FISHING;
                        fishingAmountBar.Init(FISHING_DURATION);
                        fishingAmountBar.gameObject.SetActive(true);
                    }
                }

                if (fishingLookAt.gameObject.activeSelf)
                {
                    fishingLookAt.LookAt(fishingSpot);
                    Vector3 rot = fishingLookAt.eulerAngles;
                    rot.x = 0;
                    fishingLookAt.eulerAngles = rot;
                }

                // TEMPERATURE ====================================================================
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

        internal void GoToFishing(Vector3 waterPoint)
        {
            if (Inventory.Instance.HasTool(TOOL.FISHING_ROD) && NavMesh.SamplePosition(waterPoint, out hit, 50, NavMesh.AllAreas))
            {
                playerState = PLAYER_STATE.GO_TO_FISHING;

                fishingSpot = waterPoint;
                fishingLookAt.gameObject.SetActive(true);

                // move with fishing rod !
                targetPosition = hit.position;
                agent.SetDestination(targetPosition);
                agent.isStopped = false;
                fishingAmountBar.gameObject.SetActive(false);
            }
        }

    }

}