using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 position;
    public Vector2 positionBeforeCollider;

    public Vector2 previousPosition;

    protected RopePointCollider ropePointCollider;

    public float deplacementUnit = 0.1f;
    public float gravity = 100;

    protected void Start()
    {
        ropePointCollider = GetComponent<RopePointCollider>();
        previousPosition = transform.position;
        position = transform.position;
        positionBeforeCollider = transform.position;
    }

    public void Actualise()
    {
        transform.position = position;
    }

    public void UpdateCollisions()
    {

        Vector2 movement = position - new Vector2(positionBeforeCollider.x, positionBeforeCollider.y);

        ropePointCollider.UpdateCollisions(ref movement);

        position = new Vector3(movement.x + positionBeforeCollider.x, movement.y + positionBeforeCollider.y, 0);
        positionBeforeCollider = position;
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
        //super jeu video

        //TranslatePosition(position - previousPosition);
    }
}
