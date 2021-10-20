using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRight : Player
{

    public override void InputManager()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position += new Vector2(-deplacementUnit, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            position += new Vector2(deplacementUnit, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            position += new Vector2(0, -deplacementUnit);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            position += new Vector2(0, deplacementUnit);
        }
    }

}
