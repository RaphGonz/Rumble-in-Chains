using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] private float jumpCooldown = 0.5f;
    private float timeStartJump = 0.0f;



    // grab
    public bool onGrab;
    public float grabFallingSpeed;
    public float grabDirectionx;


    private float gravity = -100f;
    [SerializeField] private float minGravity = 300f;
    [SerializeField] private float maxGravity = 150f;

    private float minVelocity = 0.1f;
    
    public float groundAcceleration;
    public float walkSpeed;
    public float sprintSpeed;
    public float actualMaxSpeed;
    public float maxYspeed;


    private float characterSpeedMultiplier;
    private float characterWeight;

    private float maxDownSpeed = 10;
    [SerializeField] float normalMaxDownSpeed = 10;
    [SerializeField] float downMaxDownSpeed = 20;

    public float groundProportionalDecelerationX;
    public float airProportionalDecelerationX;
    public float groundFlatDecelerationX;
    public float airFlatDecelerationX;
    private float flatDecelerationX;
    private float proportionalDecelerationX;



    public float timeDivider = 0.001f;

    public bool onDash = false;
    public bool onDashCooldown = false;
    public bool canDash = true;
    public bool grounded = true;
    public bool hit = false;
    public int counterFramesHit = 3;

    public bool immobile = false;
    public bool onImmobilization = false;
    public float immobileDuration = 1.0f;
    public float immobileCooldown = 0.5f;
    public float timeImmobileStart = 0;


    private PlayerState playerState = PlayerState.NORMAL;
    private bool ropeActive = true;
    private bool gravityActive = true;
    private bool decelerationActive = true;


    int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        positionBeforeCollider = transform.position;
        actualMaxSpeed = walkSpeed;
        gravity = minGravity;
        
        playerCollider = GetComponent<PlayerCollider>();
        /*
        dashManager = GetComponent<Dash>();
        jumpManager = GetComponent<Jump>();
        */
        facing = GetComponent<InputManager>().direction.x > 0 ? 1 : -1;


        Character character = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + (this.gameObject.layer == 17 ? GameManager.Instance.characterPlayer1 : GameManager.Instance.characterPlayer2) + ".asset");
        characterSpeedMultiplier = character.characterConverter.convertSpeed(character.speed);
        
        characterWeight = character.characterConverter.convertWeight(character.weight);
    }

    public void ApplyNewPosition()
    {
        if (!immobile)
        {
            transform.position = position;
        }
        
    }

    // Pour mettre à jour la position et la velocity du player
    public void UpdatePlayerVelocityAndPosition()
    {
        //UpdateActions();
        if (!immobile)
        {
            UpdateVelocity(); //ça modifie le vecteur velocité
            UpdatePosition(); //ça calcule la prochaine position idéal (sans collider)
        }
        UpdatePositionInRegardsOfCollision();

        if (GetComponent<InputManager>().direction.x > 0) {
            facing = 1;
        }
        if(GetComponent<InputManager>().direction.x < 0)
        {
            facing = -1;
        }
    }

    private void UpdateActions()
    {
        // pas de gravité pendant le dash
        if (onDash || onDashCooldown)
        {
            dashManager.UpdateDash();
            if (onDashCooldown)
            {
                if (velocity.y > -maxYspeed)
                {
                    velocity.y += gravity * Time.deltaTime;
                }
            }
        }
        else
        {

            if (gameObject.name == "PlayerRight" && hit)
            {
                Debug.Break();
                print(velocity); 
            }

            if (velocity.y > -maxYspeed)
            {
                velocity.y += gravity * Time.deltaTime;
            }
        }

        if(playerState == PlayerState.NORMAL && velocity.y > -maxYspeed)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (onJump)
        {
            jumpManager.UpdateJump();
        }

        if (onImmobilization)
        {
            UpdateImmobilisation();
        }


        if (onGrab)
        {
            velocity.y = -grabFallingSpeed;
        }

    }

    private void UpdateVelocity()
    {
        if (gravityActive)
        {
            if (velocity.y > -maxDownSpeed)
            {
                velocity.y -= gravity * characterWeight * Time.deltaTime;
            }
        }



        if (decelerationActive)
        {
            Vector2 deceleration = new Vector2(ComputeDecelerationX(), 0); //Vecteur vertical ! Possible de faire une deceleration sur y aussi, a tester
            velocity += deceleration * Time.deltaTime;
        }
    }

    private void UpdatePosition() //Positions idéales
    {
        position += velocity * Time.deltaTime;

    }

    public void UpdatePositionInRegardsOfCollision() //Modif la prochaine positions en fonctions des colliders qu'on touche, pour qu'on en sorte
    {
        Vector2 movement = position - new Vector2(positionBeforeCollider.x, positionBeforeCollider.y);

        playerCollider.UpdateCollisions(ref movement, positionBeforeCollider);

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
        if (playerState == PlayerState.NORMAL || playerState == PlayerState.JUMP)
        {
            float speedAugmentation = groundAcceleration * characterSpeedMultiplier * Mathf.Sign(directionx) * Time.deltaTime;

            if (Mathf.Abs(speedAugmentation) > actualMaxSpeed * characterSpeedMultiplier - Mathf.Abs(velocity.x))
            {
                velocity.x = actualMaxSpeed * characterSpeedMultiplier * Mathf.Sign(directionx);
            }
            else
            {
                velocity.x += speedAugmentation;
            }
            Grab(directionx); //On transmet la direction a Grab qui va gérer l'accroche au murs (si on est bien accroché ou non)
        }
    }

    public void MoveDown(bool value)
    {
        if (value)
        {
            gravity = maxGravity * characterSpeedMultiplier;
        }
        else
        {
            gravity = minGravity;
        }
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
        if (ropeActive)
        {
            position = newPosition;
        }
    }

    public void GroundTouched()
    {
        grounded = true;
        jumpCount = 2;
        if (!onDash)
        {
            canDash = true;
        }
    }

    // JumpButtonPressed
    public void Jump()
    {
        grounded = false;
        if (Time.time - timeStartJump > jumpCooldown)
        {

            jumpManager.StartJump(jumpCount);
            timeStartJump = Time.time;
        }
        
    }

    // JumpButtonReleased
    public void JumpRelease()
    {
        jumpManager.JumpRelease();
    }

    // DashButtonPressed
    public void Dash(Vector2 dashDirection)
    {
        if (playerState == PlayerState.NORMAL)
        {
            dashManager.StartDash(dashDirection);
        }
    }

    public void DashForced(Vector2 dashDirection)
    {
        dashManager.resetDash();
        dashManager.StartDash(dashDirection);
    }


    public void Grab(float direction)
    {
        if (playerState == PlayerState.NORMAL)
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
    }

    

    public void Sprint()
    {
        if (playerState == PlayerState.NORMAL || playerState == PlayerState.JUMP)
        {
            actualMaxSpeed = sprintSpeed;
        }
    }

    public void StopSprinting()
    {
        actualMaxSpeed = walkSpeed;
    }

    public void ImmobilizePlayer()
    {
        if (!onImmobilization)
        {
            playerState = PlayerState.SHIELD;
            immobile = true;
            onImmobilization = true;
            timeImmobileStart = Time.time;
        }
    }

    private void UpdateImmobilisation()
    {
        if (immobile)
        {
            if (Time.time - timeImmobileStart > immobileDuration)
            {
                immobile = false;
                timeImmobileStart = Time.time;
            }
        }
        else
        {
            if (Time.time - timeImmobileStart > immobileCooldown)
            {
                playerState = PlayerState.NORMAL;
                onImmobilization = false;
            }
        }
    }

    public void Stun()
    {

    }

    public void RopeGrab(Vector2 grabDirection)
    {

    }

    public void Shield()
    {

    }


    public void SetPlayerState(PlayerState newPlayerState)
    {
        playerState = newPlayerState;
    }

    public void SetRopeActive(bool newRopeActive)
    {
        ropeActive = newRopeActive;
    }

    public void SetGravityActive(bool newGravityActive)
    {
        gravityActive = newGravityActive;
    }
    public void SetDecelerationActive(bool newDecelerationActive)
    {
        decelerationActive = newDecelerationActive;
    }
}
