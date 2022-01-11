using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [SerializeField] CharacterController characterController;

    [SerializeField] private float jumpVelocity;

    [SerializeField] private float attackTotalTime;
    [SerializeField] private float attackCooldown;

    private Vector2 joystickDirection;
    private AttackType attackType = AttackType.Jab;


    private void Start()
    {
        timer1.setDuration(attackTotalTime);
        cooldown.setDuration(attackCooldown);
        EventManager.Instance.eventSound += TestEvent;
        EventManager.Instance.eventParticle += TestEventParticle;
    }

    private void TestEvent(string testString)
    {
        Debug.Log("AttackAction recieved the Event with string parameter : " + testString);
    }

    private void TestEventParticle(Vector2 pos, string testString)
    {
        Debug.Log("AttackAction recieved the Event with parameters : position = (" + pos.x + "; " + pos.y + ");  string = " + testString);
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
        }
        else if (joystickDirection.x == -1)
        {
            attackType = AttackType.SideTilt;
        }
        else if (joystickDirection.y == 1)
        {
            attackType = AttackType.UpTilt;
        }
        else if (joystickDirection.y == -1)
        {
            attackType = AttackType.DownTilt;
        }
        else
        {
            attackType = AttackType.Jab;
        }
    }


    override public void cancel()
    {
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
