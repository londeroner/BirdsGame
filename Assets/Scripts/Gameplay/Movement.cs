using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public FixedJoystick Joystick;
    private Rigidbody CubeRigidbody;

    public float ForcePower = 10f;
    public float TurnSpeed = 45f;

    private bool _stay = true;

    private void Start()
    {
        CubeRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float speed = ForcePower * Time.deltaTime;

        CubeRigidbody.velocity = new Vector3(Joystick.Horizontal * ForcePower, CubeRigidbody.velocity.y, Joystick.Vertical * ForcePower);

        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
        {
            if (_stay)
            {
                ChangeDir(100000f);
                _stay = false;
            }
            else
            {
                ChangeDir(TurnSpeed);
            }
        }
        else _stay = true;
    }

    private void ChangeDir(float turnSpeed)
    {
        Vector3 dir = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
