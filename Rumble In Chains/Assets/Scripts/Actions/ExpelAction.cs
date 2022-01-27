using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExpelAction : Action
{
    [SerializeField] PlayerController playerController;

    [SerializeField] private float positionDisplacement;

    [SerializeField] private float maxExpelDistance = 10;

    [SerializeField] private float expelFreezeTime;
    [SerializeField] private float expelMovementTime;
    [SerializeField] private float expelDecelerationTime;
    [SerializeField] private float expelCooldown = 0;

    private Vector2 dashDirection;
    private float weight;



    private void Start()
    {
        timer1.setDuration(expelFreezeTime);
        timer2.setDuration(expelMovementTime);
        timer3.setDuration(expelDecelerationTime);
        cooldown.setDuration(expelCooldown);

        Character character = (Character)Resources.Load("Characters/" + (this.gameObject.layer == 17 ? GameManager.Instance.characterPlayer1 : GameManager.Instance.characterPlayer2));
        weight = character.characterConverter.convertWeight(character.weight);
        positionDisplacement = positionDisplacement / weight;
    }

    public void start(Vector2 direction)
    {
        this.dashDirection = direction;
        timer1.start();
        cooldown.start();
        dashDirection /= weight;

        if (dashDirection.magnitude > maxExpelDistance)
        {
            dashDirection = maxExpelDistance * dashDirection.normalized;
        }
    }

    // Start is called before the first frame update
    override public void start()
    {
        this.dashDirection = new Vector2(0, 1);
        timer1.start();
        cooldown.start();
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            phase1Freeze();
            return false;
        }
        else if (timer2.isActive())
        {
            phase2Movement();
            return false;
        }
        else if (timer3.isActive())
        {
            phase3Deceleration();
            return false;
        }

        return true;
    }

    private void phase1Freeze()
    {
        if (!timer1.check())
        {
            playerController.velocity = new Vector2(0, 0);
        }
        else
        {
            timer2.start();
            timer1.reset();
        }
    }

    private void phase2Movement()
    {
        //print("player touched by attack");
        if (!timer2.check())
        {
            playerController.velocity = dashDirection / expelMovementTime ;
        }
        else
        {
            timer3.start();
            timer2.reset();
        }
    }

    private void phase3Deceleration()
    {
        if (!timer3.check())
        {
            playerController.velocity = dashDirection * (1 - timer3.getRatio());
        }
        else
        {
            timer3.reset();
            playerController.velocity = new Vector2(0, 0);
        }
    }

    override public void cancel()
    {
        playerController.velocity = new Vector2(0, 0);
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
