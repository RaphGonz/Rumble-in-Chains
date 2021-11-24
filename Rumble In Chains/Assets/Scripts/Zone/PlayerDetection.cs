using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public int number_of_player_inside = 0;

    
    
    [SerializeField]
    float frequency = 1f; //Frequency = number of point per seconds
    float time; // 1/f
    [SerializeField]
    int pointsGiven = 1;

    
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
            
            current_timer += Time.deltaTime;
            if(current_timer >= time)
            {
                collision.gameObject.GetComponent<CharacterController>().Points += pointsGiven;
                current_timer = 0f;
            }
            
        }
    }
}
