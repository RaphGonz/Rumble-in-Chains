using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerAnimation playerAnimation;

    [SerializeField] private float attackTotalTime;
    [SerializeField] private float attackCooldown;

    private Vector2 joystickDirection;
    private AttackType attackType = AttackType.Jab;
    private bool grounded = false;


    private void Start()
    {
        timer1.setDuration(attackTotalTime);
        cooldown.setDuration(attackCooldown);
    }


    // Start is called before the first frame update
    public void start(Vector2 direction, bool isGrounded)
    {
        joystickDirection = direction;
        grounded = isGrounded;
        timer1.start();
        cooldown.start();
        selectAttack();
        characterController.Attack(attackType);
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            if (timer1.check())
            {
                timer1.reset();
            }
            return false;
        }

        playerAnimation.AnimationState = AnimationState.IDLE;
        return true;
    }

    private void selectAttack()
    {
        if (joystickDirection.x == 1 || joystickDirection.x == -1)
        {
            attackType = AttackType.SideTilt;
            playerAnimation.AnimationState = AnimationState.SIDE;
            //Debug.Log("sideTilt");
        }
        else if (joystickDirection.y == 1)
        {
            attackType = AttackType.UpTilt;
            playerAnimation.AnimationState = AnimationState.UP;
            //Debug.Log("UpTilt");
        }
        else if (joystickDirection.y == -1)
        {
            if (grounded)
            {
                attackType = AttackType.DownTilt;
                playerAnimation.AnimationState = AnimationState.DGROUND;
                //Debug.Log("DownTilt");
            }
            else
            {
                attackType = AttackType.DownAir;
                playerAnimation.AnimationState = AnimationState.DAIR;
                //Debug.Log("DownAir");
            }
        }
        else
        {
            attackType = AttackType.Jab;
            playerAnimation.AnimationState = AnimationState.JAB;
            //Debug.Log("Jab");
        }
    }


    override public void cancel()
    {
        playerAnimation.AnimationState = AnimationState.IDLE;
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
