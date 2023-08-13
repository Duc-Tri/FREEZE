using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace IHateWinter
{
    public class Fire : MonoBehaviour
    {
        [SerializeField][Range(0f, 99f)] float duration; // in seconds

        [SerializeField] Transform billboard;

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

        private void Awake()
        {
            animator = GetComponent<Animator>();
            obstacle = (NavMeshObstacle)GetComponent<NavMeshObstacle>();
            firelight = GetComponentInChildren<Light>();
            lightTransform = firelight.transform;
            originalIntensity = firelight.GetComponent<Light>().intensity;
        }

        private void Start()
        {
            BillBoardingManager.StartAddSpriteTransform(billboard);
        }

        void Update()
        {
            if (duration <= 0) return;

            duration -= Time.deltaTime;

            if (duration > 0)
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
                animator.SetBool("fire_on", false);
                obstacle.enabled = false;
                firelight.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (duration > 0 && other.CompareTag("Player"))
            {
                Debug.Log("OnTriggerEnter Player");
                NUMBER_FIRES_WARMING_PLAYER++;
                OnPlayerInsideFireWarm?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (duration > 0 && other.CompareTag("Player"))
            {
                Debug.Log("OnTriggerExit Player");
                NUMBER_FIRES_WARMING_PLAYER--;
                OnPlayerOutSideFireWarm?.Invoke();
            }
        }

    }

}
