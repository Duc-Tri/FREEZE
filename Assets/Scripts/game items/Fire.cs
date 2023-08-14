using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace IHateWinter
{
    public class Fire : MonoBehaviour
    {
        [SerializeField][Range(0f, 999f)] public float life; // in seconds

        [SerializeField] Transform imageBillboarding;
        AmountBar amountBar;

        float originalIntensity;
        public const float RANDOM_LIGHT_RANGE = 0.25f;

        Light firelight;
        Transform lightTransform;
        float lightY;

        public static Action OnPlayerInsideFireWarm;
        public static Action OnPlayerOutSideFireWarm;

        public static int NUMBER_FIRES_WARMING_PLAYER = 0;
        private Animator animator;
        private NavMeshObstacle obstacle;
        private bool alive;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            obstacle = (NavMeshObstacle)GetComponent<NavMeshObstacle>();
            amountBar = GetComponentInChildren<AmountBar>();
            amountBar.Init(life);
            firelight = GetComponentInChildren<Light>();
            lightTransform = firelight.transform;
            originalIntensity = firelight.GetComponent<Light>().intensity;
        }

        private void Start()
        {
            BillBoardingManager.StartAddSpriteTransform(imageBillboarding);
            alive = true;
        }

        void Update()
        {
            if (life <= 0 && !alive) return;

            life -= FACTOR * Time.deltaTime;
            amountBar.UpdateValue(life);

            if (life > 0)
            {
                //firelight.intensity = originalIntensity + Random.Range(-RANDOM_LIGHT_RANGE, RANDOM_LIGHT_RANGE);
                //firelight.intensity = Mathf.Max(originalIntensity - RANDOM_LIGHT_RANGE, Mathf.PingPong(Time.time, originalIntensity + RANDOM_LIGHT_RANGE));
                firelight.intensity = originalIntensity - RANDOM_LIGHT_RANGE + Mathf.PingPong(Time.time, 2f * RANDOM_LIGHT_RANGE);
                lightTransform.localPosition = new Vector3(Random.value * 0.2f - 0.1f,
                    Random.value * 0.3f + 0.1f,
                    Random.value * 0.2f - 0.1f);
            }
            else
            {
                if (playerNearBy) OnTriggerExit(playerCollider);

                animator.SetBool("fire_on", false);
                obstacle.enabled = false;
                firelight.enabled = false;
                alive = false;
            }
        }

        public void StopFire()
        {

        }

        float FACTOR = 1;
        bool playerNearBy = false;
        Collider playerCollider;
        private void OnTriggerEnter(Collider other)
        {
            if (life > 0)
            {
                if (other.CompareTag("Player"))
                {
                    Debug.Log("OnTriggerEnter Player");
                    playerNearBy = true;
                    playerCollider = other;
                    NUMBER_FIRES_WARMING_PLAYER++;
                    OnPlayerInsideFireWarm?.Invoke();
                }
                else if (other.CompareTag("Penguin"))
                {
                    Debug.Log("OnTriggerEnter Penguin");
                    Penguin p = other.GetComponent<Penguin>();
                    if (p.penguinState == Penguin.PENGUIN_STATE.FIRE_FIGHTER)
                    {
                        FACTOR += 2;
                    }
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (playerNearBy && other.CompareTag("Player"))
            {
                playerNearBy = false;
                Debug.Log("OnTriggerExit Player");
                NUMBER_FIRES_WARMING_PLAYER--;
                OnPlayerOutSideFireWarm?.Invoke();
            }

        }

    }

}
