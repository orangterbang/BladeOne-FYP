using UnityEngine;

public class DodgeRecoveryState : StateMachine
{
    ActorContext actor;
    
    public DodgeRecoveryState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.actorCombatData.ActorHasFinishedDodging();
        actor.animator.DisableAction();

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }

    protected override void OnExit()
    {
        actor.actorCombatData.ResetFlags();
    }
}
