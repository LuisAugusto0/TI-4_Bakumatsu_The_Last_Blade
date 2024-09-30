using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTransform : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;
    public float maxDistance = 150f;

    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < maxDistance) 
        {
            SeekTarget();
        }
    }

    void SeekTarget()
    {
        // Move the object towards the target with an arc-like movement using Slerp
        transform.position = Vector3.Slerp(transform.position, target.position, Time.deltaTime * speed);

        RotateTowardsTarget();
    }

    // Entender esse codigo do chatgpt
    void RotateTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}