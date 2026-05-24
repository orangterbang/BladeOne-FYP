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
        if(Input.GetKey(KeyCode.UpArrow)){moveDirection = MoveDirection.FrontMid; isPressing = true;}
        if(Input.GetKey(KeyCode.DownArrow)){moveDirection = MoveDirection.BackMid; isPressing = true;}
        if(Input.GetKey(KeyCode.LeftArrow)){moveDirection = MoveDirection.CenterLeft; isPressing = true;}
        if(Input.GetKey(KeyCode.RightArrow)){moveDirection = MoveDirection.CenterRight; isPressing = true;}
        

        if(Input.GetKeyDown(KeyCode.LeftShift) && isPressing)
        {
            movement.Move(moveDirection);
        }
    }
}
