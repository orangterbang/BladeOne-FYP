using System;
using UnityEngine;

public class DodgeWindUpState : StateMachine
{
    ActorContext actor;
    
    public DodgeWindUpState(ActorContext actorContext)
    {
        this.actor = actorContext;
    }

    protected override void OnEnter()
    {
        actor.animator.SetActionAnimation(ActionInput.Move);
        actor.animator.EnableAction();
        actor.animator.PlayAnim();
        actor.actorCombatData.ActorIsDodging();

        
        SendTrigger(ActionEvent.ChangeSubState);
    }
}
