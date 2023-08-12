using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    public class FollowingCamera : MonoBehaviour
    {
        [SerializeField][Range(1, 100)] private float maxDistanceFromPlayer = 20f;
        [SerializeField][Range(1, 100)] private float heightAbovePlayer = 10f;
        [SerializeField][Range(0.01f, 20)] private float speed = 2.5f;

        private Vector3 idealCamPos;

        private Player player;
        private Transform playerTransform;
        public static FollowingCamera Instance;
        private const float SPEED_WAITING_FACTOR = 100f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //player = GameObject.FindGameObjectWithTag("Player");
            player = GameManager.Player;
            playerTransform = player.transform;
            transform.position = playerTransform.position + Vector3.up + Vector3.back;
            idealCamPos = transform.position;
            speed /= SPEED_WAITING_FACTOR;
        }

        private void Update()
        {
            // Gets a vector that points from the player's position to the target's
            // var heading = target.position - player.position;

            transform.LookAt(playerTransform.position);

            float dCamPlayer = Vector3.Distance(playerTransform.position, transform.position);
            if (Mathf.Abs(maxDistanceFromPlayer - dCamPlayer) > 0.01f)
            {
                idealCamPos = playerTransform.position + (transform.position - playerTransform.position).normalized * maxDistanceFromPlayer;
                idealCamPos.y = playerTransform.position.y + heightAbovePlayer;
            }

            // the ideal position is NEVER reached
            if ((transform.position - idealCamPos).sqrMagnitude > 0.01f)
                transform.position = Vector3.Lerp(transform.position, idealCamPos, Mathf.Min(speed * Time.deltaTime, 0.1f));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, 0.2f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(idealCamPos, 0.2f);
            Gizmos.DrawLine(transform.position, idealCamPos);

            if (playerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(playerTransform.position, idealCamPos);
            }
        }

        internal void PlayerActivated()
        {
            speed *= SPEED_WAITING_FACTOR;
        }
    }
}