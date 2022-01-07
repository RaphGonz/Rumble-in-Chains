using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * Gère le premier jump, le second jump et le wall jump
 * 
 */

public class Jump : MonoBehaviour
{
    public PlayerController playerController;

    [SerializeField] private float jumpForce;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private Vector2 wallJumpForce;
    private float variableJumpForce;

    private bool onFirstJump;
    private bool onSecondJump;
    private bool onWallJump;
    private bool jumpReleased;

    [SerializeField] private int jumpCount;

    private float timeStartJump;
    [SerializeField] private float timeDurationJump;




    // Appelé lorsque la touche de jump est préssée. Decide du jump à faire et le lance.
    public void StartJump(int playerControllerJumpCount)
    {
        jumpCount = playerControllerJumpCount;

        if (jumpCount == 2)
        {
            StartFirstJump();
        }

        else if (playerController.onGrab) //moche mais fait le job, j'utiliserai plutot un getter dans PlayerController pour faire propre #Raphael
        {
            StartWallJump();
        }

        else if (jumpCount == 1)
        {
            StartSecondJump();
        }
    }

    // Appelé à chaque frame par le playerController pendant un jump (onJump). Lance un Update du jump actuel.
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
            playerController.onJump = false;
        }
    }

    // Appelé lorsque la touche de jump est relachée. Indique qu'il faut modifier la taille du premier jump.
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
        //playerController.jumpCount--;
        playerController.onJump = true;

        //jumpCount--; //inutile donc commenté
        onFirstJump = true;
        playerController.velocity.y = jumpForce;
        variableJumpForce = jumpForce;
        timeStartJump = Time.time;
    }

    private void StartSecondJump()
    {
        //playerController.jumpCount--;
        playerController.onJump = true;

        //jumpCount--; //inutile donc commenté
        onFirstJump = false;
        onWallJump = false;
        onSecondJump = true;
        timeStartJump = Time.time;
    }

    private void StartWallJump()
    {
        playerController.onJump = true;

        if (playerController.grabDirectionx >= 0)
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
     *  actualisent leur jump respectif et vérifie si il est terminé.
     */
    private void UpdateFirstJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onFirstJump = false;
            playerController.onJump = false;
            jumpReleased = false;
        }
        else
        {
            playerController.velocity.y = variableJumpForce;
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
            playerController.onJump = false;
        }
        else
        {
            playerController.velocity.y = jumpForce * 0.7f;
            if (this.gameObject.layer == 18)
                print(playerController.velocity);
        }
    }

    private void UpdateWallJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onWallJump = false;
            playerController.onJump = false;
        }
        else
        {
            playerController.velocity = new Vector2(wallJumpDirection.x * wallJumpForce.x, wallJumpForce.y);
        }
    }


    public int GetJumpCount()
    {
        return jumpCount;
    }

    public void SetJumpCount(int count)
    {
        jumpCount = count;
    }

    private void Update()
    {

    }
}
