using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float ForcePower = 1f;
    public float TurnSpeed = 5f;

    private Rigidbody FormationRigidbody;

    public Transform target;
    private int wavepointIndex = 0;

    void Start()
    {
        FormationRigidbody = gameObject.GetComponent<Rigidbody>();
        if (target == null)
            target = Waypoints.WayPoints[wavepointIndex];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * TurnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        FormationRigidbody.velocity = transform.forward * ForcePower;

        //if (Vector3.Distance(transform.position, target.position) <= 0.2f)
           // GetNextWaypoint();
    }

    void GetNextWaypoint()
    {
        Debug.Log($"Next waypoint: {wavepointIndex}");

        if (wavepointIndex >= Waypoints.WayPoints.Length)
            wavepointIndex = 0;

        target = Waypoints.WayPoints[wavepointIndex++];
    }
}
