using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float ForcePower = 1f;
    public float TurnSpeed = 5f;
    public float AggressiveRadius = 12f;

    private Rigidbody FormationRigidbody;

    private GameObject player;
    private BirdsFormation playerFormation;
    private BirdsFormation selfFormation;

    [NonSerialized]
    public Transform target;

    public AIActionPattern Pattern = AIActionPattern.Aggressive;
    private bool HitPlayer = false;

    private IEnumerator afterHit;

    private float waypointNotReached = 0f;
    private float _distanceToPlayer => Vector3.Distance(player.transform.position, transform.position);
    private bool _playerHide => (playerFormation.Tree is not null 
                                && !(playerFormation.Tree == selfFormation.Tree)) 
                                || _distanceToPlayer > AggressiveRadius;

    void Start()
    {
        player = PlayerManager.instance.playerFormation;
        FormationRigidbody = gameObject.GetComponent<Rigidbody>();
        playerFormation = player.GetComponent<BirdsFormation>();
        selfFormation = gameObject.GetComponent<BirdsFormation>();
        target = player.transform;
        selfFormation.ChangeFormationType(FormationType.AttackFormation);
        afterHit = AfterHit();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= 1f)
            GetNextTarget(true);
        else GetNextTarget(false);

        Debug.Log(_distanceToPlayer);
        Debug.Log(_playerHide);

        MoveToTarget();
    }

    private void GetNextTarget(bool reach)
    {
        switch (Pattern)
        {
            case AIActionPattern.Aggressive:
                if (target == player.transform)
                {
                    if (selfFormation.canActivateAbility && _distanceToPlayer < 8f)
                    {
                        selfFormation.ActivateAbility();
                    }

                    waypointNotReached = 0f;
                    if (_playerHide)
                        target = GetRandomWaypoint();
                   else if (reach)
                   {
                        HitPlayer = true;
                        StartCoroutine(afterHit);
                        target = GetRandomWaypoint();
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

        FormationRigidbody.velocity = transform.forward * selfFormation.FormationStats.GetMaxSpeed(selfFormation.isAbilityActive);
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
