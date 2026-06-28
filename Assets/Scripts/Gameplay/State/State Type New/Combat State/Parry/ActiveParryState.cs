using UnityEngine;
using System.Collections;

public class ActiveParryState : StateMachine
{
    ActorContext actor;

    private Coroutine activeCoroutine;
    
    public ActiveParryState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }
    
    protected override void OnEnter()
    {
        activeCoroutine = actor.coroutineRun.Run(ParrySequenceCoroutine());
    }

    protected override void OnExit()
    {
        if(activeCoroutine != null)
        {
            actor.coroutineRun.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }

    private IEnumerator ParrySequenceCoroutine()
    {
        yield return new WaitUntil(() => actor.animator.AnimationClipFinished(actor.animator.triggeredAnimationName));
        
        actor.animator.EnableAction();

        actor.parry.ExecuteParry(direction);

        yield return new WaitUntil(() => {
            var currentClipRunning = actor.animator.GetFullAnimationStateName("Recovery");

            return currentClipRunning != null;});

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
