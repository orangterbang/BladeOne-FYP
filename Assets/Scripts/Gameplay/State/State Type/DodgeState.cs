using UnityEngine;

public class DodgeState : StateMachine
{
    Movement movement;

    public DodgeState(Movement movement)
    {
        this.movement = movement;
    }
    protected override void OnEnter()
    {Debug.Log("In Dodge State with direction " + direction);
        if(movement != null)
        {
            movement.Move(direction);
        }
    }
    protected override void OnUpdate()
    {
        
    }
    protected override void OnExit(){}
}
