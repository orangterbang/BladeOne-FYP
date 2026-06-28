using UnityEngine;
using System.Collections;

public class CriticalStaggerState : StateMachine
{
    ActorContext actor;

    private Coroutine activeCoroutine;
    
    public CriticalStaggerState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.animator.GetComponent<Animator>().Play("Empty", AnimatorController.ACTION_LAYER);

        activeCoroutine = actor.coroutineRun.Run(CriticalStaggerCoroutine());
    }

    protected override void OnExit()
    {
        if (!actor.actorCombatData.isStunned)
        {
            actor.actorCombatData.ResetFlags();
        }
        
        if(activeCoroutine != null)
        {
            actor.coroutineRun.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }

    private IEnumerator CriticalStaggerCoroutine()
    {
        
        actor.animator.SetActionAnimation(ActionEvent.OnHitReceived, direction);
        actor.animator.PlayAnim(AnimatorController.REACTION_LAYER);

        Debug.Log("Crit");
        actor.soundEffects.PlayCriticalHitAudio();

        yield return null;

        yield return new WaitUntil(() => {
            
            var currentClipFinished = actor.animator.AnimationClipFinished(AnimatorController.REACTION_LAYER);

            return currentClipFinished;
        });

        actor.actorCombatData.ActorHasRecovered();
        SendTrigger(ActionEvent.HitStunRecovered);
    }
}
