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
        actor.animator.DisableAction();

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }
}
