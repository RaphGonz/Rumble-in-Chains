using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public enum InputButtons
{
    NULL,
    ATTACKBUTTON,
    JUMPBUTTON,
    DASHBUTTON,
    ROPEBUTTON,
    SHIELDBUTTON,
    COUNT
}

public enum JoystickDirection
{
    CENTER,
    LEFT,
    RIGHT,
    DOWN,
    UP,
    DOWNLEFT,
    DOWNRIGHT,
    UPLEFT,
    UPRIGHT,
    COUNT
}

public class InputManager : MonoBehaviour
{
    PlayerController playerController;

    InputButtons controllerInput;

    [SerializeField] BufferManager buffer;


    public int playerNumber = 1;

    private Vector2 direction_raw;
    public Vector2 direction;

    public float deadZone;

    public bool stunned; //Permet de savoir si on est stunned ou non
    public int stunTimeInFrames = 0;
    int timeStunned = 0;

    [SerializeField]
    CharacterController characterController;

    public bool coroutineStarted = false;

    #region UI
    [SerializeField]
    UIStunSlider stunSlider;
    #endregion

    void Start()
    {
        stunned = false;
        playerController = GetComponent<PlayerController>();
        Mathf.Clamp(playerNumber, 1, 2);
    }

    

    public void UpdateInput()
    {

        stunned = false;
        if (!stunned)
        {

            direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal" + playerNumber)) + Vector2.up * Filter(Input.GetAxis("Vertical" + playerNumber));
            direction = direction_raw.normalized;

            buffer.setDirection(direction);



            if (Input.GetButtonDown("A" + playerNumber))
            {
                //print("A pressed");
                //playerController.Jump();
                controllerInput = InputButtons.JUMPBUTTON;
            }
            if (Input.GetButtonDown("X" + playerNumber))
            {
                //print("A pressed");
                //playerController.Dash(direction);
                controllerInput = InputButtons.ATTACKBUTTON;
            }
            if (Input.GetButtonDown("B" + playerNumber))
            {
                //print("B pressed");
                //playerController.Sprint();
                controllerInput = InputButtons.DASHBUTTON;
            }
            if (Input.GetButtonUp("B" + playerNumber))
            {
                //print("B released");
                //playerController.StopSprinting();
            }
            if (Input.GetButtonDown("Y" + playerNumber))
            {
                //print("Y pressed");
                //characterController.Attack(AttackType.Jab);
                controllerInput = InputButtons.ROPEBUTTON;
            }
            if (Input.GetButtonDown("LB" + playerNumber))
            {
                //playerController.ImmobilizePlayer();
                controllerInput = InputButtons.SHIELDBUTTON;
            }
        }

        /*
        else
        {
            timeStunned++;
            if (Input.anyKeyDown)
            {
                //Id�es : Appuyer sur 3 boutons en m�me temps enl�ve 3 frames
                //        Changer de direction enl�ve une framea
                timeStunned++;
            }
            stunSlider.ChangeStunValue(stunTimeInFrames - timeStunned);
            if(timeStunned >= stunTimeInFrames)
            {
                stunned = false;
                timeStunned = 0;
                gameObject.GetComponent<CharacterController>().recovering = true;
                //coroutineStarted = true;
                stunSlider.ChangeStunValue(stunTimeInFrames);
                
            }
        }
        */


        if (controllerInput != InputButtons.NULL)
        {
            buffer.addToBuffer(controllerInput);
        }
        

    }

    public float Filter(float f)
    {
        if(Mathf.Abs(f) < deadZone)
        {
            return 0f;
        }
        else
        {
            return f;
        }
    }


    IEnumerator JumpInputBuffer()
    {
        int j = 20;
        bool end = false;
        while(j > 0 && !end)
        {
            if (playerController.jumpCount > 0)
            {
                playerController.Jump();
                end = true;
            }
            j--;
            yield return new WaitForEndOfFrame();
        }
        
        yield return null;
    }

    internal void Stun(int stunTimeInFrames)
    {
        stunned = true;
        this.stunTimeInFrames = stunTimeInFrames;
        stunSlider.ChangeMaxStunValue(stunTimeInFrames);
        stunSlider.ChangeStunValue(stunTimeInFrames);
    }

}
