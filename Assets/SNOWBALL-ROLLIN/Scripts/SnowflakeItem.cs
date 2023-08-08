using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SnowflakeItem : AbstractItem
{
    [SerializeField]
    private static readonly Vector3 rotAngle = new Vector3(0, 500f, 0);

    private void Update()
    {
        transform.Rotate(rotAngle * Time.deltaTime, Space.Self);
    }

    public override void TouchedByPlayer()
    {
        base.TouchedByPlayer();
        ItemsManager.Instance.AddScore(1);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: {name} triggered by {collision.gameObject.name}");
    }

}
