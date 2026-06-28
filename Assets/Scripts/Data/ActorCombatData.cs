using UnityEngine;

public class ActorCombatData : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private float stunDuration;
    //This flag variables are only allowed edit access to statemachines. The rest are only view access
    [Header("Flags")]
    public bool isAttacking = false;
    public bool isParrying = false;
    public bool isDodging = false;
    public bool isDamaged = false;
    public bool isStunned = false;
    public bool isDead = false;

    public bool canAttack = true;
    public bool canParry = true;
    public bool canDodge = true;

    [Header("Current Action Direction")]
    public Direction currDirection;


    void Awake()
    {
        
    }

    public float GetStunDuration() {
        return stunDuration;}

    public void ActorIsParrying()
    {
        isParrying = true;

        canParry = false;
    }

    public void ActorHasFinishedParrying()
    {
        if(isStunned) return;

        ResetFlags();
    }

    public void ActorIsAttacking()
    {
        isAttacking = true;

        canAttack = false;
    }

    public void ActorHasFinishedAttacking()
    {
        if(isStunned) return;

        ResetFlags();
    }

    public void ActorIsDodging()
    {
        isDodging = true;

        canDodge = false;
    }

    public void ActorHasFinishedDodging()
    {
        if(isStunned) return;
        
        ResetFlags();
    }

    public void ActorIsDamaged()
    {
        isAttacking = false;
        isParrying = false;
        isDodging = false;

        isDamaged = true;
        isStunned = false;

        canAttack = false;
        canParry = false;
        canDodge = false;
    }

    public void ActorIsStunned()
    {
        isAttacking = false;
        isParrying = false;
        isDodging = false;
        
        isDamaged = false;
        isStunned = true;
        
        canAttack = false;
        canParry = false;
        canDodge = false;
    }

    public void ActorHasDied()
    {
        isAttacking = false;
        isParrying = false;
        isDodging = false;

        isDamaged = false;
        isStunned = false;
        isDead = false;

        canAttack = false;
        canParry = false;
        canDodge = false;
    }

    public void ActorHasRecovered()
    {
        ResetFlags();
    }

    public void ResetFlags()
    {
        isAttacking = false;
        isParrying = false;
        isDodging = false;

        isDamaged = false;
        isStunned = false;
        isDead = false;

        canAttack = true;
        canParry = true;
        canDodge = true;
    }
}
