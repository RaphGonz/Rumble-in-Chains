using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController playerController;

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
    bool planted = false;
    bool justPlanted = false;
    Vector2 position = new Vector2();

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

    

    void Update()
    {
        
        
        if (!stunned)
        {

            direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal" + playerNumber)) + Vector2.up * Filter(Input.GetAxis("Vertical" + playerNumber));
            direction = direction_raw.normalized;


            if (direction.x != 0)
            {
                playerController.MoveX(Mathf.Sign(direction.x)); //On donne la direction 
            }

            if (Input.GetButtonDown("A" + playerNumber))
            {
                //print("A pressed");
                playerController.Jump();
            }
            if (Input.GetButtonDown("X" + playerNumber))
            {
                //print("A pressed");
                playerController.Dash(direction);
            }
            if (Input.GetButtonDown("B" + playerNumber))
            {
                //print("B pressed");
                playerController.Sprint();
            }
            if (Input.GetButtonUp("B" + playerNumber))
            {
                //print("B released");
                playerController.StopSprinting();
            }
            if (Input.GetButtonDown("Y" + playerNumber))
            {
                print("hi");
                bool vertical = Mathf.Abs(Input.GetAxis("Vertical" + playerNumber)) > Mathf.Abs(Input.GetAxis("Horizontal" + playerNumber));
                bool neutral = Input.GetAxis("Vertical" + playerNumber) == 0 && Input.GetAxis("Horizontal" + playerNumber) == 0;
                //print("Y pressed");
                if (!GetComponent<PlayerController>().grounded) {
                    if (vertical && Input.GetAxis("Vertical" + playerNumber) < 0)
                    {
                        characterController.Attack(AttackType.DownAir);
                        print("where");
                    }
                    else if (vertical && Input.GetAxis("Vertical" + playerNumber) > 0)
                    {
                        characterController.Attack(AttackType.UpAir);
                        print("you");
                    }
                    else if (neutral)
                    {
                        characterController.Attack(AttackType.NeutralAir);
                        print("at");
                    }
                    else
                    {
                        characterController.Attack(AttackType.SideAir);
                        print("bruh ?");
                    }

                }
                else
                {
                    if (vertical && Input.GetAxis("Vertical" + playerNumber) > 0)
                    {
                        characterController.Attack(AttackType.UpTilt);
                    }
                    else if (vertical && Input.GetAxis("Vertical" + playerNumber) < 0) 
                    {
                        characterController.Attack(AttackType.DownTilt);
                    }
                    else if (neutral)
                    {
                        characterController.Attack(AttackType.Jab);
                        
                    }
                    else
                    {
                        characterController.Attack(AttackType.SideTilt);
                        print("hello");
                    }
                }

            }
            if (Input.GetButtonDown("LB2") && GetComponent<PlayerController>().grounded)
            {
                planted = true;
                position.x = transform.position.x;
                position.y = transform.position.y;
                transform.position = position;
            }
            if (Input.GetButtonUp("LB2") && planted)
            {
                planted = false; // ajouter qqs frames de déplantage par coroutine ? 
            }
        }
        else
        {
            timeStunned++;
            if (Input.anyKeyDown)
            {
                //Idées : Appuyer sur 3 boutons en même temps enlève 3 frames
                //        Changer de direction enlève une framea
                timeStunned++;
            }
            //stunSlider.ChangeStunValue(stunTimeInFrames - timeStunned);
            if(timeStunned >= stunTimeInFrames)
            {
                stunned = false;
                timeStunned = 0;
                gameObject.GetComponent<CharacterController>().recovering = true;
                //coroutineStarted = true;
                //stunSlider.ChangeStunValue(stunTimeInFrames);
                
            }
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
        //stunSlider.ChangeMaxStunValue(stunTimeInFrames);
        //stunSlider.ChangeStunValue(stunTimeInFrames);
    }
}
