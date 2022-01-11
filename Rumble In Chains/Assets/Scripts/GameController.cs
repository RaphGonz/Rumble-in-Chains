using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] InputManager inputManagerLeft;
    [SerializeField] InputManager inputManagerRight;

    [SerializeField] ActionController actionControllerLeft;
    [SerializeField] ActionController actionControllerRight;

    [SerializeField] PlayerController playerControllerLeft;
    [SerializeField] PlayerController playerControllerRight;

    [SerializeField] CharacterController characterControllerLeft;
    [SerializeField] CharacterController characterControllerRight;

    [SerializeField] RopeManager ropeManager;


    [SerializeField] float gameIteration = 2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputManagerLeft.UpdateInput();
        inputManagerRight.UpdateInput();

        actionControllerLeft.UpdateActions();
        actionControllerRight.UpdateActions();

        for (int i = 0; i < gameIteration; i++)
        {
            playerControllerLeft.UpdatePlayerVelocityAndPosition();
            playerControllerRight.UpdatePlayerVelocityAndPosition();

            characterControllerLeft.UpdateCharacter();
            characterControllerRight.UpdateCharacter();
        }

        ropeManager.UpdateRope();


    }
}
