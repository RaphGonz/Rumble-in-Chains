using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePoint : MonoBehaviour
{

    private RopePointCollider ropePointCollider;

    public Vector2 position;
    public Vector2 positionBeforeCollider;

    public Vector2 previousPosition;

    public bool onContact = false;

    public void Awake()
    {
        ropePointCollider = GetComponent<RopePointCollider>();
        previousPosition = transform.position;
        position = transform.position;
        positionBeforeCollider = transform.position;
    }

    private void Update()
    {
    }

    public void Actualise()
    {
        UpdateCollisions();
        transform.position = position;
    }


    public bool UpdateCollisions()
    {
        bool collision = false;
        Vector2 movement = position - new Vector2(positionBeforeCollider.x, positionBeforeCollider.y);

        if (movement.magnitude > 1000f)
        {
            movement = movement.normalized * 1000f;
        }


        collision = ropePointCollider.UpdateCollisions(ref movement, positionBeforeCollider);

        position = new Vector3(movement.x + positionBeforeCollider.x, movement.y + positionBeforeCollider.y, 0);
        positionBeforeCollider = position;

        return collision;
    }

    /*
    public void Move(Vector2 translatePosition)
    {
        position += translatePosition;
        transform.Translate(translatePosition);
    }
    */

    public void TranslatePosition(Vector2 positionChange)
    {
        position += positionChange;
    }

    public void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
    }
}
