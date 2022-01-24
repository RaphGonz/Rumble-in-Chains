using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerAnimation playerAnimation;

    [SerializeField] private float attackCooldown;

    private bool inPostLag;

    private float preLagAndHitboxFrames;
    private float postLagFrames;

    private Vector2 joystickDirection;
    private AttackType attackType = AttackType.Jab;
    private bool grounded = false;
    Vector2 attackFrames;


    private void Start()
    {
        timer1.setDuration(preLagAndHitboxFrames);
        timer2.setDuration(postLagFrames);
        cooldown.setDuration(attackCooldown);
        inPostLag = false;
    }


    // Start is called before the first frame update
    public void start(Vector2 direction, bool isGrounded)
    {
        joystickDirection = direction;
        grounded = isGrounded;

        selectAttack();
        attackFrames = characterController.Attack(attackType);

        attackFrames = attackFrames / 60;

        preLagAndHitboxFrames = attackFrames.x;
        postLagFrames = attackFrames.y;
        inPostLag = false;

        timer1.setDuration(preLagAndHitboxFrames);
        timer2.setDuration(postLagFrames);

        timer1.start();
        cooldown.start();
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            if (timer1.check())
            {
                timer1.reset();
                inPostLag = true;
                timer2.start();
            }
            return false;
        }
        else if (timer2.isActive())
        {
            if (timer2.check())
            {
                timer2.reset();
            }
            return false;
        }

        inPostLag = false;
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
        inPostLag = false;
    }

    public bool InPostLag()
    {
        return inPostLag;
    }
}
