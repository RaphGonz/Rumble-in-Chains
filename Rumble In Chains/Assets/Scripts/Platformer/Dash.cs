using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public PlayerController playerController;

    private Vector2 dashDirection;

    private float timeStartDash;


    [SerializeField] private float positionDisplacement;

    [SerializeField] private float timeFreezeOnDash;
    [SerializeField] private float timeInsideDash;
    [SerializeField] private float timeDecelerationAfterDash;



    private void Start()
    {
        dashDirection = new Vector2(0, 0);
    }

    public void StartDash(Vector2 dashDirection)
    {
        if (playerController.canDash)
        {
            if (dashDirection == new Vector2(0, 0))
            {
                dashDirection = Vector2.right;
            }
            playerController.onDash = true;
            playerController.canDash = false;
            this.dashDirection = dashDirection;
            timeStartDash = Time.time;
        }
    }

    public void UpdateDash()
    {
        float currentTime = Time.time - timeStartDash;

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
            playerController.onDash = false;
        }
    }

    private float GetCurrentDashSpeed()
    {
        float currentDashSpeed = positionDisplacement / timeInsideDash;
        return currentDashSpeed;
    }
}
