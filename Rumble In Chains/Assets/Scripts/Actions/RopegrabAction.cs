using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopegrabAction : Action
{

    [SerializeField] PlayerController playerController;
    [SerializeField] RopeManager ropeManager;

    [SerializeField] int playerNumber;

    [SerializeField] private float maxGrabAngle;
    private float currentGrabAngle = 0;
    private Vector2 joystickDirection;


    [SerializeField] private float ropegrabFreezeTime;
    [SerializeField] private float ropegrabGrabTime;
    [SerializeField] private float ropegrabCooldown;


    private void Start()
    {
        timer1.setDuration(ropegrabFreezeTime);
        timer2.setDuration(ropegrabGrabTime);
        cooldown.setDuration(ropegrabCooldown);
    }


    // Start is called before the first frame update
    public void start(Vector2 direction)
    {
        joystickDirection = direction;
        timer3.setDuration(0);
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
            if (joystickDirection.y > 0)
            {
                phase2GrabWithAdjustment();
            }
            else
            {
                phase2Attraction();
            }
            return false;
        }
        else if (timer3.isActive())
        {
            phase3PrelagIfCollision();
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
            ropeManager.startRopeGab();
            currentGrabAngle = 0;
        }
    }

    private void phase2GrabWithAdjustment()
    {
        if (!timer2.check())
        {
            playerController.velocity = new Vector2(0, 0);
            currentGrabAngle = timer2.getRatio() * maxGrabAngle;
            Vector2 newDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * currentGrabAngle), Mathf.Sin(Mathf.Deg2Rad * currentGrabAngle));

            if (ropeManager.placePointsTowardDirection(playerNumber, newDir, timer2.getRatio()))
            {
                timer3.setDuration(ropegrabGrabTime * (1 - timer2.getRatio()));
                timer3.start();
                timer2.reset();
                ropeManager.endRopeGrab();
            }
        }
        else
        {
            timer3.start();
            timer2.reset();
            ropeManager.endRopeGrab();
        }
    }


    private void phase2Attraction()
    {
        if (!timer2.check())
        {
            playerController.velocity = new Vector2(0, 0);
            currentGrabAngle = timer2.getRatio() * maxGrabAngle;
            Vector2 newDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * currentGrabAngle), Mathf.Sin(Mathf.Deg2Rad * currentGrabAngle));

            if (ropeManager.attractPoints(playerNumber, timer2.getRatio()))
            {
                timer3.setDuration(ropegrabGrabTime * (1 - timer2.getRatio()));
                timer3.start();
                timer2.reset();
                ropeManager.endRopeGrab();
            }
        }
        else
        {
            timer3.start();
            timer2.reset();
            ropeManager.endRopeGrab();
        }
    }


    private void phase3PrelagIfCollision()
    {
        if (!timer3.check())
        {
            timer2.reset();
        }
        else
        {
            timer3.reset();
            ropeManager.endRopeGrab();
        }
    }

    override public void cancel()
    {
        ropeManager.endRopeGrab();
        //playerController.velocity = new Vector2(0, 0);
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }

}
