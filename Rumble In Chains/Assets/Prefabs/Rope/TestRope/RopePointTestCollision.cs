using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePointTestCollision : MonoBehaviour
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
        float val = 5f;

        Vector2 direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal1")) + Vector2.up * Filter(Input.GetAxis("Vertical1"));

        position += direction_raw * val;
        
        if (Input.GetButtonDown("A1"))
        {
            //print("A pressed");
            //playerController.Jump();
            position.y -= val;
        }
        if (Input.GetButtonDown("X1"))
        {
            //print("A pressed");
            //playerController.Dash(direction);
            position.x -= val;
        }
        if (Input.GetButtonDown("B1"))
        {
            //print("B pressed");
            //playerController.Sprint();
            position.x += val;
        }
        if (Input.GetButtonDown("Y1"))
        {
            //print("Y pressed");
            //characterController.Attack(AttackType.Jab);
            position.y += val;
        }

        Actualise();
    }

    public float Filter(float f)
    {
        if (Mathf.Abs(f) < 0.3f)
        {
            return 0f;
        }
        else
        {
            return f;
        }
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
