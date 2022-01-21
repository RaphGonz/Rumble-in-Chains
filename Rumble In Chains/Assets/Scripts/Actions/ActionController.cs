using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PlayerState
{
    NORMAL,
    JUMP,
    DASH,
    ATTACK,
    STUN,
    ROPEGRAB,
    SHIELD,
    EXPEL,
    POSING,
    COUNT
}




public class ActionController : MonoBehaviour
{
    [SerializeField] private PlayerState playerState = PlayerState.NORMAL;
    private Vector2 joystickDirection;
    private Joystick joystick;
    private AttackType attackType = AttackType.Jab;

    private bool canDash = true;
    private bool canJump = true;
    private bool shieldActive = false;
    private bool invincible = false;
    private bool inZone = false;
    


    private float invincibilityTime;

    private AnimationState animationState;



    private bool inRecoveryFrames;

    Timer invincibilityTimer;
    Timer stunTimer;


    [SerializeField] JumpAction jumpAction;
    [SerializeField] DashAction dashAction;
    [SerializeField] ShieldAction shieldAction;
    [SerializeField] RopegrabAction ropegrabAction;
    [SerializeField] AttackAction attackAction;
    [SerializeField] ExpelAction expelAction;
    [SerializeField] PublicAction publicAction;

    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerCollider playerCollider;
    [SerializeField] BufferManager buffer;

    [SerializeField] PlayerAnimation playerAnimation;

    [SerializeField] public int playerNumber;
    [SerializeField] private float stunInvincibilityRatio;
    [SerializeField] private float maxInvincibilityTime;

    [SerializeField] private int shieldBrokenStunFrames = 60;
    [SerializeField] private int stopRopegrabStunFrames = 60;

    public ParticleSystem stunParticles1;
    public ParticleSystem stunParticles2;

    public ParticleSystem dustParticles;
    //On ne peut pas faire .Play() à chaque update donc puisqu'on ne peut le faire qu'une fois il faut un booléen pour l'implémenter
    private bool movingOnGround = false;


    private void Start()
    {
        invincibilityTimer = new Timer();
        stunTimer = new Timer();

        EventManager.Instance.eventDash += EventEnemyDash;
        EventManager.Instance.eventRopegrab += EventEnemyRopegrab;
        EventManager.Instance.eventPlayerInZone += EventPlayerZone;

    }



    public void UpdateActions()
    {
        joystick = buffer.getJoystick();
        joystickDirection = joystick.GetDirection();

        LaunchNewAction();

        UpdateCurrentAction();


        if (joystickDirection.x != 0)
        {
            MoveX();
        }
        if (joystick.getFilter4().y < 0)
        {
            MoveDown(true);
        }
        else
        {
            MoveDown(false);
        }

        playerAnimation.UpdateAnimator();
    }

    private bool Attack()
    {
        if (GetPriority(PlayerState.ATTACK) && attackAction.getCooldown())
        {
            CancelCurrentAction();
            changeState(PlayerState.ATTACK);
            attackAction.start(joystick.getFilter4(), playerCollider.IsGrounded());
            return true;
        }
        return false;
    }

    private bool Dash()
    {
        if (canDash && GetPriority(PlayerState.DASH) && dashAction.getCooldown())
        {
            CancelCurrentAction();
            //EventManager.Instance.OnEventDash(playerNumber);
            dashAction.start(joystick.getFilter8(), playerNumber);
            changeState(PlayerState.DASH);
            return true;
        }
        return false;
    }

    private bool Jump()
    {
        if (canJump && GetPriority(PlayerState.JUMP) && jumpAction.getCooldown())
        {
            CancelCurrentAction();
            jumpAction.start();
            changeState(PlayerState.JUMP);
            return true;
        }
        return false;
    }

    

    private bool RopeGrab()
    {
        if (GetPriority(PlayerState.ROPEGRAB) && ropegrabAction.getCooldown())
        {
            CancelCurrentAction();
            //EventManager.Instance.OnEventRopegrab(playerNumber);
            ropegrabAction.start(joystick.GetDirection(), playerNumber);
            changeState(PlayerState.ROPEGRAB);
            return true;
        }
        return false;
    }

    private bool Shield()
    {
        if (GetPriority(PlayerState.SHIELD) && shieldAction.getCooldown())
        {
            CancelCurrentAction();
            shieldAction.start();
            changeState(PlayerState.SHIELD);
            return true;
        }
        return false;
    }

    public void ExpelAndStun(Vector2 direction, float stunFrames)
    {
        //Debug.Log("player Expel&Stun");
        float stunTime = stunFrames / 60;
        invincibilityTime = stunTime * stunInvincibilityRatio;
        if (invincibilityTime > maxInvincibilityTime)
        {
            invincibilityTime = maxInvincibilityTime;
        }
        invincibilityTimer.setDuration(invincibilityTime);

        stunTimer.setDuration(stunTime);
        stunTimer.start();

        CancelCurrentAction();

        playerAnimation.Trigger("HitTrigger");

        expelAction.start(direction);
        changeState(PlayerState.EXPEL);
    }

    public bool Stun(float stunFrames)
    {
        if (playerState == PlayerState.ATTACK)
        {
            characterController.InterruptAttack();
        }
        CancelCurrentAction();
        playerAnimation.Trigger("HitTrigger");
        //playerController.Stun();

        if (stunFrames < 3) stunFrames = 3;
        float stunTime = stunFrames / 60;
        stunTimer.setDuration(stunTime);
        stunTimer.start();

        invincibilityTime = stunTime * stunInvincibilityRatio;
        if (invincibilityTime > maxInvincibilityTime)
        {
            invincibilityTime = maxInvincibilityTime;
        }
        invincibilityTimer.setDuration(invincibilityTime);

        changeState(PlayerState.STUN);
        
        return true;
    }


    private void MoveX()
    {
        if (playerState == PlayerState.NORMAL || playerState == PlayerState.JUMP)
        {
            if (playerCollider.IsGrounded() && !Mathf.Approximately(playerController.velocity.x,0) )
            {
                if (!movingOnGround)
                {
                    movingOnGround = true;
                    dustParticles.Play();
                }
                
            }
            else
            {
                movingOnGround = false;
                dustParticles.Stop();
            }
            
            playerController.MoveX(joystickDirection.x);
        }
    }

    private void MoveDown(bool value)
    {
        if (playerState == PlayerState.NORMAL)
        {
            playerController.MoveDown(value);
        }
        else
        {
            playerController.MoveDown(false);
        }
    }

    private bool TakePose()
    {
        if (playerCollider.IsGrounded() && GetPriority(PlayerState.POSING) && publicAction.getCooldown())
        {
            CancelCurrentAction();
            changeState(PlayerState.POSING);
            publicAction.start();
            return true;
        }
        return false;
    }

    public bool isInvincible()
    {
        return invincible;
    }

    public bool isShieldActive()
    {
        return shieldActive;
    }

    private void LaunchNewAction()
    {
        InputButtons input = buffer.getBufferElement();
        if (input != InputButtons.NULL)
        {
            //print(playerState);
            bool popBuffer = false;
            switch (input)
            {
                case InputButtons.ATTACKBUTTON:
                    if (inZone)
                    {
                        popBuffer = TakePose();
                    }
                    else
                    {
                        popBuffer = Attack();
                    }
                    break;
                case InputButtons.DASHBUTTON:
                    popBuffer = Dash();
                    break;
                case InputButtons.JUMPBUTTON:
                    popBuffer = Jump();
                    break;
                case InputButtons.ROPEBUTTON:
                    popBuffer = RopeGrab();
                    break;
                case InputButtons.SHIELDBUTTON:
                    popBuffer = Shield();
                    break;
            }

            if (popBuffer)
            {
                buffer.popBuffer();
            }
        }
    }

    private void UpdateCurrentAction()
    {   
        if (playerState != PlayerState.NORMAL)
        {
            bool terminated = false;

            switch (playerState)
            {
                case PlayerState.JUMP:
                    terminated = jumpAction.update();
                    break;
                case PlayerState.DASH:
                    terminated = dashAction.update();
                    break;
                case PlayerState.SHIELD:
                    terminated = shieldAction.update();
                    break;
                case PlayerState.ROPEGRAB:
                    terminated = ropegrabAction.update();
                    break;
                case PlayerState.ATTACK:
                    terminated = attackAction.update();
                    break;
                case PlayerState.EXPEL:
                    terminated = expelAction.update();
                    break;
                case PlayerState.POSING:
                    terminated = publicAction.update();
                    break;
                case PlayerState.STUN:
                    terminated = stunTimer.check();
                    break;
            }

            if (terminated)
            {
                if (playerState == PlayerState.STUN)
                {
                    stunTimer.reset();
                    invincibilityTimer.start();
                    print("jsuis plus stunned frero");
                    print(stunParticles1.isPlaying);
                    stunParticles1.Stop();
                    stunParticles2.Stop(); //On arrête les particules
                    changeState(PlayerState.NORMAL);
                }
                else if (playerState == PlayerState.EXPEL)
                {
                    if (stunTimer.isActive())
                    {
                        changeState(PlayerState.STUN);
                    }
                    else
                    {
                        changeState(PlayerState.NORMAL);
                    }
                }
                else
                {
                    changeState(PlayerState.NORMAL);
                }
            }
        }

        if (stunTimer.isActive())
        {
            if (stunTimer.check())
            {
                changeState(PlayerState.NORMAL);
                stunTimer.reset();
            }
            else
            {

                invincibilityTimer.start();
            }
        }

        if (invincibilityTimer.isActive())
        {
            if (!invincibilityTimer.check())
            {
                invincible = true;
                playerAnimation.SetBool("IsInvincible", true);
            }
            else
            {
                invincible = false;
                playerAnimation.SetBool("IsInvincible", false);
            }
        }
    }

    

    private void CancelCurrentAction()
    {
        if (playerState != PlayerState.NORMAL)
        {
            switch (playerState)
            {
                case PlayerState.JUMP:
                    jumpAction.cancel();
                    break;
                case PlayerState.DASH:
                    dashAction.cancel();
                    break;
                case PlayerState.SHIELD:
                    shieldAction.cancel();
                    break;
                case PlayerState.ROPEGRAB:
                    ropegrabAction.cancel();
                    break;
                case PlayerState.ATTACK:
                    attackAction.cancel();
                    break;
                case PlayerState.EXPEL:
                    attackAction.cancel();
                    break;
                case PlayerState.POSING:
                    attackAction.cancel();
                    break;
            }
        }
    }


    public void changeState(PlayerState newState)
    {
        playerState = newState;

        switch (newState)
        {
            case PlayerState.NORMAL:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                playerController.SetDecelerationActive(true);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.JUMP:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(true);
                playerController.SetDecelerationActive(true);
                shieldActive = false;
                invincible = false;
                animationState = AnimationState.JUMP;
                break;
            case PlayerState.DASH:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                playerController.SetDecelerationActive(true);
                shieldActive = false;
                invincible = false;
                animationState = AnimationState.FOCUSDASH;
                break;
            case PlayerState.ATTACK:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                playerController.SetDecelerationActive(true);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.ROPEGRAB:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(false);
                playerController.SetDecelerationActive(true);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.SHIELD:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(false);
                playerController.SetDecelerationActive(true);
                shieldActive = true;
                invincible = false;
                break;
            case PlayerState.STUN:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                playerController.SetDecelerationActive(false);
                shieldActive = false;
                invincible = true;
                print("Yo je suis stun mec");
                stunParticles1.Play(); //On est stun donc on lance les particules
                stunParticles2.Play();
                break;
            case PlayerState.EXPEL:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(false);
                playerController.SetDecelerationActive(false);
                shieldActive = false;
                invincible = true;
                break;
            case PlayerState.POSING:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                playerController.SetDecelerationActive(true);
                shieldActive = false;
                invincible = false;
                break;
        }
    }



    private bool GetPriority(PlayerState newState)
    {
        switch (playerState)
        {
            case PlayerState.NORMAL:
                return true;
                break;
            case PlayerState.JUMP:
                if (newState == PlayerState.STUN) return true;
                break;
            case PlayerState.DASH:
                if (newState == PlayerState.STUN) return true;
                break;
            case PlayerState.ATTACK:
                if (newState == PlayerState.STUN) return true;
                break;
            case PlayerState.ROPEGRAB:
                if (newState == PlayerState.STUN) return true;
                break;
            case PlayerState.SHIELD:
                if (newState == PlayerState.STUN) return true;
                break;
            case PlayerState.POSING:
                if (newState == PlayerState.STUN) return true;
                break;
            case PlayerState.STUN:
                if (newState == PlayerState.STUN) return true;
                break;
        }
        return false;
    }

    /*

    private Vector2 getCurrentDirection()
    {
        Vector2 direction = new Vector2();

        switch (joystickDirection)
        {
            case JoystickDirection.LEFT:
                direction.x = -1;
                break;
            case JoystickDirection.RIGHT:
                direction.x =  1;
                break;
            case JoystickDirection.DOWN:
                direction.y = -1;
                break;
            case JoystickDirection.UP:
                direction.y =  1;
                break;
            case JoystickDirection.DOWNLEFT:
                direction.x = -1;
                direction.y = -1;
                break;
            case JoystickDirection.DOWNRIGHT:
                direction.x =  1;
                direction.y = -1;
                break;
            case JoystickDirection.UPLEFT:
                direction.x = -1;
                direction.y =  1;
                break;
            case JoystickDirection.UPRIGHT:
                direction.x =  1;
                direction.y =  1;
                break;
        }

        return direction;
    }
    */

    private void EventEnemyRopegrab(int number)
    {
        if (number != playerNumber && playerState == PlayerState.SHIELD)
        {
            CancelCurrentAction();
            Stun(shieldBrokenStunFrames);
        }
    }

    private void EventEnemyDash(int number)
    {
        if (number != playerNumber && playerState == PlayerState.ROPEGRAB)
        {
            CancelCurrentAction();
            Stun(stopRopegrabStunFrames);
        }
    }

    private void EventPlayerZone(int number, bool value)
    {
        if (number == playerNumber)
        {
            inZone = value;
        }
    }
}



