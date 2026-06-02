using UnityEngine;

public class AttackWindUpState : StateMachine
{
    ActorContext actor;
    public AttackWindUpState(ActorContext actorContext) 
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.animator.SetActionAnimation(ActionInput.Attack, direction);
        
        actor.animator.PlayAnim();

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
