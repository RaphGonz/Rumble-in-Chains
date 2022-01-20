using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum AnimationState
{
    IDLE,
    JAB,
    SIDE,
    UP,
    DAIR,
    DGROUND,
    JUMP,
    MOVEX,
    MOVEDOWN,
    FOCUSDASH,
    DASH,
    FOCUSROPEGRAB,
    BACKROPEGRAB,
    UPROPEGRAB,
    DOWNROPEGRAB,
    STUN,
    EXPEL,
    COUNT
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    AnimationState _animationState = AnimationState.IDLE;

    public AnimationState AnimationState { get => _animationState; set { _animationState = value; } }

    public void UpdateAnimator()
    {
        animator.SetInteger("State", (int)_animationState);
    }

    public void SetBool(string s, bool b)
    {
        animator.SetBool(s, b);
    }

    public void SetInt(string s, int i)
    {
        animator.SetInteger(s, i);
    }

    public void SetFloat(string s, float f)
    {
        animator.SetFloat(s, f);
    }

    public void Trigger(string s)
    {
        animator.SetTrigger(s);
    }

}
