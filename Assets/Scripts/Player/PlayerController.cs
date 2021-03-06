﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AttackController attackController;
    private PlayerMovementController playerMovementController;

    private void Awake()
    {
        attackController = transform.GetComponent<AttackController>();
        playerMovementController = transform.GetComponent<PlayerMovementController>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attackController.RangeActive = !playerMovementController.IsMoving;
        attackController.MeeleActive = playerMovementController.IsMoving;
    }
}
