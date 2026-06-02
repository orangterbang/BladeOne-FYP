using UnityEngine;
using System.Collections;

public class StunnedState : StateMachine
{
    ActorContext actor;
    
    public StunnedState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.coroutineRun.Run(StunnedCoroutine());
    }

    private IEnumerator StunnedCoroutine()
    {
        actor.animator.SetActionAnimation(ActionEvent.OnStunned);
        actor.animator.PlayAnim(AnimatorController.REACTION_LAYER);

        yield return new WaitForSeconds(actor.actorCombatData.GetStunDuration());

        actor.animator.EnableAction();

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
