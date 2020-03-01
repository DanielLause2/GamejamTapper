using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Gold;
    public int Health;
    public Dictionary<string, int> BoughtUpgrades;
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
