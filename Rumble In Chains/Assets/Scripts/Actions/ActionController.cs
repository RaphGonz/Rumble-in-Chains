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
    COUNT
}




public class ActionController : MonoBehaviour
{
    private PlayerState playerState = PlayerState.NORMAL;
    private Vector2 joystickDirection;
    private Joystick joystick;
    private AttackType attackType = AttackType.Jab;

    private bool canDash = true;
    private bool canJump = true;
    private bool shieldActive = false;
    private bool invincible = false;



    [SerializeField] JumpAction jumpAction;
    [SerializeField] DashAction dashAction;
    [SerializeField] ShieldAction shieldAction;
    [SerializeField] RopegrabAction ropegrabAction;
    [SerializeField] AttackAction attackAction;
    [SerializeField] ExpelAction expelAction;

    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerController playerController;
    [SerializeField] BufferManager buffer;




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
        if (joystickDirection.y < 0)
        {
            MoveDown();
        }
    }

    private bool Attack()
    {
        if (GetPriority(PlayerState.ATTACK))
        {
            CancelCurrentAction();
            attackAction.start(joystick.getFilter4());
            return true;
        }
        return false;
    }

    private bool Dash()
    {
        if (canDash && GetPriority(PlayerState.DASH))
        {
            CancelCurrentAction();
            dashAction.start(joystick.getFilter8());
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

    private bool Stun()
    {
        CancelCurrentAction();
        playerController.Stun();
        changeState(PlayerState.STUN);
        return true;
    }

    private bool RopeGrab()
    {
        if (GetPriority(PlayerState.ROPEGRAB))
        {
            CancelCurrentAction();
            ropegrabAction.start(joystick.GetDirection());
            changeState(PlayerState.ROPEGRAB);
            return true;
        }
        return false;
    }

    private bool Shield()
    {
        if (GetPriority(PlayerState.SHIELD))
        {
            CancelCurrentAction();
            playerController.Shield();
            changeState(PlayerState.SHIELD);
            return true;
        }
        return false;
    }

    public void ExpelAndStun(Vector2 direction, float stunTime)
    {
        CancelCurrentAction();
        expelAction.start(direction);
        changeState(PlayerState.EXPEL);
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
            print(playerState);
            bool popBuffer = false;
            switch (input)
            {
                case InputButtons.ATTACKBUTTON:
                    popBuffer = Attack();
                    break;
                case InputButtons.DASHBUTTON:
                    //print("dash");
                    popBuffer = Dash();
                    break;
                case InputButtons.JUMPBUTTON:
                    //print("jump");
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
            }

            if (terminated)
            {
                changeState(PlayerState.NORMAL);
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
            }
        }
    }

    private void MoveX()
    {
        if (playerState == PlayerState.NORMAL || playerState == PlayerState.JUMP)
        {
            playerController.MoveX(joystickDirection.x);
        }
    }

    private void MoveDown()
    {
        if (playerState == PlayerState.NORMAL)
        {
            playerController.MoveDown();
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
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.JUMP:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(true);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.DASH:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(false);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.ATTACK:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.ROPEGRAB:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(false);
                shieldActive = false;
                invincible = false;
                break;
            case PlayerState.SHIELD:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(false);
                shieldActive = true;
                invincible = false;
                break;
            case PlayerState.STUN:
                playerController.SetGravityActive(true);
                playerController.SetRopeActive(true);
                shieldActive = false;
                invincible = true;
                break;
            case PlayerState.EXPEL:
                playerController.SetGravityActive(false);
                playerController.SetRopeActive(false);
                shieldActive = false;
                invincible = true;
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
}



