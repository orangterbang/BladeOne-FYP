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

        if (currCombatData.isDodging)
        {
            //make sure to check parry direction to & compare with attack direction
            Debug.Log("Dodged");
            defensiveActionPerformed = ActionEvent.DodgePressed;
            return false;
        }

        if (IsParryable(direction, parryActionDirection))
        {
            Debug.Log("Parried");//Send Event
            defensiveActionPerformed = ActionEvent.ParryPressed;
            currCombatData.isParrying = false;
            return false;
        }

        //take damage
        health.TakeDamage(damage);
        OnHitInput?.Invoke(ActionEvent.OnHitReceived, direction);

        return true;
    }

    public ActionEvent DefensiveActionPerformed() => defensiveActionPerformed;

    public void ParryPerformed(Direction direction)
    {
        currCombatData.isParrying = true;
        parryActionDirection = direction;
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
        OnHitInput?.Invoke(ActionEvent.OnStunned, currCombatData.currDirection);
        currCombatData.isStunned = true;
    }
}
