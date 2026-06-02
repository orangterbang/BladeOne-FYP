using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    void Update()
    {
        if(health <= 0)
        {
            //Send character is dead event
        }
    }

    public void TakeDamage(float damage)
    {
        health = health - damage;
    }

    public bool IsHealthActive() => health > 0;

    public int GetCurrentHealth() => (int)health;
}
