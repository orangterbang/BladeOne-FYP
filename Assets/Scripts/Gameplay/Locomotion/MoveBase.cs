using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class MoveBase : MonoBehaviour
{
    public event Action<ActionInput, Direction> OnMoveAction;
    protected Movement movement;
    protected Direction moveDirection;

    protected virtual void Start()
    {
        movement = GetComponent<Movement>();    
    }

    protected virtual void Update()
    {
        
    }

    protected void RaiseMoveAction(ActionInput action, Direction direction)
    {
        OnMoveAction?.Invoke(ActionInput.Move, direction);
        Debug.Log("Move Action Raised with direction " + direction);
    }
}
