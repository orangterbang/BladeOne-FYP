using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class MoveBase : MonoBehaviour
{
    protected Movement movement;
    protected MoveDirection moveDirection;

    protected virtual void Start()
    {
        movement = GetComponent<Movement>();    
    }

    protected virtual void Update()
    {
        
    }
}
