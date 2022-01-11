using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public PlayerController playerController;

    private Vector2 dashDirection;

    private float timeStart;
    //private bool inCooldwon = false;


    [SerializeField] private float positionDisplacement;

    [SerializeField] private float timeFreezeOnDash;
    [SerializeField] private float timeInsideDash;
    [SerializeField] private float timeDecelerationAfterDash;
    [SerializeField] private float timeCooldown = 1.0f;



    private void Start()
    {
        dashDirection = new Vector2(0, 0);
    }

    public void StartDash(Vector2 dashDirection)
    {
        if (playerController.canDash && !playerController.onDashCooldown)
        {
            if (dashDirection == new Vector2(0, 0))
            {
                dashDirection = Vector2.right;
            }
            playerController.onDash = true;
            playerController.canDash = false;
            this.dashDirection = dashDirection;
            timeStart = Time.time;
        }
    }

    public void UpdateDash()
    {
        float currentTime = Time.time - timeStart;
        if (!playerController.onDashCooldown)
        {
            if (currentTime < timeFreezeOnDash)
            {
                playerController.velocity = new Vector2(0, 0);
            }
            else if (currentTime < timeFreezeOnDash + timeInsideDash)
            {
                playerController.velocity = GetCurrentDashSpeed() * dashDirection;
            }

            else if (currentTime < timeFreezeOnDash + timeInsideDash + timeDecelerationAfterDash)
            {
                playerController.velocity = dashDirection * playerController.actualMaxSpeed;
            }

            else
            {
                playerController.onDashCooldown = true;
                playerController.onDash = false;
                timeStart = Time.time;
            }
        }
        else
        {
            if (currentTime > timeCooldown)
            {
                playerController.onDashCooldown = false;
            }
        }
        
    }

    private float GetCurrentDashSpeed()
    {
        float currentDashSpeed = positionDisplacement / timeInsideDash;
        return currentDashSpeed;
    }

    public void resetDash()
    {
        playerController.onDashCooldown = false;
        playerController.canDash = true;
    }
}
