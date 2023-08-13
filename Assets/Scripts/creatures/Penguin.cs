using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace IHateWinter
{
    public class Penguin : MonoBehaviour
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
        private Vector3 RandomPos => new Vector3(Random.Range(-GameManager.Instance.halfObjectsMaxX, GameManager.Instance.halfObjectsMaxX), 0,
                Random.Range(-GameManager.Instance.halfObjectsMaxX, GameManager.Instance.halfObjectsMaxX));

        static NavMeshHit hit;
        public static Action<Penguin> OnPenguinInstantiated;

        public enum PENGUIN_STATE : byte { NONE, WANDERING, FIRE_FIGHTER, HAPPY_WITH_FISH, EATING_FISH }

        public PENGUIN_STATE state;

        private void Awake()
        {
            alive = true;
            state = PENGUIN_STATE.WANDERING;
            spriteTransform = spriteRenderer.transform;

            if (NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        static WaitForEndOfFrame waitFEOF = new WaitForEndOfFrame();
        IEnumerator Start()
        {
            agent = GetComponent<NavMeshAgent>(); // in Start
            agent.speed /= 2f;
            SetTargetPos();

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
            if (!alive) return;

            //if (path.status == NavMeshPathStatus.PathComplete)
            if (Commons.NearEnoughXZ(targetPos, transform.position, 1.5f))
                SetTargetPos();
        }

        private void SetTargetPos()
        {
            targetPos = RandomPos;
            agent.SetDestination(targetPos);
            //agent.CalculatePath(targetPos, path);
        }


        Fire fireToPutOut;
        internal void StopFire(Fire fire)
        {
            agent.isStopped = true;

            state = PENGUIN_STATE.FIRE_FIGHTER;
            fireToPutOut = fire;
            agent.speed *= 4;
            agent.SetDestination(fire.transform.position);

            agent.isStopped = false;
        }
    }

}