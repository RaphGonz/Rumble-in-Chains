using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * G�re le premier jump, le second jump et le wall jump
 * 
 */

public class Jump : MonoBehaviour
{


    [SerializeField] private float jumpForce;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private Vector2 wallJumpForce;
    private float variableJumpForce;

    private bool onFirstJump;
    private bool onSecondJump;
    private bool onWallJump;
    private bool jumpReleased;

    private float jumpCount;

    private float timeStartJump;
    [SerializeField] private float timeDurationJump;




    // Appel� lorsque la touche de jump est pr�ss�e. Decide du jump � faire et le lance.
    public void StartJump(float playerControllerJumpCount)
    {
        jumpCount = playerControllerJumpCount;

        if (jumpCount == 2)
        {
            StartFirstJump();
        }

        else if (PlayerController.instance.onGrab) //moche mais fait le job, j'utiliserai plutot un getter dans PlayerController pour faire propre #Raphael
        {
            StartWallJump();
        }

        else if (jumpCount == 1)
        {
            StartSecondJump();
        }
    }

    // Appel� � chaque frame par le playerController pendant un jump (onJump). Lance un Update du jump actuel.
    public void UpdateJump()
    {
        if (onFirstJump)
        {
            UpdateFirstJump();
        }
        else if (onSecondJump)
        {
            UpdateSecondJump();
        }
        else if (onWallJump)
        {
            UpdateWallJump();
        }
        else
        {
            PlayerController.instance.onJump = false;
        }
    }

    // Appel� lorsque la touche de jump est relach�e. Indique qu'il faut modifier la taille du premier jump.
    public void JumpRelease()
    {
        jumpReleased = true;
    }


    /*
     *  StartFirstJump, StartSecondJump, StartWallJUmp
     *  lancent leurs jump respectifs.
     */
    private void StartFirstJump()
    {
        PlayerController.instance.jumpCount--;
        PlayerController.instance.onJump = true;

        jumpCount--;
        onFirstJump = true;
        PlayerController.instance.velocity.y = jumpForce;
        variableJumpForce = jumpForce;
        timeStartJump = Time.time;
    }

    private void StartSecondJump()
    {
        PlayerController.instance.jumpCount--;
        PlayerController.instance.onJump = true;

        jumpCount--;
        onFirstJump = false;
        onWallJump = false;
        onSecondJump = true;
        timeStartJump = Time.time;
    }

    private void StartWallJump()
    {
        PlayerController.instance.onJump = true;

        if (PlayerController.instance.grabDirectionx >= 0)
        {
            wallJumpDirection = new Vector2(-1, 0);
        }
        else
        {
            wallJumpDirection = new Vector2(1, 0);
        }
        onWallJump = true;
        onFirstJump = false;
        timeStartJump = Time.time;
    }


    /*
     *  UpdateFirstJump, UpdateSecondJump, UpdateWallJUmp
     *  actualisent leur jump respectif et v�rifie si il est termin�.
     */
    private void UpdateFirstJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onFirstJump = false;
            PlayerController.instance.onJump = false;
            jumpReleased = false;
        }
        else
        {
            PlayerController.instance.velocity.y = variableJumpForce;
        }

        if (jumpReleased && Time.time - timeStartJump > timeDurationJump * 0.5f)
        {
            variableJumpForce = jumpForce * 0.5f;
        }
    }

    private void UpdateSecondJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onSecondJump = false;
            PlayerController.instance.onJump = false;
        }
        else
        {
            PlayerController.instance.velocity.y = jumpForce * 0.7f;
        }
    }

    private void UpdateWallJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onWallJump = false;
            PlayerController.instance.onJump = false;
        }
        else
        {
            PlayerController.instance.velocity = new Vector2(wallJumpDirection.x * wallJumpForce.x, wallJumpForce.y);
        }
    }
}
