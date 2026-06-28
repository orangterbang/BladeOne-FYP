using UnityEngine;

public class ParryRecoveryState : StateMachine
{
    ActorContext actor;
    
    public ParryRecoveryState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }
    
    protected override void OnEnter()
    {
        actor.actorCombatData.ActorHasFinishedParrying();
        actor.parry.ParryExecuted();
        
        SendTrigger(ActionEvent.CombatSequenceComplete);
    }

    protected override void OnExit()
    {
        actor.actorCombatData.ResetFlags();
    }
}
