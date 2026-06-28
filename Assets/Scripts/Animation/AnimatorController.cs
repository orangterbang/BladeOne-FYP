using System;
using UnityEngine;
//using System.Diagnostics;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]protected Animator animator;

    public const int BASE_LAYER = 0;
    public const int ACTION_LAYER = 1;
    public const int REACTION_LAYER = 2;
    private bool isActionActive = false;

    public String triggeredAnimationName { get; private set; }
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
            case ActionEvent.OnStunned:
                SetHash("StunnedReaction_WindUp", "isStunned");
                break;
            case ActionEvent.OnHealthZero:
                SetHash("DeathReaction");
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
            case ActionInput.Parry when direction == Direction.Up:
                SetHash("TopParry_WindUp", "isParrying");
                break;
            case ActionInput.Parry when  direction == Direction.Down:
                SetHash("BotParry_WindUp", "isParrying");
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
                SetHash("Top_Stagger");
                break;
            case ActionEvent.OnHitReceived when direction == Direction.Down:
                SetHash("Bot_Stagger");
                break;
            case ActionEvent.OnHitReceived when direction == Direction.Left:
                SetHash("Left_Stagger");
                break;
            case ActionEvent.OnHitReceived when direction == Direction.Right:
                SetHash("Right_Stagger");
                break;
        }
    }

    public void EnableAction()
    {
        animator.ResetTrigger("ActiveAction");
        animator.SetTrigger("ActiveAction");
        isActionActive = true;
    }

    public bool CheckIfActionIsActive() => isActionActive;

    public void EnableReaction()
    {
        animator.ResetTrigger("FinishedReaction");
        animator.SetTrigger("FinishedReaction");
    }

    public void DisableAction()
    {
        animator.ResetTrigger("ActiveAction");
        animator.ResetTrigger("FinishedReaction");
        //Set something to false
        animator.SetBool(animationBool, false);
        isActionActive = false;
    }

    public void PlayAnim(int layer = ACTION_LAYER)
    {
        //Play the animation through trigger
        animator.CrossFade(triggeredAnimationHash, 0.1f, layer);
    }

    private void SetHash(String animName)
    {
        triggeredAnimationHash = Animator.StringToHash(animName);
        triggeredAnimationName = animName;
    }

    private void SetHash(String animName, String animBool)
    {
        triggeredAnimationName = animName;
        triggeredAnimationHash = Animator.StringToHash(animName);
        animationBool = Animator.StringToHash(animBool);
    }

    public bool AnimationClipFinished(int Layer = ACTION_LAYER)
    {
        //UnityEngine.Debug.Log("In AnimationClipFinished");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(Layer);

        //Debug.Log("In AnimationFinished");

        return stateInfo.normalizedTime >= 0.6f && !animator.IsInTransition(Layer);
    }

    public bool AnimationClipFinished(String animName, int Layer = ACTION_LAYER)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(Layer);

        //UnityEngine.Debug.Log("In AnimationFinished");

        /*bool inTransition = animator.IsInTransition(Layer);

        if (stateInfo.IsName(animName))
        {
            // Either fully played out, or already transitioning away (exit time reached)
            return stateInfo.normalizedTime >= 1.0f || inTransition;
        }

        // Already moved past this state entirely
        return !inTransition;*/

        return stateInfo.IsName(animName) && stateInfo.normalizedTime >= 0.1f && !animator.IsInTransition(Layer);
    }

    public String GetFullAnimationStateName(String stateName, int Layer = ACTION_LAYER)
    {
        //Safely check the current playing clip first
        AnimatorClipInfo[] currentClips = animator.GetCurrentAnimatorClipInfo(Layer);
        if (currentClips != null && currentClips.Length > 0)
        {
            string clipName = currentClips[0].clip.name;
            if (IsMatchingState(clipName, stateName)) return clipName;
        }

        //Safely check the next clip if the animator is mid-transition
        AnimatorClipInfo[] nextClips = animator.GetNextAnimatorClipInfo(Layer);
        if (nextClips != null && nextClips.Length > 0)
        {
            string clipName = nextClips[0].clip.name;
            if (IsMatchingState(clipName, stateName)) return clipName;
        }

        return null; 
    }

    private bool IsMatchingState(string fullClipName, string targetStateName)
    {
        int position = fullClipName.IndexOf("_");
        if (position == -1) return false;

        string extractedStateName = fullClipName.Substring(position + 1);
        return extractedStateName.Equals(targetStateName, System.StringComparison.OrdinalIgnoreCase);
    }
}
