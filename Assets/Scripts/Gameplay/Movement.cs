using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public FixedJoystick Joystick;

    public float ForcePower = 10f;
    public float TurnSpeed = 45f;

    private Rigidbody CubeRigidbody;
    private BirdsFormation birdsFormation;
    private FormationType lastFormation = FormationType.CollectFormation;

    private float _flyTime = 0f;

    private bool _stay = true;

    private void Start()
    {
        CubeRigidbody = gameObject.GetComponent<Rigidbody>();
        birdsFormation = gameObject.GetComponent<BirdsFormation>();
    }

    private void Update()
    {
        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
        {
            _flyTime += Time.deltaTime;

            ChangeDir(birdsFormation.FormationStats.GetTurnSpeed());

            // Если начали движения после остановки
            if (_stay && CubeRigidbody.velocity.magnitude < 0.3f)
                birdsFormation.ChangeFormationType(lastFormation);
            _stay = false;

            CubeRigidbody.velocity = transform.forward * 
                Mathf.Min((ForcePower * _flyTime)
                          , birdsFormation.FormationStats.GetMaxSpeed(birdsFormation.isAbilityActive));
        }
        //Если джойстик не двигается
        else 
        {
            //Если скорость упала ниже нужного значения со статичным джойстиком
            //то считаем что остановились
            if (CubeRigidbody.velocity.magnitude < 0.3f)
            {
                birdsFormation.ChangeFormationType(FormationType.NeutralFormation);
                _stay = true;
                _flyTime = 0;
            }
        }
    }

    private void ChangeDir(float turnSpeed)
    {
        Vector3 dir = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
