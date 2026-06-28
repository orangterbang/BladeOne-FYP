using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    private int maxHealth;

    public event Action<ActionEvent> OnHealthZero;

    [System.Serializable]
    public class DirectionalDamageReceiver
    {
        public Direction direction;
        [Range(0f,1f)]
        public float damageMultiplier;
    }

    [SerializeField] private DirectionalDamageReceiver[] directionDamage = new DirectionalDamageReceiver[4];

    Dictionary<Direction, DirectionalDamageReceiver> damageMap = new Dictionary<Direction, DirectionalDamageReceiver>();

    void Awake()
    {
        maxHealth = (int)health;
    }

    void Start()
    {
        foreach (var receiver in directionDamage)
        {
            if(receiver != null && !damageMap.ContainsKey(receiver.direction))
            {
                damageMap.Add(receiver.direction, receiver);
            }
        }
    }
    void Update()
    {
        if(!IsHealthActive())
        {
            //Send character is dead event
            OnHealthZero?.Invoke(ActionEvent.OnHealthZero);
        }
    }

    public void TakeDamage(float damage)
    {
        health = health - damage;
    }

    public void TakeDamage(float damage, Direction direction)
    {
        if(damageMap.TryGetValue(direction, out DirectionalDamageReceiver receiver))
        {
            health = health - (damage + damage * receiver.damageMultiplier);
        }
        else
        {
            TakeDamage(damage);
        }
    }

    public bool IsHealthActive() => health > 0;

    public int GetCurrentHealth() => (int)health;
    public int GetMaxHealth() => maxHealth;
}
