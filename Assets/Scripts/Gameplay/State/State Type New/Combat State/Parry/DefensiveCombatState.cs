using UnityEngine;

public class DefensiveCombatState : StateMachine
{
    ActorContext actor;
    public DefensiveCombatState(ActorContext actorContext)
    {
        this.actor=actorContext;

        ParryWindUpState parryWindUpState = new ParryWindUpState(actor);
        ActiveParryState activeParryState = new ActiveParryState(actor);
        ParryRecoveryState parryRecoveryState = new ParryRecoveryState(actor);

        LoadSubState(parryWindUpState);
        LoadSubState(activeParryState);
        LoadSubState(parryRecoveryState);

        AddTransition(parryWindUpState, activeParryState, ActionEvent.ChangeSubState);
        AddTransition(activeParryState, parryRecoveryState, ActionEvent.ChangeSubState);
    }
}
