using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float MovementSpeed;

    public bool IsMoving => Joystick.IsDraged;

    private void FixedUpdate()
    {
        float v = Joystick.Vertical;
        float h = Joystick.Horizontal;

        Vector3 translate = (new Vector3(h, v, 0) * Time.deltaTime) * MovementSpeed;
        transform.Translate(translate);
    }
}
