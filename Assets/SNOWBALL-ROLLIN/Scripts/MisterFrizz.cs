using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisterFrizz : MonoBehaviour
{
    [SerializeField] private Transform AimDirection;
    [SerializeField] private float forceMulti = 10f;

    private Rigidbody rigibody;
    private Vector3 forceNormalized;
    private Camera mainCamera;
    private Transform cameraTransform;

    private SphereCollider sphereCollider;

    [SerializeField]
    [Range(0, 4f)]
    private float upScaleFactor = 0.5f;

    private LineRenderer lineRenderer;


    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.posi

        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
    }

    Vector3 aimPos;
    private void Update()
    {
        aimPos = transform.position;
        aimPos.y -= (transform.localScale.y / 2f - 0.1f);
        AimDirection.position = aimPos;

        if (transform.position.y < -50)
        {
            transform.position = Vector3.zero + Vector3.up * 10;
            rigibody.velocity = rigibody.angularVelocity = Vector3.zero;
        }
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
        if (other.CompareTag("ItemSnowflake"))
        {
            transform.Translate(Vector3.up * upScaleFactor);
            transform.localScale += Vector3.one * upScaleFactor;
            AimDirection.localScale += Vector3.one * upScaleFactor;
            //sphereCollider.radius *= upScaleFactor;
        }
        else if (other.CompareTag("ItemFlame"))
        {
            transform.Translate(Vector3.up * upScaleFactor);
            transform.localScale -= Vector3.one * upScaleFactor;
            AimDirection.localScale -= Vector3.one * upScaleFactor;
            //sphereCollider.radius *= upScaleFactor;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: {name} triggered by {collision.gameObject.name}");
    }
}
