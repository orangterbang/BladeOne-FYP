using UnityEngine;
using System.Collections;

public class StaggerState : StateMachine
{
    ActorContext actor;

    private Coroutine activeCoroutine;
    
    public StaggerState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        //actor.actorCombatData.ActorIsDamaged();

        actor.animator.GetComponent<Animator>().Play("Empty", AnimatorController.ACTION_LAYER);

        activeCoroutine = actor.coroutineRun.Run(StaggerCoroutine());
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

    private IEnumerator StaggerCoroutine()
    {
        
        actor.animator.SetActionAnimation(ActionEvent.OnHitReceived, direction);
        actor.animator.PlayAnim(AnimatorController.REACTION_LAYER);

        if (actor.actorCombatData.isStunned)
        {   
            VFXManager.PlayHit();
            actor.soundEffects.PlayParryHitAudio();
        }
        else
        {
            actor.soundEffects.PlayPunchAudio();
        }

        yield return null;

        yield return new WaitUntil(() => {
            
            var currentClipFinished = actor.animator.AnimationClipFinished(AnimatorController.REACTION_LAYER);

            return currentClipFinished;
        });
        //Debug.Log("IN stagger state before if else");

        if (actor.actorCombatData.isStunned)
        {
            activeCoroutine = null;
            
            SendTrigger(ActionEvent.OnStunned);
            yield break;
        }
        else
        {
            actor.actorCombatData.ActorHasRecovered();
            SendTrigger(ActionEvent.HitStunRecovered);
        }
    }
}
