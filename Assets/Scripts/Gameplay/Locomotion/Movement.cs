using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [SerializeField]private MovementPoint movementPoint;
    [SerializeField] private float jumpDuration = 0.1f;

    public void Move(Direction moveDirection)
    {
        if(!movementPoint.CheckPoint(moveDirection)) return;

        Point point = movementPoint.GetPoint(moveDirection);
        transform.position = point.pointPos;
    }

    public float GetJumpDuration() => jumpDuration;
}
