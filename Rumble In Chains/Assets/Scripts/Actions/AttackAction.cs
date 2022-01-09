using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [SerializeField] CharacterController characterController;

    [SerializeField] private float jumpVelocity;

    [SerializeField] private float attackTotalTime;
    [SerializeField] private float attackCooldown;


    private void Start()
    {
        timer1.setDuration(attackTotalTime);
        cooldown.setDuration(attackCooldown);
    }


    // Start is called before the first frame update
    override public void start()
    {
        timer1.start();
        cooldown.start();
        characterController.Attack(AttackType.Jab);
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

        return true;
    }


    override public void cancel()
    {
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
