using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public int number_of_player_inside = 0;


    [SerializeField]
    CharacterController charCon;
    [SerializeField]
    float frequency; //Frequency = number of point per seconds
    float time; // 1/f
    [SerializeField]
    float pointsGiven = 1;
    [SerializeField]
    int player;

    
    float current_timer = 0f;

    private void Start()
    {
        time = 1 / frequency;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        number_of_player_inside++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        number_of_player_inside--;
        current_timer = 0f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(number_of_player_inside == 1)
        {
            Debug.Log(current_timer);
            current_timer += Time.deltaTime;
            if(current_timer >= time)
            {
                charCon.Points += pointsGiven;
                current_timer = 0;
                UIController.Instance.MakeBubbleAppear(player, pointsGiven);

            }  
        }
    }
}
