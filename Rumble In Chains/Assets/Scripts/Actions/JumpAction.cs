using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JumpAction : Action
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerAnimation playerAnimation;

    [SerializeField] private float jumpAccelerationTime;
    [SerializeField] private float jumpMovementTime;
    [SerializeField] private float jumpCooldown;

    private float jumpVelocity;


    private void Start()
    {
        timer1.setDuration(jumpAccelerationTime);
        timer2.setDuration(jumpMovementTime);
        cooldown.setDuration(jumpCooldown);


        Character character = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + (this.gameObject.layer == 17 ? GameManager.Instance.characterPlayer1 : GameManager.Instance.characterPlayer2) + ".asset");
        float jumpDistance = character.characterConverter.convertJumpHeight(character.jumpHeight);
        jumpVelocity = jumpDistance / (jumpMovementTime + 0.5f * jumpAccelerationTime);
    }


    // Start is called before the first frame update
    override public void start()
    {
        timer1.start();
        cooldown.start();
        playerAnimation.AnimationState = AnimationState.JUMP;
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            phase1Acceleration();
            return false;
        }
        else if (timer2.isActive())
        {
            phase2Movement();
            return false;
        }

        return true;
    }

    private void phase1Acceleration()
    {
        if (!timer1.check())
        {
            playerController.velocity.y = jumpVelocity * timer1.getRatio();
        }
        else
        {
            timer2.start();
            timer1.reset();
        }
    }

    private void phase2Movement()
    {
        if (!timer2.check())
        {
            playerController.velocity.y = jumpVelocity;
        }
        else
        {
            timer3.start();
            timer2.reset();
            playerAnimation.AnimationState = AnimationState.IDLE;
        }
    }

    override public void cancel()
    {
        //playerController.velocity = new Vector2(0, 0);
        playerAnimation.AnimationState = AnimationState.IDLE;
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
