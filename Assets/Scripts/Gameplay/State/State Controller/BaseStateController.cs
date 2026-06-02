using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    [Header("Event Object")]
    protected MoveBase move;
    protected ActionBase action;
    protected CombatReceiver combatReceiver;

    [Header("Actor Context")]
    protected ActorContext actor;

    protected virtual void Awake()
    {
        move = GetComponent<MoveBase>();
        action = GetComponent<ActionBase>();
        actor = GetComponent<ActorContext>();
        combatReceiver = GetComponent<CombatReceiver>();
    }

    protected virtual void Start(){}

    // Update is called once per frame
    protected virtual void Update(){}
    protected virtual void OnDestroy(){}

    protected abstract void HandleState(ActionEvent action);
    protected abstract void HandleState(ActionEvent action, Direction direction);
}

//might use interface later on or just add the movement input/movebase to have the event initialized