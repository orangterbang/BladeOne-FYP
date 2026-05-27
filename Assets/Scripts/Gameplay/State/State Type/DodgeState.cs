using UnityEngine;
using System.Collections;

public class DodgeState : StateMachine
{
    ActorContext actor;

    public DodgeState(ActorContext actor)
    {
        this.actor = actor;
    }
    protected override void OnEnter()
    {
        if(actor != null)
        {
            actor.coroutineRun.Run(MoveCoroutine());
        }
    }
    protected override void OnUpdate()
    {
        
    }
    protected override void OnExit(){}

    private IEnumerator MoveCoroutine()
    {
        actor.animator.SetActionAnimation(ActionInput.Move);
        actor.animator.EnableAction();
        actor.animator.PlayAnim();

        yield return new WaitForSeconds(actor.movement.GetJumpDuration());

        actor.movement.Move(direction);
        actor.animator.DisableAction();
    }
}
