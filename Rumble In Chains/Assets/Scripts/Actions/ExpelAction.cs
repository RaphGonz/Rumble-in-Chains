using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpelAction : Action
{
    [SerializeField] PlayerController playerController;

    [SerializeField] private float positionDisplacement;

    [SerializeField] private float expelFreezeTime;
    [SerializeField] private float expelMovementTime;
    [SerializeField] private float expelDecelerationTime;
    [SerializeField] private float expelCooldown = 0;

    private Vector2 dashDirection;



    private void Start()
    {
        timer1.setDuration(expelFreezeTime);
        timer2.setDuration(expelMovementTime);
        timer3.setDuration(expelDecelerationTime);
        cooldown.setDuration(expelCooldown);
    }

    public void start(Vector2 direction)
    {
        this.dashDirection = direction;
        timer1.start();
        cooldown.start();
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
        if (!timer2.check())
        {
            playerController.velocity = positionDisplacement / expelMovementTime * dashDirection;
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
            playerController.velocity = positionDisplacement * (1 - timer3.getRatio()) * dashDirection;
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
