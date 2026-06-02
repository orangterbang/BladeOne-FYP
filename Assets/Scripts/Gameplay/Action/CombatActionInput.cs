using UnityEngine;

public class CombatActionInput : ActionBase
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
        if(Input.GetKey(KeyCode.UpArrow)){actionDirection = Direction.Up; isPressing = true;}
        if(Input.GetKey(KeyCode.DownArrow)){actionDirection = Direction.Down; isPressing = true;}
        if(Input.GetKey(KeyCode.LeftArrow)){actionDirection = Direction.Left; isPressing = true;}
        if(Input.GetKey(KeyCode.RightArrow)){actionDirection = Direction.Right; isPressing = true;}
        

        if(Input.GetKeyDown(KeyCode.D) && isPressing)
        {
            RaiseMoveAction(ActionEvent.AttackPressed, actionDirection);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isPressing)
        {
            RaiseMoveAction(ActionEvent.ParryPressed, actionDirection);
        }

    }
}
