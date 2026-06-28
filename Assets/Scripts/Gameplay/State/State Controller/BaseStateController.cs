using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    [Header("Event Object")]
    protected MoveBase move;
    protected ActionBase action;
    protected Health health;
    protected CombatReceiver combatReceiver;

    [Header("Actor Context")]
    protected ActorContext actor;

    [Header("State")]
    protected AliveState aliveState;
    protected DeadState deadState;

    protected StaticState staticState;
        protected StanceState stanceState;
    protected LocomotionState locomotionState;   
    protected OffensiveCombatState offensiveCombatState;
    protected DefensiveCombatState defensiveCombatState;
    protected HitStunState hitStunState;
    
    protected virtual void Awake()
    {
        move = GetComponent<MoveBase>();
        action = GetComponent<ActionBase>();
        actor = GetComponent<ActorContext>();
        combatReceiver = GetComponent<CombatReceiver>();
        health = GetComponent<Health>();
    }

    protected virtual void Start(){}

    // Update is called once per frame
    protected virtual void Update(){}
    protected virtual void OnDestroy(){}

    protected abstract void HandleState(ActionEvent action);
    protected abstract void HandleState(ActionEvent action, Direction direction);
}

//might use interface later on or just add the movement input/movebase to have the event initialized