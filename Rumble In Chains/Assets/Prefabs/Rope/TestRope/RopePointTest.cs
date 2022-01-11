using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePointTest : MonoBehaviour
{

    private RopePointCollider ropePointCollider;

    public Vector2 position;
    public Vector2 positionBeforeCollider;

    public float gravity = 10;

    public Vector2 previousPosition;

    public bool onContact = false;
    public bool fix = false;

    public void Awake()
    {
        ropePointCollider = GetComponent<RopePointCollider>();
        previousPosition = transform.position;
        position = transform.position;
        positionBeforeCollider = transform.position;
    }

    private void Update()
    {
        //transform.position = position;
    }

    public void Actualise()
    {
        if(!fix) transform.position = position;
    }


    public void UpdateCollisions()
    {
        Vector2 movement = position - new Vector2(positionBeforeCollider.x, positionBeforeCollider.y);

        ropePointCollider.UpdateCollisions(ref movement, positionBeforeCollider);

        position = new Vector3(movement.x + positionBeforeCollider.x, movement.y + positionBeforeCollider.y, 0);
        positionBeforeCollider = position;
    }

    public void Move(Vector2 translatePosition)
    {
        if (!fix) {
            position += translatePosition;
            transform.Translate(translatePosition);
        }
        else { position = transform.position; }

    }


    public void TranslatePosition(Vector2 positionChange)
    {
        if (!fix) position += positionChange;
        else { position = transform.position; }
    }

    public void SetPosition(Vector2 newPosition)
    {
        if (!fix) position = newPosition;
        else { position = transform.position; }
    }

    public float GetGravity()
    {
        return gravity;
    }
}