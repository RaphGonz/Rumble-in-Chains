using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [SerializeField] CharacterController characterController;

    [SerializeField] private float attackTotalTime;
    [SerializeField] private float attackCooldown;

    private Vector2 joystickDirection;
    private AttackType attackType = AttackType.Jab;


    private void Start()
    {
        timer1.setDuration(attackTotalTime);
        cooldown.setDuration(attackCooldown);
    }


    // Start is called before the first frame update
    public void start(Vector2 direction)
    {
        joystickDirection = direction;
        timer1.start();
        cooldown.start();
        selectAttack();
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

    private void selectAttack()
    {
        if (joystickDirection.x == 1)
        {
            attackType = AttackType.SideTilt;
            Debug.Log("sideTilt");
        }
        else if (joystickDirection.x == -1)
        {
            attackType = AttackType.SideTilt;
            Debug.Log("sideTilt");
        }
        else if (joystickDirection.y == 1)
        {
            attackType = AttackType.UpTilt;
            Debug.Log("UpTilt");
        }
        else if (joystickDirection.y == -1)
        {
            attackType = AttackType.DownTilt;
            Debug.Log("DownTilt");
        }
        else
        {
            attackType = AttackType.Jab;
            Debug.Log("Jab");
        }
    }


    override public void cancel()
    {
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
