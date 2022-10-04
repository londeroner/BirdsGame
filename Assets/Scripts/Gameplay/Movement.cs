using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public FixedJoystick Joystick;
    private Rigidbody CubeRigidbody;

    public float ForcePower = 10f;
    public float TurnSpeed = 45f;

    private float _flyTime = 0f;

    private bool _stay = true;

    private void Start()
    {
        CubeRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(_stay);
        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
        {
            if (ForcePower * _flyTime < 6) _flyTime += Time.deltaTime;
            if (_stay && CubeRigidbody.velocity == Vector3.zero)
            {
                ChangeDir(100000f);
                _stay = false;
            }
            else
            {
                ChangeDir(TurnSpeed);
            }
            CubeRigidbody.velocity = transform.forward * (ForcePower * _flyTime);
        }
        else 
        {
            _stay = true;
            _flyTime = 0;
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
