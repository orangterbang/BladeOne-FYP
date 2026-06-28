using UnityEngine;

public class DeadState : StateMachine
{
    ActorContext actor;

    public DeadState(ActorContext actor)
    {
        this.actor = actor;
    }

    protected override void OnEnter()
    {
        Debug.Log("Died");
        actor.actorCombatData.ActorHasDied();
        actor.animator.SetActionAnimation(ActionEvent.OnHealthZero);
        actor.animator.PlayAnim(AnimatorController.REACTION_LAYER);
    }
}
