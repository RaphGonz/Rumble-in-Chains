using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 position;
    public Vector2 previousPosition;

    protected RopePointCollider ropePointCollider;

    protected float deplacementUnit = 0.1f;
    public float gravity = 100;

    protected void Start()
    {
        ropePointCollider = GetComponent<RopePointCollider>();
        previousPosition = transform.position;
        position = transform.position;
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

    public virtual void InputManager()
    {
    }

    public void UpdatePosition()
    {
        InputManager();

        TranslatePosition(Vector2.down * gravity * Time.deltaTime * Time.deltaTime);
        //TranslatePosition(position - previousPosition);
    }
}
