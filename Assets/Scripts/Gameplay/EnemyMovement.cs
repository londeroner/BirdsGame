using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float ForcePower = 1f;
    public float TurnSpeed = 5f;

    private Rigidbody FormationRigidbody;

    public GameObject player;
    private BirdsFormation playerFormation;
    private BirdsFormation selfFormation;

    [NonSerialized]
    public Transform target;

    public AIActionPattern Pattern = AIActionPattern.Aggressive;
    private bool HitPlayer = false;

    private IEnumerator afterHit;

    private float waypointNotReached = 0f;

    private bool _playerHide => playerFormation.Tree is not null && !(playerFormation.Tree == selfFormation.Tree);

    void Start()
    {
        FormationRigidbody = gameObject.GetComponent<Rigidbody>();
        playerFormation = player.GetComponent<BirdsFormation>();
        selfFormation = gameObject.GetComponent<BirdsFormation>();
        target = player.transform;
        selfFormation.FormationStats.FormationType = FormationType.AttackFormation;
        afterHit = AfterHit();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= 1f)
            GetNextTarget(true);
        else GetNextTarget(false);

        MoveToTarget();
    }

    private void GetNextTarget(bool reach)
    {
        switch (Pattern)
        {
            case AIActionPattern.Aggressive:
                if (target == player.transform)
                {
                    waypointNotReached = 0f;
                    if (_playerHide)
                        target = GetClothestWaypoint();
                   else if (reach)
                   {
                        HitPlayer = true;
                        StartCoroutine(afterHit);
                        target = GetClothestWaypoint();
                   }
                }
                else if (Waypoints.WayPoints.Contains(target))
                {
                    waypointNotReached += Time.deltaTime;
                    if (reach)
                    {
                        if (!_playerHide)
                            target = player.transform;
                        else target = GetRandomWaypoint();
                    }
                    else if (waypointNotReached > 10f)
                    {
                        waypointNotReached = 0f;
                        target = GetRandomWaypoint();
                    }
                    else if (!_playerHide && !HitPlayer)
                        target = player.transform;
                }
                else target = player.transform;
                break;
            case AIActionPattern.Collection:
                break;
        }
    }

    private void MoveToTarget()
    {
        Vector3 dir = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * selfFormation.FormationStats.GetTurnSpeed()).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        FormationRigidbody.velocity = transform.forward * selfFormation.FormationStats.GetMaxSpeed(false);
    }

    private Transform GetClothestWaypoint()
    {
        return Waypoints.WayPoints.OrderBy(x => Vector3.Distance(transform.position, x.position)).FirstOrDefault();
    }

    private Transform GetRandomWaypoint()
    {
        return Waypoints.WayPoints[new System.Random().Next(0, Waypoints.WayPoints.Length)];
    }

    private IEnumerator AfterHit()
    {
        yield return new WaitForSeconds(2);
        HitPlayer = false;
        afterHit = AfterHit();
    }
}
