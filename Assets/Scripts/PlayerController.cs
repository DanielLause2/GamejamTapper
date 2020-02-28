using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float MovementSpeed;


    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float v = Joystick.Vertical;
        float h = Joystick.Horizontal;

        Vector3 translate = (new Vector3(h, v, 0) * Time.deltaTime) * MovementSpeed;
        transform.Translate(translate);
    }
}
