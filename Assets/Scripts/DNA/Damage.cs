using UnityEngine;

public class Damage : MonoBehaviour
{
    //[SerializeField] private float baseDamage = 10f;
    //[SerializeField] private float damageMultiplier = 0.5f;
    
    private class DirectionDamageHandler
    {
        public Direction direction;
        public float damage;
    }

    [SerializeField] private DirectionDamageHandler[] directionDamages = new DirectionDamageHandler[4];


}
