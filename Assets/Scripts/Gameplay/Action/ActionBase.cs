using System;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    public event Action<ActionEvent, Direction> OnActionInput;

    protected Direction actionDirection;

    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        
    }

    protected void RaiseMoveAction(ActionEvent action, Direction direction)
    {
        OnActionInput?.Invoke(action, direction);
    }
}
