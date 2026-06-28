using System;
using UnityEngine;

public class CombatReceiver : MonoBehaviour
{
    private ActorCombatData currCombatData;
    private Health health;
    private Direction parryActionDirection;
    private ActionEvent defensiveActionPerformed;

    public event Action<ActionEvent, Direction> OnHitInput;

    void Start()
    {
        currCombatData = GetComponent<ActorCombatData>();
        health = GetComponent<Health>();
    }

    public bool ReceiveHit(float damage, Direction direction)
    {
        if(currCombatData == null) return false;
        if(currCombatData.isDamaged && !currCombatData.isStunned) return false;

        if (currCombatData.isDodging)
        {
            //make sure to check parry direction to & compare with attack direction
            defensiveActionPerformed = ActionEvent.DodgePressed;
            return false;
        }

        if (IsParryable(direction, parryActionDirection))
        {
            defensiveActionPerformed = ActionEvent.ParryPressed;
            currCombatData.ActorHasFinishedParrying();
            return false;
        }

        if (!currCombatData.isDamaged && currCombatData.isStunned)
        {
            currCombatData.ActorIsDamaged();
            health.TakeDamage(damage + (damage * 4f), direction);
            OnHitInput?.Invoke(ActionEvent.OnHitReceived, direction);
            return true;
        }

        //take damage
        currCombatData.ActorIsDamaged();
        health.TakeDamage(damage, direction);
        OnHitInput?.Invoke(ActionEvent.OnHitReceived, direction);

        return true;
    }

    public ActionEvent DefensiveActionPerformed() => defensiveActionPerformed;

    public void ParryPerformed(Direction direction)
    {
        currCombatData.ActorIsParrying();
        parryActionDirection = direction;
    }

    public void ParryFinished()
    {
        currCombatData.ActorHasFinishedParrying();
    }

    public void AttackFinished()
    {
        currCombatData.ActorHasFinishedAttacking();
    }

    private bool IsParryable(Direction attackDirection, Direction parryDirection)
    {
        if(!currCombatData.isParrying) return false;

        var parryable = false;

        switch (parryDirection)
        {
            case Direction.Up when attackDirection == Direction.Up:
                parryable = true;
                break;
            case Direction.Down when attackDirection == Direction.Down:
                parryable = true;
                break;
            case Direction.Left when attackDirection == Direction.Right:
                parryable = true;
                break;
            case Direction.Right when attackDirection == Direction.Left:
                parryable = true;
                break;
        }
        return parryable;
    }

    public void ActorParried()
    {
        currCombatData.ActorIsStunned();
        OnHitInput?.Invoke(ActionEvent.OnStunned, currCombatData.currDirection);
    }
}
