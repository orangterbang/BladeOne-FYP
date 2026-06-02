using UnityEngine;
using System.Collections;

public class ActiveAttackState : StateMachine
{
    ActorContext actor;
    
    public ActiveAttackState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }
    
    protected override void OnEnter()
    {
        actor.coroutineRun.Run(AttackSequenceCoroutine());
    }

    private IEnumerator AttackSequenceCoroutine()
    {
        actor.animator.EnableAction();
        yield return null;

        actor.attack.ExecuteAttack(direction);

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
