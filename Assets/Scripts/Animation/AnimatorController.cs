using System;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    protected Animator animator;

    private int triggeredAnimationHash;
    private int animationBool;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void SetActionAnimation(ActionInput actionInput)
    {
        //Get the actionInput
        switch (actionInput)
        {
            case ActionInput.Move:
                SetHash("Jump", "isJumping");
                break;
            case ActionInput.Attack:
                SetHash("Attack", "isAttacking");
                break;
            case ActionInput.Parry:
                SetHash("Parry", "isParrying");
                break;
        }
    }

    public void EnableAction()
    {
        //Set something to true
        animator.SetBool(animationBool, true);
    }

    public void DisableAction()
    {
        //Set something to false
        animator.SetBool(animationBool, false);
    }

    public void PlayAnim()
    {
        //Play the animation through trigger
        animator.CrossFade(triggeredAnimationHash, 0.1f, 1);
    }

    private void SetHash(String animName, String animBool)
    {
        triggeredAnimationHash = Animator.StringToHash(animName);
        animationBool = Animator.StringToHash(animBool);
    }
}
