using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [SerializeField]private MovementPoint movementPoint;
    [SerializeField] private float jumpDuration = 0.1f;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(Direction moveDirection)
    {
        Debug.Log("In " + name);
        if(!movementPoint.CheckPoint(moveDirection)) return;
        Debug.Log("In " + name + " After CheckPoint");

        StartCoroutine(MoveCoroutine(moveDirection));
    }

    private IEnumerator MoveCoroutine(Direction moveDirection)
    {
        // Start jump animation
        animator.SetBool("isJumping", true);
        animator.CrossFade("Player_Jump", 0.1f, 1);
        yield return new WaitForSeconds(jumpDuration);
        // Change position
        Point point = movementPoint.GetPoint(moveDirection);
        transform.position = point.pointPos;

        // Wait before turning animation off
        

        animator.SetBool("isJumping", false);
    }
}
