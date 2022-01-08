using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopegrabAction : Action
{

    [SerializeField] PlayerController playerController;

    [SerializeField] private float jumpVelocity;

    [SerializeField] private float jumpAccelerationTime;
    [SerializeField] private float jumpMovementTime;
    [SerializeField] private float jumpCooldown;


    private void Start()
    {
        timer1.setDuration(jumpAccelerationTime);
        timer2.setDuration(jumpMovementTime);
        cooldown.setDuration(jumpCooldown);
    }


    // Start is called before the first frame update
    override public void start()
    {
        timer1.start();
        cooldown.start();
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
        }
    }

    override public void cancel()
    {
        //playerController.velocity = new Vector2(0, 0);
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }

}
