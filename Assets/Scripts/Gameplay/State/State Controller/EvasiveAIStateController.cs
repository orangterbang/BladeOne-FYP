using UnityEngine;

public class EvadeAIStateController : StateController
{
    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------
    private ActorContext evadeAI;
    private StateMachine evadeAIState;

    // --- Timers ---
    private float attackTimer;
    private float dodgeTimer;

    // --- Dodge Budget ---
    private int dodgesRemaining;
    private bool mustAttackBeforeDodge;

    // --- Range ---
    private bool isInRange;

    // -------------------------------------------------------------------------
    // Tuning Constants
    // -------------------------------------------------------------------------
    private const float ATTACK_TIMER_MIN = 3f;
    private const float ATTACK_TIMER_MAX = 5f;

    private const float DODGE_TIMER_MIN = 2f;
    private const float DODGE_TIMER_MAX = 10f;

    // -------------------------------------------------------------------------
    // Lifecycle
    // -------------------------------------------------------------------------

    protected override void Start()
    {
        base.Start();
        evadeAI = actor;

        InitializeEvadeStates();
        ResetAttackTimer();
        ResetDodgeTimer();
        RollDodgeBudget();

        mustAttackBeforeDodge = false;
        isInRange = false;
    }

    protected override void Update()
    {
        base.Update();

        evadeAIState?.UpdateStateMachine();

        isInRange = CheckIfInRange();

        if (!evadeAI.actorCombatData.isStunned && !evadeAI.actorCombatData.isDamaged)
        {
            TickTimers();
        }

        HandleAIBehavior();
    }

    protected void OnEnable()
    {
        combatReceiver.OnHitInput += HandleState;
        health.OnHealthZero += HandleState;
    }

    protected void OnDisable()
    {
        combatReceiver.OnHitInput -= HandleState;
        health.OnHealthZero -= HandleState;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        evadeAIState?.ExitStateMachine();
    }

    // -------------------------------------------------------------------------
    // Tick
    // -------------------------------------------------------------------------

    private void TickTimers()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        if (dodgeTimer > 0f)
            dodgeTimer -= Time.deltaTime;
    }

    private void HandleAIBehavior()
    {
        // Not in range — approach by dodging Up, skip everything else this frame
        

        // Dodge check — only fires if in range, budget allows, and attack debt is cleared
        if (dodgeTimer <= 0f && dodgesRemaining > 0 && !mustAttackBeforeDodge)
        {
            TriggerDodge();
            return;
        }

        if (attackTimer <= 0f && actor.actorCombatData.canAttack)
        {
            if (!isInRange)
            {
                TriggerApproach();
                return;
            }

            TriggerAttack();
        }
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    private void TriggerApproach()
    {
        if (!actor.actorCombatData.canDodge) return;

        // Dodge Up to close the gap — don't touch any budget or debt flags,
        // this is purely positional and shouldn't count as a combat dodge
        HandleState(ActionEvent.DodgePressed, Direction.Up);

        ResetDodgeTimer(); // small cooldown so it doesn't spam approach every frame
    }

    private void TriggerAttack()
    {
        HandleState(ActionEvent.AttackPressed, GetRandomDirection());
        mustAttackBeforeDodge = false;

        if (dodgesRemaining <= 0)
            RollDodgeBudget();

        ResetAttackTimer();
    }

    private void TriggerDodge()
    {
        if (!actor.actorCombatData.canDodge) return;

        HandleState(ActionEvent.DodgePressed, GetRandomDirection());

        dodgesRemaining--;
        mustAttackBeforeDodge = true;

        if (dodgesRemaining <= 0)
            dodgesRemaining = 0;

        ResetDodgeTimer();
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    /// <summary>
    /// Checks distance to player. Requires the player GameObject to be tagged "Player".
    /// </summary>
    private bool CheckIfInRange()
    {
        return actor.attack.IsInAttackRange();
    }

    private void ResetAttackTimer()
    {
        attackTimer = Random.Range(ATTACK_TIMER_MIN, ATTACK_TIMER_MAX);
    }

    private void ResetDodgeTimer()
    {
        dodgeTimer = Random.Range(DODGE_TIMER_MIN, DODGE_TIMER_MAX);
    }

    private void RollDodgeBudget()
    {
        dodgesRemaining = Random.Range(1, 3); // 1 or 2
    }

    private Direction GetRandomDirection()
    {
        System.Array directions = System.Enum.GetValues(typeof(Direction));
        return (Direction)directions.GetValue(Random.Range(0, directions.Length));
    }

    // -------------------------------------------------------------------------
    // State Machine Setup
    // -------------------------------------------------------------------------

    private void InitializeEvadeStates()
    {
        evadeAIState         = new EvadeAIState();
        aliveState           = new AliveState();
        deadState            = new DeadState(evadeAI);

        staticState          = new StaticState();
        stanceState          = new StanceState();
        locomotionState      = new LocomotionState(evadeAI);
        offensiveCombatState = new OffensiveCombatState(evadeAI);
        hitStunState         = new HitStunState(evadeAI);

        evadeAIState.LoadSubState(aliveState);
        evadeAIState.LoadSubState(deadState);
        evadeAIState.LoadSubState(hitStunState);

        aliveState.LoadSubState(staticState);
        aliveState.LoadSubState(locomotionState);
        aliveState.LoadSubState(offensiveCombatState);

        staticState.LoadSubState(stanceState);

        evadeAIState.AddTransition(aliveState,   hitStunState, ActionEvent.OnHitReceived);
        evadeAIState.AddTransition(hitStunState, aliveState,   ActionEvent.HitStunRecovered);
        evadeAIState.AddTransition(aliveState,   deadState,    ActionEvent.OnHealthZero);

        aliveState.AddTransition(staticState,          offensiveCombatState, ActionEvent.AttackPressed);
        aliveState.AddTransition(staticState,          locomotionState,      ActionEvent.DodgePressed);
        aliveState.AddTransition(offensiveCombatState, locomotionState,      ActionEvent.DodgePressed);

        aliveState.AddTransition(offensiveCombatState, staticState, ActionEvent.CombatSequenceComplete);
        aliveState.AddTransition(locomotionState,      staticState, ActionEvent.CombatSequenceComplete);

        evadeAIState.EnterStateMachine();
    }

    // -------------------------------------------------------------------------
    // State Handlers
    // -------------------------------------------------------------------------

    protected override void HandleState(ActionEvent action, Direction direction)
    {
        switch (action)
        {
            case ActionEvent.AttackPressed:
                if (!actor.actorCombatData.canAttack) return;
                evadeAIState.SetDirection(direction);
                evadeAIState.SendTrigger(ActionEvent.AttackPressed);
                break;

            case ActionEvent.DodgePressed:
                if (!actor.actorCombatData.canDodge) return;
                evadeAIState.SetDirection(direction);
                evadeAIState.SendTrigger(ActionEvent.DodgePressed);
                break;

            case ActionEvent.OnHitReceived:
            case ActionEvent.OnStunned:
                evadeAIState.SetDirection(direction);
                evadeAIState.SendTrigger(ActionEvent.OnHitReceived);
                break;
        }
    }

    protected override void HandleState(ActionEvent action)
    {
        switch (action)
        {
            case ActionEvent.CombatSequenceComplete:
                evadeAIState.SendTrigger(ActionEvent.CombatSequenceComplete);
                break;
            case ActionEvent.HitStunRecovered:
                evadeAIState.SendTrigger(ActionEvent.HitStunRecovered);
                break;
            case ActionEvent.OnHealthZero:
                evadeAIState.SendTrigger(ActionEvent.OnHealthZero);
                break;
        }
    }
}