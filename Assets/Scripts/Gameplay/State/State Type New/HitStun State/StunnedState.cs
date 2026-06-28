using UnityEngine;
using System.Collections;

public class StunnedState : StateMachine
{
    ActorContext actor;

    private Coroutine activeCoroutine;
    private float elapsed = 0;
    private float duration;
    
    public StunnedState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.actorCombatData.ActorIsStunned();
        //activeCoroutine = actor.coroutineRun.Run(StunnedCoroutine());
        actor.animator.SetActionAnimation(ActionEvent.OnStunned);
        actor.animator.PlayAnim(AnimatorController.REACTION_LAYER);

        actor.soundEffects.PlayStunnedAudio();

        elapsed = 0;
        duration = actor.actorCombatData.GetStunDuration();
    }

    protected override void OnUpdate()
    {
        if(elapsed >= duration)
        {
            SendTrigger(ActionEvent.ChangeSubState);
        }

        if (actor.actorCombatData.isDamaged)
        {
            SendTrigger(ActionEvent.OnCritHitReceived);
        }

        elapsed += Time.deltaTime;
    }

    protected override void OnExit()
    {
        if (activeCoroutine != null)
        {
            actor.coroutineRun.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }

    private IEnumerator StunnedCoroutine()
    {
        
        yield return new WaitForSeconds(actor.actorCombatData.GetStunDuration());
        

        
    }
}
