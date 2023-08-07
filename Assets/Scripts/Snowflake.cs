using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Snowflake : MonoBehaviour
{
    [SerializeField]
    private static readonly Vector3 rotAngle = new Vector3(0, 500f, 0);

    private void Update()
    {
        transform.Rotate(rotAngle * Time.deltaTime, Space.World);
    }

    public void Triggered()
    {
        SnowflakesManager.Instance.AddScore(1);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {name} triggered by {other.name}");

        if (other.CompareTag("Player")) Triggered();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: {name} triggered by {collision.gameObject.name}");

    }
}
