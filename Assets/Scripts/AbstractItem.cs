using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractItem : MonoBehaviour
{
    public virtual void TouchedByPlayer()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {name} triggered by {other.name}");

        if (other.CompareTag("Player")) TouchedByPlayer();
    }

}
