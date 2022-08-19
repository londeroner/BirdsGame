using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public FixedJoystick Joystick;
    private Rigidbody CubeRigidbody;

    public float ForcePower = 10f;
    public float TurnSpeed = 45f;

    private void Start()
    {
        CubeRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float speed = ForcePower * Time.deltaTime;

        Vector3 dir = new Vector3(0, 0, 0);

        CubeRigidbody.velocity = new Vector3(Joystick.Horizontal * ForcePower, CubeRigidbody.velocity.y, Joystick.Vertical * ForcePower);

        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
        {
            dir = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * TurnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
}
