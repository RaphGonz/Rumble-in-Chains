using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;

    public int playerNumber = 1;

    private Vector2 direction_raw;
    private Vector2 direction;

    public float deadZone;


    void Start()
    {
        player = GetComponent<PlayerController>();
        Mathf.Clamp(playerNumber, 1, 2);
    }

    

    void Update()
    {
        direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal" + playerNumber)) + Vector2.up * Filter(Input.GetAxis("Vertical" + playerNumber));
        direction = direction_raw.normalized;

        
        if(direction.x != 0)
        {
            player.MoveX(Mathf.Sign(direction.x)); //On donne la direction 
        }
        


        /*
        var input = Input.inputString;

        switch (input)
        {
            case ("joystick button 0"):
                if (player.canJump)
                {
                    player.Jump();
                }
                else
                {
                    StartCoroutine(JumpInputBuffer());
                }
                break;
            case ("X"):
                player.Dash(direction);
                break;
            case ("joystick button 1"):
                //player.Sprint()
                print("B pressed");
                break;
            default:
                break;
        }

        */

        

        if (Input.GetButtonDown("A" + playerNumber))
        {
            player.Jump();
        }
        if (Input.GetButtonDown("X" + playerNumber))
        {
            player.Dash(direction);
        }
        if (Input.GetButtonDown("B" + playerNumber))
        {
            print("B pressed");
            player.Sprint();
        }
        if (Input.GetButtonUp("B" + playerNumber))
        {
            print("B released");
            player.StopSprinting();
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
            if (player.jumpCount > 0)
            {
                player.Jump();
                end = true;
            }
            j--;
            yield return new WaitForEndOfFrame();
        }
        
        yield return null;
    }

}
