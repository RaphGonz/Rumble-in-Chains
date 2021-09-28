using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringForce : MonoBehaviour
{
    public float springConstant = 50;
    public float frictionConstant = 5;
    public float dampingConstant = 0.2f;

    public float initialStretchingLeft = 1;
    public float initialStretchingRight = 1;


    private ConstantForce2D forceModifier;

    public GameObject leftStick;
    public GameObject rightStick;

    private Rigidbody2D leftRigidbody;
    private Rigidbody2D rightRigidbody;
    private Rigidbody2D thisRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        forceModifier = GetComponent<ConstantForce2D>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        if (leftStick)
        {
            leftRigidbody = leftStick.GetComponent<Rigidbody2D>();
        }
        if (rightStick)
        {
            rightRigidbody = rightStick.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 actualForce = forceModifier.force;
        Vector2 newForce = new Vector2(0, 0);

        float velocity = thisRigidbody.velocity.magnitude;

        if (leftStick)
        {
            Vector2 leftVector = (leftStick.transform.position - transform.position);
            float leftStretching = leftVector.magnitude;
            Vector2 leftDirection = leftVector.normalized;

            float springForce = springConstant * (leftStretching - initialStretchingLeft);
            float frictionForce = frictionConstant * (Vector2.Dot((leftRigidbody.velocity - thisRigidbody.velocity), leftVector) / leftStretching);

            Vector2 totalSpringForce = (springForce + frictionForce) * leftDirection;

            Vector2 dampingForce = - dampingConstant * velocity * velocity * thisRigidbody.velocity.normalized;


            newForce += totalSpringForce + dampingForce;
        }
        
        if (rightStick)
        {
            Vector2 rightVector = (rightStick.transform.position - transform.position);
            float rightStretching = rightVector.magnitude;
            Vector2 rightDirection = rightVector.normalized;

            float springForce = springConstant * (rightStretching - initialStretchingRight);
            float frictionForce = frictionConstant * (Vector2.Dot((rightRigidbody.velocity - thisRigidbody.velocity), rightVector) / rightStretching);

            Vector2 totalSpringForce = (springForce + frictionForce) * rightDirection;

            Vector2 dampingForce = - dampingConstant * velocity * velocity * thisRigidbody.velocity.normalized;


            newForce += totalSpringForce + dampingForce;
        }


        forceModifier.force = 0.5f * newForce + 0.5f * actualForce;

        //Vector2 leftPoint = (leftStick.transform.position + transform.position) / 2;
        //Vector2 rightPoint = (rightStick.transform.position + transform.position) / 2;
        if (leftStick && rightStick)
        {
            Vector2 direction = rightStick.transform.position - leftStick.transform.position;
            float angle = Vector2.SignedAngle(new Vector2(1, 0), direction);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
    }

}
