using UnityEngine;
using System.Collections;

public class ActiveAttackState : StateMachine
{
    ActorContext actor;

    private Coroutine activeCoroutine;
    
    public ActiveAttackState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }
    
    protected override void OnEnter()
    {
    }

    protected override void OnUpdate()
    {
        if (actor.animator.CheckIfActionIsActive() && actor.actorCombatData.isAttacking)
        {
            
            actor.attack.ExecuteAttack(direction);
        }

        if (!actor.animator.CheckIfActionIsActive() && !actor.actorCombatData.isAttacking)
        {
            SendTrigger(ActionEvent.ChangeSubState);
        }
    }

    protected override void OnExit()
    {
        if(activeCoroutine != null)
        {
            actor.coroutineRun.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }

    private IEnumerator AttackSequenceCoroutine()
    {
        yield return new WaitUntil(() => {
            //var info = actor.animator.animator.GetCurrentAnimatorStateInfo(AnimatorController.ACTION_LAYER);
            //Debug.Log($"triggeredAnimationName={actor.animator.triggeredAnimationName}, currentState normalizedTime={info.normalizedTime}, isName={info.IsName(actor.animator.triggeredAnimationName)}, inTransition={actor.animator.animator.IsInTransition(AnimatorController.ACTION_LAYER)}");
            return actor.animator.AnimationClipFinished(actor.animator.triggeredAnimationName);
        });
        actor.animator.EnableAction();
        actor.attack.ExecuteAttack(direction);
        
        yield return new WaitUntil(() => {
            var currentClipRunning = actor.animator.GetFullAnimationStateName("Recovery");

            return currentClipRunning != null;});
            
        SendTrigger(ActionEvent.ChangeSubState);
    }
}
