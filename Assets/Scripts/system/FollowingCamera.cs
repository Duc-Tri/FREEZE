using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace IHateWinter
{
    public class FollowingCamera : MonoBehaviour
    {
        [SerializeField][Range(1, 100)] private float maxDistanceFromPlayer = 20f;
        [SerializeField][Range(1, 100)] private float heightAbovePlayer = 10f;
        [SerializeField][Range(0.01f, 20)] private float speed = 2.5f;

        [SerializeField][Range(0.01f, 90)] private float ROTATION_ANGLE = 15f;

        //private Vector3 idealCamPos;

        private Player player;
        private Transform playerTransform;
        public static FollowingCamera Instance;
        private const float SPEED_WAITING_FACTOR = 5f;

        private float angleFromPlayer = 0;
        private Vector3 vectorFromPlayer;
        private Vector3 targetCameraPos;

        private void Awake()
        {
            Instance = this;
            angleFromPlayer = 0;
        }

        private void Start()
        {
            //player = GameObject.FindGameObjectWithTag("Player");
            player = GameManager.Player;
            playerTransform = player.transform;
            UpdateVectorFromPlayer();
            transform.position = playerTransform.position;// + Vector3.up + Vector3.back;
            //idealCamPos = transform.position;

            // to begin very slowly
            speed /= SPEED_WAITING_FACTOR;
        }

        private void Update()
        {
            transform.LookAt(playerTransform.position);
            targetCameraPos = playerTransform.position + vectorFromPlayer;

            if (!Commons.NearEnoughXYZ(transform.position, targetCameraPos, 0.02f))
                transform.position = Vector3.Lerp(transform.position, targetCameraPos, Mathf.Min(speed * Time.deltaTime, 0.1f));
        }

        private void Update2()
        {
            // Gets a vector that points from the player's position to the target's
            // var heading = target.position - player.position;

            transform.LookAt(playerTransform.position);

            float dCamPlayer = Vector3.Distance(playerTransform.position, transform.position);
            if (Mathf.Abs(maxDistanceFromPlayer - dCamPlayer) > 0.01f)
            {
                targetCameraPos = playerTransform.position + (transform.position - playerTransform.position).normalized * maxDistanceFromPlayer;
                targetCameraPos.y = playerTransform.position.y + heightAbovePlayer;
            }

            // the ideal position is NEVER reached
            if ((transform.position - targetCameraPos).sqrMagnitude > 0.01f)
                transform.position = Vector3.Lerp(transform.position, targetCameraPos, Mathf.Min(speed * Time.deltaTime, 0.1f));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, 0.2f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(targetCameraPos, 0.2f);
            Gizmos.DrawLine(transform.position, targetCameraPos);

            if (playerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(playerTransform.position, targetCameraPos);
            }
        }

        internal void PlayerActivated()
        {
            speed *= SPEED_WAITING_FACTOR;
        }

        public void OnMouseWheel(float delta)
        {
            //Debug.Log("OnMouseWheel " + delta);

            if (delta != 0)
            {
                angleFromPlayer += delta * ROTATION_ANGLE;
                UpdateVectorFromPlayer();
                //player.cameraFollowingTransform.RotateAround(playerTransform.position, Vector3.up, delta * 5f);
            }
        }

        private void UpdateVectorFromPlayer()
        {
            vectorFromPlayer = Vector3.one;
            vectorFromPlayer.x = Mathf.Cos(Mathf.Deg2Rad * angleFromPlayer);
            vectorFromPlayer.z = Mathf.Sin(Mathf.Deg2Rad * angleFromPlayer);
            vectorFromPlayer *= maxDistanceFromPlayer;
            vectorFromPlayer.y = heightAbovePlayer;

            transform.position = playerTransform.position + vectorFromPlayer;
        }

    }

}