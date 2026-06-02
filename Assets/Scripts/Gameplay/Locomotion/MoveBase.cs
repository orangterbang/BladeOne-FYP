using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class MoveBase : MonoBehaviour
{
    public event Action<ActionEvent, Direction> OnMoveAction;
    protected Movement movement;
    protected Direction moveDirection;

    protected virtual void Start()
    {
        movement = GetComponent<Movement>();    
    }

    protected virtual void Update()
    {
        
    }

    protected void RaiseMoveAction(ActionEvent action, Direction direction)
    {
        OnMoveAction?.Invoke(action, direction);
    }
}
