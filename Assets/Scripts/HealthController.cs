using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    void Start()
    {
    }

    void Update()
    {

    }

    public void RemoveHealth(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            //Gameover;
        }
    }
}
