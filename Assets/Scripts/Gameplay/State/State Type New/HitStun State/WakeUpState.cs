using UnityEngine;

public class WakeUpState : StateMachine
{
    ActorContext actor;
    
    public WakeUpState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.actorCombatData.ActorHasRecovered();

        if(!actor.actorCombatData.isStunned)
        {
            actor.animator.EnableReaction();
        }

        SendTrigger(ActionEvent.HitStunRecovered);
    }
}
