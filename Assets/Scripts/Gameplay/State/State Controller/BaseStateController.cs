using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    protected MoveBase move;
    protected ActionBase action;
    protected ActorContext actor;

    protected virtual void Awake()
    {
        move = GetComponent<MoveBase>();
        action = GetComponent<ActionBase>();
        actor = GetComponent<ActorContext>();
    }

    protected virtual void Start(){}

    // Update is called once per frame
    protected virtual void Update(){}
    protected virtual void OnDestroy(){}

    protected abstract void HandleState(ActionInput action, Direction direction);
}

//might use interface later on or just add the movement input/movebase to have the event initialized