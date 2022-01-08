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
    COUNT
}




public class ActionController : MonoBehaviour
{
    private PlayerState playerState;
    private Vector2 joystickDirection;
    private AttackType attackType = AttackType.Jab;

    private bool canDash = true;
    private bool canJump = true;


    //private bool[][] priorityMatrix = new bool[((int)PlayerState.COUNT)* ((int)PlayerState.COUNT)];



    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerController playerController;
    [SerializeField] BufferManager buffer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateActions()
    {
        InputButtons input = buffer.getBufferElement();
        if (input != InputButtons.NULL)
        {
            switch (input)
            {
                case InputButtons.ATTACKBUTTON:
                    Attack();
                    break;
                case InputButtons.DASHBUTTON:
                    Dash();
                    break;
                case InputButtons.JUMPBUTTON:
                    Jump();
                    break;
                case InputButtons.ROPEBUTTON:
                    RopeGrab();
                    break;
                case InputButtons.SHIELDBUTTON:
                    Shield();
                    break;
            }
        }

        joystickDirection = buffer.getDirection();

        if (joystickDirection.x != 0)
        {
            MoveX();
        }
        if (joystickDirection.y < 0)
        {
            MoveDown();
        }
    }

    private void Attack()
    {
        if (GetPriority(PlayerState.ATTACK))
        {
            characterController.Attack(attackType);
        }
    }

    private void Dash()
    {
        if (canDash && GetPriority(PlayerState.DASH))
        {
            playerController.Dash(joystickDirection);
        }
    }

    private void Jump()
    {
        if (canJump && GetPriority(PlayerState.JUMP))
        {
            playerController.Jump();
            changeState(PlayerState.JUMP);
        }
    }

    private void Stun()
    {
        playerController.Stun();
    }

    private void RopeGrab()
    {
        if (GetPriority(PlayerState.ROPEGRAB))
        {
            playerController.RopeGrab(joystickDirection);
        }
    }

    private void Shield()
    {
        if (GetPriority(PlayerState.SHIELD))
        {
            playerController.Shield();
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



