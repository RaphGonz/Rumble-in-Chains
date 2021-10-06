using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePoint : MonoBehaviour
{

    private RopePointCollider ropePointCollider;

    public Vector2 position;
    public Vector2 previousPosition;

    public bool onContact = false;

    public void Start()
    {
        ropePointCollider = GetComponent<RopePointCollider>();
        previousPosition = transform.position;
        position = transform.position;
    }

    private void Update()
    {
        //transform.position = position;
    }

    public void Actualise()
    {
        Vector2 movement = position - new Vector2(transform.position.x, transform.position.y);

        ropePointCollider.UpdateCollisions(ref movement);

        transform.position += new Vector3(movement.x, movement.y, 0);
        position = transform.position;
    }

    public void Move(Vector2 translatePosition)
    {
        position += translatePosition;
        transform.Translate(translatePosition);
    }


    public void TranslatePosition(Vector2 positionChange)
    {
        position += positionChange;
    }

    public void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
    }
}
