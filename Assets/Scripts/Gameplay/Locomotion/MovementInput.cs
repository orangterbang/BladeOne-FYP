using UnityEngine;

public class MovementInput : MoveBase
{
    bool isPressing = false;
    bool lockInput = false;

    protected override void Start()
    {
        base.Start();

        lockInput = false;
    }

    protected override void Update()
    {
        base.Update();

        if (!lockInput)
        {
            InputAction();

            isPressing = false;
        }
    }

    void InputAction()
    {
        if(Input.GetKey(KeyCode.UpArrow)){moveDirection = Direction.Up; isPressing = true;}
        if(Input.GetKey(KeyCode.DownArrow)){moveDirection = Direction.Down; isPressing = true;}
        if(Input.GetKey(KeyCode.LeftArrow)){moveDirection = Direction.Left; isPressing = true;}
        if(Input.GetKey(KeyCode.RightArrow)){moveDirection = Direction.Right; isPressing = true;}
        

        if(Input.GetKeyDown(KeyCode.LeftShift) && isPressing)
        {
            RaiseMoveAction(ActionInput.Move, moveDirection);
        }
    }
}
