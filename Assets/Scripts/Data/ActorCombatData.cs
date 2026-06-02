using UnityEngine;

public class ActorCombatData : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private float stunDuration;
    //This flag variables are only allowed edit access to statemachines. The rest are only view access
    [Header("Flags")]
    public bool isParrying = false;
    public bool isDodging = false;
    public bool isStunned = false;
    public bool isDead = false;

    [Header("Current Action Direction")]
    public Direction currDirection;


    void Awake()
    {
        isParrying = false;
        isDodging = false;
        isStunned = false;
        isDead = false;
    }

    public float GetStunDuration() => stunDuration;
}
