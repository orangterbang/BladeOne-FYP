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

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }
}
