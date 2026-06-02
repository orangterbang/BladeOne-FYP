using UnityEngine;

public class StaggerState : StateMachine
{
    ActorContext actor;
    
    public StaggerState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.animator.SetActionAnimation(ActionEvent.OnHitReceived);
        actor.animator.PlayAnim(AnimatorController.REACTION_LAYER);

        //Check if the player is parried/stunned then change to stunned state

        if (actor.actorCombatData.isStunned)
        {
            SendTrigger(ActionEvent.ChangeSubState);
        }

        SendTrigger(ActionEvent.CombatSequenceComplete);
    }
}
