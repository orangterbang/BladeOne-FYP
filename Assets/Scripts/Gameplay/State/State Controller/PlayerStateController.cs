using UnityEngine;

public class PlayerStateController : StateController
{
    ActorContext player;
    PlayerState playerState;

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
        health.OnHealthZero += HandleState;
    }

    protected void OnDisable()
    {
        move.OnMoveAction -= HandleState;
        action.OnActionInput -= HandleState;
        combatReceiver.OnHitInput -= HandleState;
        health.OnHealthZero -= HandleState;
    }

    protected override void HandleState(ActionEvent action, Direction direction)
    {
        switch (action)
        {
            case ActionEvent.DodgePressed:
                if(!actor.actorCombatData.canDodge) return;
                playerState.SetDirection(direction);
                playerState.SendTrigger(ActionEvent.DodgePressed);
                break;
            case ActionEvent.AttackPressed:
                if(!actor.actorCombatData.canAttack) return;
                playerState.SetDirection(direction);
                playerState.SendTrigger(ActionEvent.AttackPressed);
                break;
            case ActionEvent.ParryPressed:
                if(!actor.actorCombatData.canParry) return;
                playerState.SetDirection(direction);
                playerState.SendTrigger(ActionEvent.ParryPressed);
                break;
            case ActionEvent.OnHitReceived:
                playerState.SetDirection(direction);
                playerState.SendTrigger(ActionEvent.OnHitReceived);
                break;
            
        }
    }

    protected override void HandleState(ActionEvent action)
    {
        switch (action)
        {
            case ActionEvent.OnHealthZero:
                playerState.SendTrigger(ActionEvent.OnHealthZero);
                break;
            case ActionEvent.HitStunRecovered:
                playerState.SendTrigger(ActionEvent.HitStunRecovered);
                break;
            case ActionEvent.OnRespawn:
                playerState.SendTrigger(ActionEvent.OnRespawn);
                break;
            
            case ActionEvent.CombatSequenceComplete:
                playerState.SendTrigger(ActionEvent.CombatSequenceComplete);
                break;
        }
    }

    private void InitializeAllStates()
    {
        //Main State
        playerState = new PlayerState();
        aliveState = new AliveState();
        deadState = new DeadState(player);

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
        playerState.LoadSubState(hitStunState);

        aliveState.LoadSubState(staticState);
        aliveState.LoadSubState(locomotionState);
        aliveState.LoadSubState(offensiveCombatState);
        aliveState.LoadSubState(defensiveCombatState);

        staticState.LoadSubState(stanceState);

        //Global Transition
        playerState.AddTransition(aliveState, hitStunState, ActionEvent.OnHitReceived);
        playerState.AddTransition(hitStunState, aliveState, ActionEvent.HitStunRecovered);

        playerState.AddTransition(aliveState, deadState, ActionEvent.OnHealthZero);
        playerState.AddTransition(deadState, aliveState, ActionEvent.OnRespawn);

        //Local Action Transition
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

        playerState.EnterStateMachine();
    }
}
