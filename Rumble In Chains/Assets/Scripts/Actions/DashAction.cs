using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DashAction : Action
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerAnimation playerAnimation;

    [SerializeField] private float positionDisplacement;

    [SerializeField] private float dashFreezeTime;
    [SerializeField] private float dashMovementTime;
    [SerializeField] private float dashDecelerationTime;
    [SerializeField] private float dashCooldown;

    private Vector2 dashDirection;
    private int playerNumber;

    public ParticleSystem onDashParticles;
    public ParticleSystem dashFocusShort;
    public ParticleSystem dashFocusMedium;
    public ParticleSystem dashFocusLong;
    private ParticleSystem chosenOne;


    private void Start()
    {
        Character character = this.gameObject.layer == 17 ? GameManager.Instance.Character1 : GameManager.Instance.Character2;
        dashFreezeTime = character.characterConverter.convertDashActivation(character.dashActivation);
        positionDisplacement = character.characterConverter.convertDashDistance(character.dashDistance);
        switch (character.dashActivation)
        {
            case DashActivation.LOW: chosenOne = dashFocusShort; break;
            case DashActivation.MEDIUM: chosenOne = dashFocusMedium; break;
            case DashActivation.HIGH: chosenOne = dashFocusLong; break;
        }
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
        chosenOne.Play();
        playerNumber = newNumber;
        playerAnimation.AnimationState = AnimationState.FOCUSDASH;
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
            onDashParticles.Play(); //La phase2 de Dash commence : on lance les particules
            timer1.reset();
            playerController.SetGravityActive(false);
            playerController.SetRopeActive(false);
            playerController.SetDecelerationActive(false);
            playerAnimation.AnimationState = AnimationState.DASH;
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
            onDashParticles.Stop();
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
            playerAnimation.AnimationState = AnimationState.IDLE;
        }
    }

    override public void cancel() {
        playerAnimation.AnimationState = AnimationState.IDLE;
        playerController.velocity = new Vector2(0, 0);
        onDashParticles.Stop(); //On arrête les particules si le move est annule
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
