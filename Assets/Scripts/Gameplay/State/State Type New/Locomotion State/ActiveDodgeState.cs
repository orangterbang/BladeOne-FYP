using UnityEngine;
using System.Collections;

public class ActiveDodgeState : StateMachine
{
    ActorContext actor;

    private Coroutine activeCoroutine;
    
    public ActiveDodgeState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        activeCoroutine = actor.coroutineRun.Run(MoveCoroutine());
    }

    protected override void OnExit()
    {
        if(activeCoroutine != null)
        {
            actor.coroutineRun.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }

    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(actor.movement.GetJumpDuration());

        actor.movement.Move(direction);

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
