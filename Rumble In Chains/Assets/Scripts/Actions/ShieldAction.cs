using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : Action
{
    [SerializeField] PlayerController playerController;

    [SerializeField] private float shieldTime;
    [SerializeField] private float shieldCooldown;


    private void Start()
    {
        timer1.setDuration(shieldTime);
        cooldown.setDuration(shieldCooldown);
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
            phase1Shield();
            return false;
        }


        return true;
    }

    private void phase1Shield()
    {
        if (!timer1.check())
        {
            playerController.velocity = new Vector2(0, 0);
        }
        else
        {
            timer1.reset();
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
