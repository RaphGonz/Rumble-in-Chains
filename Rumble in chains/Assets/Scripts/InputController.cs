using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Vector3 movement;
    [SerializeField]
    float xSpeed;
    [SerializeField]
    float jumpSpeed;
    bool beingHit = false;
    bool jumping = false;
    [SerializeField]
    int maxJumpFramesCounter;       
    int jumpFramesCounter;
    //!!!
    [SerializeField]
    CharacterController myCharacterController;
    public bool attacking = false;
    //!!!

    
    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3();
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal") * Time.deltaTime * xSpeed ;
        if (!jumping && Input.GetKeyDown(KeyCode.Space))
        {
            movement.y += Time.deltaTime * jumpSpeed;
            jumping = true;
        }
        if (jumping && !beingHit && jumpFramesCounter > 0)
        {
            jumpFramesCounter--;
        }
        if(jumpFramesCounter == 0)
        {
            movement.y = 0;
            jumping = false;
            Debug.Log("oui");
            jumpFramesCounter = maxJumpFramesCounter;
        }
        transform.Translate(movement);
        //!!!
        if (!attacking)
        {
            if (Input.GetKeyDown(KeyCode.J) && this.gameObject.name.Equals("Player1"))
            {
                myCharacterController.Attack(AttackType.Jab);
                attacking = true;
            }
            /*if (Input.GetKeyDown(KeyCode.K))//Exemple
            {
                myCharacterController.Attack(AttackType.SideTilt);
            }*/
            
        }
        //!!!
    }

    
}
