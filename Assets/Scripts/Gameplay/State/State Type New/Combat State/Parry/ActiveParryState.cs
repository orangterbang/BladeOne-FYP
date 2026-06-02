using UnityEngine;
using System.Collections;

public class ActiveParryState : StateMachine
{
    ActorContext actor;
    
    public ActiveParryState(ActorContext actorContext)
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

        actor.parry.ExecuteParry(direction);

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
