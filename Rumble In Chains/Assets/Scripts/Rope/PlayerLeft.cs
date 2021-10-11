using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeft : Player
{
    public override void InputManager()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            position += new Vector2(-deplacementUnit, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position += new Vector2(deplacementUnit, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            position += new Vector2(0, -deplacementUnit);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            position += new Vector2(0, deplacementUnit);
        }
    }
}
