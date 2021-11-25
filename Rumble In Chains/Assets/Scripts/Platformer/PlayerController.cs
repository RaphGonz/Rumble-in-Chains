using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int facing;

    private PlayerCollider playerCollider;
    private Dash dashManager;
    private Jump jumpManager;

    public Vector2 positionBeforeCollider;

    public Vector2 position;
    private Vector2 lastPosition;
    public Vector2 velocity;

    
    public bool bottomDirectionLocked;
    public bool topDirectionLocked;
    public bool leftDirectionLocked;
    public bool rightDirectionLocked;

    // Jump
    public int jumpCount;
    public bool onJump;


    // grab
    public bool onGrab;
    public float grabFallingSpeed;
    public float grabDirectionx;


    public float gravity = -100f;

    private float minVelocity = 0.1f;
    
    public float groundAcceleration;
    public float walkSpeed;
    public float sprintSpeed;
    public float actualMaxSpeed;
    public float maxYspeed;

    public float groundProportionalDecelerationX;
    public float airProportionalDecelerationX;
    public float groundFlatDecelerationX;
    public float airFlatDecelerationX;
    private float flatDecelerationX;
    private float proportionalDecelerationX;



    public float timeDivider = 0.001f;

    public bool onDash = false;
    public bool canDash = true;


    int i = 0;

   

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        positionBeforeCollider = transform.position;
        actualMaxSpeed = walkSpeed;
        playerCollider = GetComponent<PlayerCollider>();
        dashManager = GetComponent<Dash>();
        jumpManager = GetComponent<Jump>();
        facing = GetComponent<InputManager>().direction.x > 0 ? 1 : -1;
    }

    public void ApplyNewPosition()
    {
        transform.position = position;
    }

    // Pour mettre à jour la position et la velocity du player
    public void UpdatePlayerVelocityAndPosition()
    {
 
        UpdateVelocity(); //ça modifie le vecteur velocité
        UpdatePosition(); //ca calcule la prochaine position idéal (sans collider)
        UpdatePositionInRegardsOfCollision();
        if (GetComponent<InputManager>().direction.x > 0) {
            facing = 1;
        }
        if(GetComponent<InputManager>().direction.x < 0)
        {
            facing = -1;
        }
    }

    

    private void UpdateVelocity()
    {

        Vector2 deceleration = new Vector2(ComputeDecelerationX(), 0); //Vecteur vertical ! Possible de faire une deceleration sur y aussi, a tester

        // pas de gravité pendant le dash
        if (onDash)
        {
            dashManager.UpdateDash();
        }
        else
        {
            if (velocity.y > -maxYspeed)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            
        }

        if (onJump)
        {
            jumpManager.UpdateJump();
        }
        /*
        else if (onSecondJump)
        {
            UpdateSecondJump();
        }
        else if (onWallJump)
        {
            UpdateWallJump();
        }
        */

        if (onGrab)
        {
            velocity.y = -grabFallingSpeed;
        }


        velocity += deceleration * Time.deltaTime;
        

        /*
        if (bottomDirectionLocked && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else if (topDirectionLocked && velocity.y > 0)
        {
            velocity.y = 0;
        }

        if (leftDirectionLocked && velocity.x < 0)
        {
            velocity.x = 0;
        }
        else if (rightDirectionLocked && velocity.x > 0)
        {
            velocity.x = 0;
        }
        */
    }

    private void UpdatePosition() //Positions idéales
    {
        position += velocity * Time.deltaTime;

    }

    public void UpdatePositionInRegardsOfCollision() //Modif la prochaine positions en fonctions des colliders qu'on touche, pour qu'on en sorte
    {
        Vector2 movement = position - new Vector2(positionBeforeCollider.x, positionBeforeCollider.y);

        playerCollider.UpdateCollisions(ref movement);

        position = new Vector3(movement.x + positionBeforeCollider.x, movement.y + positionBeforeCollider.y, 0);
        positionBeforeCollider = position;
    }

    




    /*
    public void MoveRight()
    {
        MoveX(new Vector2(1, 0));
    }

    public void MoveLeft()
    {
        MoveX(new Vector2(-1, 0));
    }
    */

    public void MoveX(float directionx) //On lui donne 1 ou -1 selon si on va a droite ou a gauche
    {
        float speedAugmentation = groundAcceleration * Mathf.Sign(directionx) * Time.deltaTime;
        
        if (Mathf.Abs(speedAugmentation) > actualMaxSpeed - Mathf.Abs(velocity.x))
        {
            velocity.x = actualMaxSpeed * Mathf.Sign(directionx);
        }
        else
        {
            velocity.x += speedAugmentation;
        }
        Grab(directionx); //On transmet la direction a Grab qui va gérer l'accroche au murs (si on est bien accroché ou non)
    }

    private float ComputeDecelerationX()
    {
        // deceleration au sol
        if (bottomDirectionLocked)
        {
            flatDecelerationX = groundFlatDecelerationX;
            proportionalDecelerationX = groundProportionalDecelerationX;
        }
        // deceleration dans les airs
        else
        {
            flatDecelerationX = airFlatDecelerationX;
            proportionalDecelerationX = airProportionalDecelerationX;
        }

        if (onDash)
        {
            return 0;
        }


        if (velocity.x > minVelocity)
        {
            return (-proportionalDecelerationX * velocity.x - flatDecelerationX);
        }
        else if (velocity.x >= -minVelocity)
        {
            velocity.x = 0;
            return 0;
        }
        else
        {
            return (-proportionalDecelerationX * velocity.x + flatDecelerationX);
        }
    }

    internal void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
    }

    public void GroundTouched()
    {
        jumpCount = 2;
        if (!onDash)
        {
            canDash = true;
        }
    }

    // JumpButtonPressed
    public void Jump()
    {
        jumpManager.StartJump(jumpCount);
    }

    // JumpButtonReleased
    public void JumpRelease()
    {
        jumpManager.JumpRelease();
    }

    // DashButtonPressed
    public void Dash(Vector2 dashDirection)
    {
        if (canDash)
        {
            dashManager.StartDash(dashDirection);
            canDash = false;
            onDash = true;
        }
    }


    public void Grab(float direction)
    {
        if (leftDirectionLocked && direction < 0 && !onJump && velocity.y < 0)
        {
            onGrab = true;
            grabDirectionx = direction;
        }
        else if (rightDirectionLocked && direction > 0 && !onJump && velocity.y < 0)
        {
            onGrab = true;
            grabDirectionx = direction;
        }
        else
        {
            onGrab = false;
        }
    }

    

    public void Sprint()
    {
        actualMaxSpeed = sprintSpeed;
    }

    public void StopSprinting()
    {
        actualMaxSpeed = walkSpeed;
    }
}
