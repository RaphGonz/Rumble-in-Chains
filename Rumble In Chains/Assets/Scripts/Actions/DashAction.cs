using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DashAction : Action
{
    [SerializeField] PlayerController playerController;

    [SerializeField] private float positionDisplacement;

    [SerializeField] private float dashFreezeTime;
    [SerializeField] private float dashMovementTime;
    [SerializeField] private float dashDecelerationTime;
    [SerializeField] private float dashCooldown;

    private Vector2 dashDirection;
    private int playerNumber;



    private void Start()
    {
        Character character = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + (this.gameObject.layer == 17 ? GameManager.Instance.characterPlayer1 : GameManager.Instance.characterPlayer2) + ".asset");
        dashFreezeTime = character.characterConverter.convertDashActivation(character.dashActivation);
        positionDisplacement = character.characterConverter.convertDashDistance(character.dashDistance);


        timer1.setDuration(dashFreezeTime);
        timer2.setDuration(dashMovementTime);
        timer3.setDuration(dashDecelerationTime);
        cooldown.setDuration(dashCooldown);
    }

    public void start(Vector2 direction, int newNumber)
    {
        this.dashDirection = direction;
        timer1.start();
        cooldown.start();
        print("dashStart");
        playerNumber = newNumber;
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
            EventManager.Instance.OnEventDash(playerNumber);
            phase2Movement();
            return false;
        }
        else if (timer3.isActive())
        {
            EventManager.Instance.OnEventDash(playerNumber);
            phase3Deceleration();
            return false;
        }

        return true;
    }

    private void phase1Freeze()
    {
        if (!timer1.check())
        {
            //playerController.velocity = new Vector2(0, 0);
        }
        else
        {
            timer2.start();
            timer1.reset();
            playerController.SetGravityActive(false);
            playerController.SetRopeActive(false);
            playerController.SetDecelerationActive(false);
        }
    }

    private void phase2Movement()
    {
        if (!timer2.check())
        {
            playerController.velocity = positionDisplacement / dashMovementTime * dashDirection;
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
            playerController.velocity = new Vector2(0,0);
        }
    }

    override public void cancel() {
        playerController.velocity = new Vector2(0, 0);
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}