using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisterFrizz : MonoBehaviour
{
    [SerializeField] private float forceMulti = 10f;
    private Rigidbody rigibody;
    private Vector3 forceNormalized;
    private Camera mainCamera;
    private Transform cameraTransform;

    private SphereCollider sphereCollider;

    [SerializeField]
    [Range(0, 4f)]
    private float upScaleFactor = 0.5f;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
    }

    private void FixedUpdate()
    {
        forceNormalized = (cameraTransform.right * Input.GetAxis("Horizontal") +
            cameraTransform.forward * Input.GetAxis("Vertical")).normalized;

        if (forceNormalized.sqrMagnitude > 0.01f)
        {
            rigibody.AddForce(forceNormalized * forceMulti * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, forceNormalized);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {name} triggered by {other.name}");
        if (other.CompareTag("Snowflake"))
        {
            transform.Translate(Vector3.up * upScaleFactor);
            transform.localScale += Vector3.one * upScaleFactor;
            //sphereCollider.radius *= upScaleFactor;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: {name} triggered by {collision.gameObject.name}");
    }
}
