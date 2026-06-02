using UnityEngine;

public class LocomotionState : StateMachine
{
    ActorContext actor;
    
    public LocomotionState(ActorContext actorContext)
    {
        this.actor=actorContext;

        DodgeWindUpState dodgeWindUpState = new DodgeWindUpState(actor);
        ActiveDodgeState activeDodgeState = new ActiveDodgeState(actor);
        DodgeRecoveryState dodgeRecoveryState = new DodgeRecoveryState(actor);

        LoadSubState(dodgeWindUpState);
        LoadSubState(activeDodgeState);
        LoadSubState(dodgeRecoveryState);

        AddTransition(dodgeWindUpState, activeDodgeState, ActionEvent.ChangeSubState);
        AddTransition(activeDodgeState, dodgeRecoveryState, ActionEvent.ChangeSubState);
    }
}