using UnityEngine;

public class PlayerStateController : StateController
{
    ActorContext player;
    MoveState moveState;
        StanceState stanceState;
        DodgeState dodgeState;


    protected override void Start()
    {   
        base.Start();
        player = actor;

        moveState = new MoveState();
        stanceState = new StanceState();
        dodgeState = new DodgeState(player);

        
        moveState.LoadSubState(stanceState);
        moveState.LoadSubState(dodgeState);
        moveState.AddTransition(stanceState, dodgeState, ActionInput.Move);
        moveState.AddTransition(dodgeState, stanceState, ActionInput.Stance);
        moveState.EnterStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        moveState.UpdateStateMachine();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        moveState.ExitStateMachine();
    }

    protected void OnEnable()
    {
        move.OnMoveAction += HandleState;
    }

    protected void OnDisable()
    {
        move.OnMoveAction -= HandleState;
    }

    protected override void HandleState(ActionInput action, Direction direction)
    {
        switch (action)
        {
            case ActionInput.Move:
                dodgeState.SetDirection(direction);
                moveState.SendTrigger(action);
                HandleState(ActionInput.Stance, direction);
                break;
            case ActionInput.Stance:
                stanceState.SetDirection(direction);
                moveState.SendTrigger(action);
                break;
        }
    }
}
