using Unity.VisualScripting;
using UnityEngine;

public class ParryWindUpState : StateMachine
{
    ActorContext actor;
    
    public ParryWindUpState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.animator.SetActionAnimation(ActionInput.Parry, direction);
        
        actor.animator.PlayAnim();
        actor.actorCombatData.ActorHasFinishedParrying();

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
