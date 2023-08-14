using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace IHateWinter
{
    public class Penguin : MonoBehaviour
    {
        public const float MAX_DISTANCE_TO_HARVEST = 2.5f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private HoveringTextMeshPro hoveringTMP;
        [SerializeField][Range(1f, 20f)] private float BASE_SPEED;

        private Vector3 targetPos;
        private Transform spriteTransform;
        private NavMeshAgent agent;

        private bool alive;

        public static Action<Penguin> OnPenguinInstantiated;
        public static Action<Fish> OnConsumeFish;

        private Vector3 RandomPos => new Vector3(Random.Range(-GameManager.Instance.halfObjectsMaxX, GameManager.Instance.halfObjectsMaxX), 0,
                Random.Range(-GameManager.Instance.halfObjectsMaxX, GameManager.Instance.halfObjectsMaxX));

        static NavMeshHit hit;
        private Fire fireToStop;
        public Fish fishToGet;

        public enum PENGUIN_STATE : byte { NONE, WANDERING, FIRE_FIGHTER, HAPPY_WITH_FISH, EATING_FISH }
        public PENGUIN_STATE penguinState;

        private void Awake()
        {
            alive = true;
            spriteTransform = spriteRenderer.transform;

            if (NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        static WaitForEndOfFrame waitFEOF = new WaitForEndOfFrame();
        IEnumerator Start()
        {
            agent = GetComponent<NavMeshAgent>(); // in Start
            Wandering();

            // after Awake of BillBoardingManager creations of objects
            BillBoardingManager.StartAddSpriteTransform(spriteTransform);

            // DIRTY SOLUTION !!!!!!!!!!!!!!!!!!!!!!!
            yield return waitFEOF;
            OnPenguinInstantiated?.Invoke(this);
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
            //if (path.status == NavMeshPathStatus.PathComplete)
            if (!alive) return;

            if (penguinState == PENGUIN_STATE.FIRE_FIGHTER)
            {
                if (Commons.NearEnoughXZ(fireToStop.transform.position, transform.position, 2f))
                {
                    fireToStop.life -= 20 * Time.deltaTime; // quickly kill the fire
                    //fireToStop.life = 0;
                }

                if (fireToStop.life <= 0)
                    Wandering();
            }
            else if (penguinState == PENGUIN_STATE.HAPPY_WITH_FISH)
            {
                if (Commons.NearEnoughXZ(fishToGet.transform.position, transform.position, 1.5f))
                {
                    fishToGet.gameObject.SetActive(false);
                    OnConsumeFish(fishToGet);
                    fishToGet = null;
                }
            }
            else if (Commons.NearEnoughXZ(targetPos, transform.position, 1.5f))
            {
                SetTargetPos();
            }
        }

        private void SetTargetPos()
        {
            targetPos = RandomPos;
            agent.SetDestination(targetPos);
            //agent.CalculatePath(targetPos, path);
        }

        public void Wandering()
        {
            penguinState = PENGUIN_STATE.WANDERING;
            hoveringTMP.Off();
            agent.speed = BASE_SPEED;
            SetTargetPos();
        }

        public void GoGetFish(Fish fish)
        {
            fishToGet = fish;
            penguinState = PENGUIN_STATE.HAPPY_WITH_FISH;
            agent.speed = 2 * BASE_SPEED;

            hoveringTMP.WakeUp("FISH ! FISH !");

            agent.isStopped = true;
            agent.SetDestination(fishToGet.transform.position);
            agent.isStopped = false;
        }

        internal void StopFire(Fire fire)
        {
            fireToStop = fire;
            penguinState = PENGUIN_STATE.FIRE_FIGHTER;
            agent.speed = 1.5f * BASE_SPEED;

            hoveringTMP.WakeUp("STOP THE FIRE !");

            agent.isStopped = true;
            agent.SetDestination(fire.transform.position);
            agent.isStopped = false;
        }

    }

}