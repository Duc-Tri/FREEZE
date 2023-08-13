using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace IHateWinter
{
    public class Pinguin : MonoBehaviour
    {
        Vector3 targetPos;
        //NavMeshPath path;
        public const float MAX_DISTANCE_TO_HARVEST = 2.5f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;
        private NavMeshAgent agent;

        private bool alive;

        public static Action<float> OnBodyTemperatureChange;
        public static Action OnPlayerDead;
        private Vector3 RandomPos => new Vector3(Random.Range(-GameManager.Instance.halfMaxX, GameManager.Instance.halfMaxX), 0,
                Random.Range(-GameManager.Instance.halfMaxX, GameManager.Instance.halfMaxX));

        private void Awake()
        {
            alive = true;

            spriteTransform = spriteRenderer.transform;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); // in Start
            agent.speed /= 2f;
            SetTargetPos();

            // after Awake of BillBoardingManager creations of objects
            BillBoardingManager.StartAddSpriteTransform(spriteTransform);
        }

        internal void Act(AResource resource)
        {
            /*
            if (Commons.NearEnough(transform.position, resource.transform.position, MAX_DISTANCE_TO_HARVEST) && resource is IHarvestable i)
            {
            }
            */
        }

        private void Update()
        {
            if (!alive) return;

            //if (path.status == NavMeshPathStatus.PathComplete)
            if (Commons.NearEnough(targetPos, transform.position, 1.5f))
                SetTargetPos();
        }

        private void SetTargetPos()
        {
            targetPos = RandomPos;
            agent.SetDestination(targetPos);
            //agent.CalculatePath(targetPos, path);
        }

    }

}