using UnityEngine;

public class PlayerStateController : StateController
{
    ActorContext player;
    PlayerState playerState;
    AliveState aliveState;
    DeadState deadState;

    StaticState staticState;
        StanceState stanceState;

    LocomotionState locomotionState;
        DodgeWindUpState dodgeWindUpState;
        ActiveDodgeState activeDodgeState;
        DodgeRecoveryState dodgeRecoveryState;
    
    OffensiveCombatState offensiveCombatState;
        AttackWindUpState attackWindUpState;
        ActiveAttackState activeAttackState;
        AttackRecoveryState attackRecoveryState;

    DefensiveCombatState defensiveCombatState;
        ParryWindUpState parryWindUpState;
        ActiveParryState activeParryState;
        ParryRecoveryState parryRecoveryState;

    
    HitStunState hitStunState;
        StaggerState staggerState;
        KnockbackState knockbackState;
        StunnedState stunnedState;
        WakeUpState wakeUpState;


    protected override void Start()
    {   
        base.Start();
        player = actor;
        
        InitializeAllStates();
    }

    protected override void Update()
    {
        base.Update();
        playerState.UpdateStateMachine();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        playerState.ExitStateMachine();
    }

    protected void OnEnable()
    {
        move.OnMoveAction += HandleState;
        action.OnActionInput += HandleState;
        combatReceiver.OnHitInput += HandleState;
    }

    protected void OnDisable()
    {
        move.OnMoveAction -= HandleState;
        action.OnActionInput -= HandleState;
        combatReceiver.OnHitInput -= HandleState;
    }

    protected override void HandleState(ActionEvent action, Direction direction)
    {
        switch (action)
        {
            case ActionEvent.DodgePressed:
                aliveState.SetDirection(direction);
                aliveState.SendTrigger(ActionEvent.DodgePressed);
                break;
            case ActionEvent.AttackPressed:
                aliveState.SetDirection(direction);
                aliveState.SendTrigger(ActionEvent.AttackPressed);
                break;
            case ActionEvent.ParryPressed:
                aliveState.SetDirection(direction);
                aliveState.SendTrigger(ActionEvent.ParryPressed);
                break;
            case ActionEvent.OnHitReceived:
                aliveState.SetDirection(direction);
                aliveState.SendTrigger(ActionEvent.OnHitReceived);
                break;
            
        }
    }

    protected override void HandleState(ActionEvent action)
    {
        switch (action)
        {
            case ActionEvent.OnHealthZero:
                aliveState.SendTrigger(ActionEvent.OnHealthZero);
                break;
            case ActionEvent.HitStunRecovered:
                aliveState.SendTrigger(ActionEvent.HitStunRecovered);
                break;
            case ActionEvent.OnRespawn:
                aliveState.SendTrigger(ActionEvent.OnRespawn);
                break;
            
            case ActionEvent.CombatSequenceComplete:
                aliveState.SendTrigger(ActionEvent.CombatSequenceComplete);
                break;
        }
    }

    private void InitializeAllStates()
    {
        //Main State
        playerState = new PlayerState();
        aliveState = new AliveState();
        deadState = new DeadState();

        //Static State
        staticState = new StaticState();
        stanceState = new StanceState();

        //Locomotion State
        locomotionState = new LocomotionState(player);

        //Combat State
        offensiveCombatState = new OffensiveCombatState(player);


        defensiveCombatState = new DefensiveCombatState(player);

        //HitStun State
        hitStunState = new HitStunState(player);

        //Load State
        playerState.LoadSubState(aliveState);
        playerState.LoadSubState(deadState);

        aliveState.LoadSubState(staticState);
        aliveState.LoadSubState(locomotionState);
        aliveState.LoadSubState(offensiveCombatState);
        aliveState.LoadSubState(defensiveCombatState);
        aliveState.LoadSubState(hitStunState);

        staticState.LoadSubState(stanceState);

        //Transition
        playerState.AddTransition(aliveState, deadState, ActionEvent.OnHealthZero);
        playerState.AddTransition(deadState, aliveState, ActionEvent.OnRespawn);

        aliveState.AddTransition(staticState, offensiveCombatState, ActionEvent.AttackPressed);
        aliveState.AddTransition(staticState, defensiveCombatState, ActionEvent.ParryPressed);
        aliveState.AddTransition(staticState, locomotionState, ActionEvent.DodgePressed);

        //Transition that allows attack & parry be interrupted by each other & dodge but not allow both interrupt dodge
        aliveState.AddTransition(defensiveCombatState, offensiveCombatState, ActionEvent.AttackPressed);
        aliveState.AddTransition(offensiveCombatState, defensiveCombatState, ActionEvent.ParryPressed);
        aliveState.AddTransition(offensiveCombatState, locomotionState, ActionEvent.DodgePressed);
        aliveState.AddTransition(defensiveCombatState, locomotionState, ActionEvent.DodgePressed);

        aliveState.AddTransition(offensiveCombatState, staticState, ActionEvent.CombatSequenceComplete);
        aliveState.AddTransition(defensiveCombatState, staticState, ActionEvent.CombatSequenceComplete);
        aliveState.AddTransition(locomotionState, staticState, ActionEvent.CombatSequenceComplete);

        aliveState.AddTransition(staticState, hitStunState, ActionEvent.OnHitReceived);
        aliveState.AddTransition(locomotionState, hitStunState, ActionEvent.OnHitReceived);
        aliveState.AddTransition(defensiveCombatState, hitStunState, ActionEvent.OnHitReceived);

        aliveState.AddTransition(hitStunState, staticState, ActionEvent.HitStunRecovered);

        playerState.EnterStateMachine();
    }
}
