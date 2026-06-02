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
        //actor.animator.SetActionAnimation(ActionEvent.OnStunhitRecovered);
        //actor.animator.PlayAnim();

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }
}
