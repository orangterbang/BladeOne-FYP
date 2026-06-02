using UnityEngine;
using System.Collections;

public class ActiveDodgeState : StateMachine
{
    ActorContext actor;
    
    public ActiveDodgeState(ActorContext actorContext)
    {
        this.actor=actorContext;
    }

    protected override void OnEnter()
    {
        actor.coroutineRun.Run(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(actor.movement.GetJumpDuration());

        actor.movement.Move(direction);

        SendTrigger(ActionEvent.ChangeSubState);
    }
}
