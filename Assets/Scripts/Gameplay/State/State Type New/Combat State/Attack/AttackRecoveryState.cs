using UnityEngine;

public class AttackRecoveryState : StateMachine
{
    ActorContext actor;
    public AttackRecoveryState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }
    
    protected override void OnEnter()
    {
        actor.actorCombatData.ActorHasFinishedAttacking();

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }

    protected override void OnExit()
    {
        actor.actorCombatData.ResetFlags();
    }
}
