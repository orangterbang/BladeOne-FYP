using UnityEngine;

public class DummyAIStateController : StateController
{
    ActorContext dummyAI;
    private StateMachine dummyAIState; // The root state machine tracker for this AI
    private float attackTimer;

    protected override void Start()
    {   
        base.Start();
        dummyAI = actor;
        // Initialize the states required for the dummy AI
        InitializeDummyStates();

        // Set the initial random timer for the first attack
        ResetAttackTimer();
    }

    protected override void Update()
    {
        base.Update();
        
        // Tick the state machine
        if (dummyAIState != null)
        {
            dummyAIState.UpdateStateMachine();
        }

        // Handle the AI attack timing logic
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
        if (dummyAIState != null)
        {
            dummyAIState.ExitStateMachine();
        }
    }

    private void HandleAIBehavior()
    {
        if (!dummyAI.actorCombatData.isStunned)
        {
            attackTimer -= Time.deltaTime;
        }
        
        if (attackTimer <= 0f)
        {
            // Get a random direction
            Direction randomDirection = GetRandomDirection();

            // Trigger the attack state using the overridden HandleState method
            HandleState(ActionEvent.AttackPressed, randomDirection);

            // Reset the timer for the next attack sequence
            ResetAttackTimer();
        }
    }

    private void ResetAttackTimer()
    {
        // Generates a random float between 5 and 10 seconds
        attackTimer = Random.Range(3f, 5f);
    }

    private Direction GetRandomDirection()
    {
        // System.Array is used to pull a random value from your Direction Enum
        System.Array directions = System.Enum.GetValues(typeof(Direction));
        return (Direction)directions.GetValue(Random.Range(0, directions.Length));
    }

    private void InitializeDummyStates()
    {
        // 1. Instantiate the states (using 'actor' from BaseStateController)
        dummyAIState = new DummyAIState(); // Assuming a root BaseState exists, or use PlayerState if generic
        aliveState = new AliveState();
        deadState = new DeadState(dummyAI);

        staticState = new StaticState();
            stanceState = new StanceState();
        offensiveCombatState = new OffensiveCombatState(dummyAI);
        hitStunState = new HitStunState(dummyAI);

        // 2. Load Hierarchy
        dummyAIState.LoadSubState(aliveState);
        dummyAIState.LoadSubState(deadState);
        dummyAIState.LoadSubState(hitStunState);

        aliveState.LoadSubState(staticState);
        aliveState.LoadSubState(offensiveCombatState);     

        staticState.LoadSubState(stanceState);

        // 3. Define Transitions needed for the AI's routine
        dummyAIState.AddTransition(aliveState, deadState, ActionEvent.OnHealthZero);
        dummyAIState.AddTransition(aliveState, hitStunState, ActionEvent.OnHitReceived);
        dummyAIState.AddTransition(hitStunState, aliveState, ActionEvent.HitStunRecovered);

        aliveState.AddTransition(staticState, offensiveCombatState, ActionEvent.AttackPressed);
        aliveState.AddTransition(offensiveCombatState, staticState, ActionEvent.CombatSequenceComplete);

        // 4. Start the state machine
        dummyAIState.EnterStateMachine();
    }

    // --- State Handler Overrides ---
    
    protected override void HandleState(ActionEvent action, Direction direction)
    {
        switch (action)
        {
            case ActionEvent.AttackPressed:
                if(!actor.actorCombatData.canAttack) return;
                dummyAIState.SetDirection(direction);
                dummyAIState.SendTrigger(ActionEvent.AttackPressed);
                break;
            case ActionEvent.OnHitReceived or ActionEvent.OnStunned:
                ResetAttackTimer();
                dummyAIState.SetDirection(direction);
                dummyAIState.SendTrigger(ActionEvent.OnHitReceived);
                break;
        }
    }

    protected override void HandleState(ActionEvent action)
    {
        switch (action)
        {
            case ActionEvent.CombatSequenceComplete:
                dummyAIState.SendTrigger(ActionEvent.CombatSequenceComplete);
                break;
            case ActionEvent.OnHealthZero:
                dummyAIState.SendTrigger(ActionEvent.OnHealthZero);
                break;
        }
    }
}