using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWall : MonoBehaviour
{
    [SerializeField] Vector2 direction;
    [SerializeField] bool left;
    [SerializeField] int stunFrames;

    ActionController playerLeft;
    ActionController playerRight;

    private void Start()
    {
        if (left)
        {
            direction.x = -Mathf.Abs(direction.x);
        }
        else
        {
            direction.x = Mathf.Abs(direction.x);
        }

        playerLeft = GameObject.Find("PlayerLeft").GetComponent<ActionController>();
        playerRight = GameObject.Find("PlayerRight").GetComponent<ActionController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLeft")
        {
            playerLeft.ExpelAndStun(direction, stunFrames);
            playerRight.Stun(stunFrames);
        }
        else if (collision.gameObject.name == "PlayerRight")
        {
            playerRight.ExpelAndStun(direction, stunFrames);
            playerLeft.Stun(stunFrames);
        }
    }
}
