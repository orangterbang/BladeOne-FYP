using System;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    protected Animator animator;

    public const int ACTION_LAYER = 1;
    public const int REACTION_LAYER = 2;

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
        }
    }

    public void SetActionAnimation(ActionEvent actionEvent)
    {
        //Get the actionInput
        switch (actionEvent)
        {
            case ActionEvent.OnHitReceived:
                SetHash("Stagger", "isStaggered");
                break;
            case ActionEvent.OnStunned:
                SetHash("Stunned", "isStunned");
                break;
            case ActionEvent.HitStunRecovered:
                SetHash("StunRecovery", "isStunned");
                break;
        }
    }

    public void SetActionAnimation(ActionInput actionInput, Direction direction)
    {
        //Get the actionInput
        switch (actionInput)
        {
            //Attack
            case ActionInput.Attack when direction == Direction.Up:
                SetHash("TopLightAttack_WindUp", "isAttackActive");
                break;
            case ActionInput.Attack when direction == Direction.Down:
                SetHash("BotLightAttack_WindUp", "isAttackActive");
                break;
            case ActionInput.Attack when direction == Direction.Left:
                SetHash("LeftLightAttack_WindUp", "isAttackActive");
                break;
            case ActionInput.Attack when direction == Direction.Right:
                SetHash("RightLightAttack_WindUp", "isAttackActive");
                break;

            //Parry
            case ActionInput.Parry when direction == Direction.Up || direction == Direction.Down:
                SetHash("CenterParry_WindUp", "isParrying");
                break;
            case ActionInput.Parry when direction == Direction.Left:
                SetHash("LeftParry_WindUp", "isParrying");
                break;
            case ActionInput.Parry when direction == Direction.Right:
                SetHash("RightParry_WindUp", "isParrying");
                break;
        }
    }

    public void SetActionAnimation(ActionEvent actionEvent, Direction direction)
    {
        //Get the actionInput
        switch (actionEvent)
        {           
            //Stagger
            case ActionEvent.OnHitReceived when direction == Direction.Up:
                SetHash("TopStagger");
                break;
            case ActionEvent.OnHitReceived when direction == Direction.Down:
                SetHash("BotStagger");
                break;
            case ActionEvent.OnHitReceived when direction == Direction.Left:
                SetHash("LeftStagger");
                break;
            case ActionEvent.OnHitReceived when direction == Direction.Right:
                SetHash("RightStagger");
                break;
        }
    }

    public void EnableAction()
    {
        //Set something to true
        //animator.SetBool(animationBool, true);
        animator.SetTrigger("ActiveAction");
    }

    public void DisableAction()
    {
        //Set something to false
        animator.SetBool(animationBool, false);
    }

    public void PlayAnim(int layer = ACTION_LAYER)
    {
        //Play the animation through trigger
        animator.CrossFade(triggeredAnimationHash, 0.1f, layer);
    }

    private void SetHash(String animName)
    {
        triggeredAnimationHash = Animator.StringToHash(animName);
    }

    private void SetHash(String animName, String animBool)
    {
        triggeredAnimationHash = Animator.StringToHash(animName);
        animationBool = Animator.StringToHash(animBool);
    }
}
