using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] Vector2 direction;
    [SerializeField] bool left;
    [SerializeField] int stunFrames;


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
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionController actionController = collision.gameObject.GetComponent<ActionController>();
        if (actionController != null && !actionController.isInvincible())
        {
            actionController.ExpelAndStun(direction, stunFrames);
        }
    }
    */

    private void OnTriggerStay2D(Collider2D collision)
    {
        ActionController actionController = collision.gameObject.GetComponent<ActionController>();
        if (actionController != null && !actionController.isInvincible())
        {
            actionController.ExpelAndStun(direction, stunFrames);
        }
    }
}
