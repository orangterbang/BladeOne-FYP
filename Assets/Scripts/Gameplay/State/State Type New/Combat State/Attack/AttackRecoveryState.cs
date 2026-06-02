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
        actor.animator.DisableAction();

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }
}
