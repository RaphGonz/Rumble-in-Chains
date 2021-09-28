using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePoint : MonoBehaviour
{

    public Vector2 position;
    public Vector2 previousPosition;

    public bool onContact = false;

    public void Start()
    {
        previousPosition = transform.position;
        position = transform.position;
    }

    private void Update()
    {
        //transform.position = position;
    }

    public void Actualise()
    {
        if (!onContact)
        {
            transform.position = position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onContact = true;
        print("contact");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onContact = false;
    }


}
