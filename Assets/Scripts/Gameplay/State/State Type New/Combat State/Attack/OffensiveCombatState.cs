using UnityEngine;

public class OffensiveCombatState : StateMachine
{
    ActorContext actor;
    public OffensiveCombatState(ActorContext actorContext)
    {
        this.actor=actorContext;

        AttackWindUpState attackWindUpState = new AttackWindUpState(actor);
        ActiveAttackState activeAttackState = new ActiveAttackState(actor);
        AttackRecoveryState attackRecoveryState = new AttackRecoveryState(actor);

        LoadSubState(attackWindUpState);
        LoadSubState(activeAttackState);
        LoadSubState(attackRecoveryState);

        AddTransition(attackWindUpState, activeAttackState, ActionEvent.ChangeSubState);
        AddTransition(activeAttackState, attackRecoveryState, ActionEvent.ChangeSubState);
    }  
}
