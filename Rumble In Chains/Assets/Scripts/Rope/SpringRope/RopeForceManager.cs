using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeForceManager : MonoBehaviour
{
    public int pointNumber = 10;
    public float stickLength;
    public float playerPointStickLength;
    public float gravity = 100f;

    public float simulationLoopIterations = 3;

    public float playerRopePointRatio = 1f;

    public GameObject pointPrefab;




    public float springConstant = 50;
    public float frictionConstant = 5;
    public float dampingConstant = 0.2f;

    public float initialStretchingLeft = 1;
    public float initialStretchingRight = 1;





    List<RopePoint> listRopePoints;
    List<RopePoint> listAllRopePoints;

    List<Vector2> listForces;
    List<Vector2> listVelocities;

    public GameObject leftCharacter;
    public GameObject rightCharacter;

    private Player leftPlayer;
    private Player rightPlayer;

    //public RopePoint rightCharacter;

    Vector2 startPosition = new Vector2(5, 10);

    void Start()
    {

        leftPlayer = leftCharacter.GetComponent<Player>();
        rightPlayer = rightCharacter.GetComponent<Player>();

        startPosition = leftPlayer.position;

        listRopePoints = new List<RopePoint>();
        listAllRopePoints = new List<RopePoint>();
        listForces = new List<Vector2>();
        listVelocities = new List<Vector2>();

        Vector2 position = startPosition;

        listAllRopePoints.Add(leftCharacter.GetComponent<RopePoint>());
        listForces.Add(new Vector2(0, 0));
        listVelocities.Add(new Vector2(0, 0));

        for (int i = 0; i < pointNumber; i++)
        {
            GameObject point = Instantiate(pointPrefab, position, Quaternion.Euler(0, 0, 0));
            listRopePoints.Add(point.GetComponent<RopePoint>());
            listForces.Add(new Vector2(0, 0));
            listVelocities.Add(new Vector2(0, 0));
            position.x += stickLength;
        }

        listAllRopePoints.Add(rightCharacter.GetComponent<RopePoint>());
        listForces.Add(new Vector2(0, 0));
        listVelocities.Add(new Vector2(0, 0));
    }

    // Update is called once per frame


    void Update()
    {
        for (int i = 0; i < simulationLoopIterations; i++)
        {
            SimulatePoints();
            DisplayPoints();
        }
    }

    private void UpdatePlayerLeft()
    {
        leftPlayer.UpdatePosition();
    }

    private void UpdatePlayerRight()
    {
        rightPlayer.UpdatePosition();
    }

    void SimulatePoints()
    {
        Vector2 positionBeforeUpdateLeft = leftPlayer.transform.position;
        UpdatePlayerLeft();
        leftPlayer.previousPosition = positionBeforeUpdateLeft;

        Vector2 positionBeforeUpdateRight = rightPlayer.transform.position;
        UpdatePlayerRight();
        rightPlayer.previousPosition = positionBeforeUpdateRight;


        for (int i = 0; i < pointNumber + 2; i++)
        {
            listForces[i] = new Vector2(0,0);
        }

        ComputePosition();

        for (int i = 0; i < pointNumber + 2; i++)
        {
            //listRopePoints[i].TranslatePosition((listRopePoints[i].position - listRopePoints[i].previousPosition) * 0.9f);
            listForces[i] += Vector2.down * gravity * Time.deltaTime * Time.deltaTime;
        }

        for (int i = 0; i < pointNumber + 2; i++)
        {
            listVelocities[i] += listForces[i] * Time.deltaTime;
            listAllRopePoints[i].position += listVelocities[i] * Time.deltaTime;
        }

    }



    private void ComputePosition()
    {


        //leftPlayer.UpdateCollisions();
        //listAllRopePoints[1].UpdateCollisions();

        Vector2 rightVector = (listRopePoints[1].position - listRopePoints[0].position);
        float rightStretching = rightVector.magnitude;
        Vector2 rightDirection = rightVector.normalized;

        float springForce = springConstant * (rightStretching - initialStretchingLeft);

        float frictionForce = 0;
        //float frictionForce = frictionConstant * (Vector2.Dot((listVelocities[currentIndice + 1] - listVelocities[currentIndice - 1]), rightVector) / rightStretching);


        Vector2 totalSpringForce = (springForce + frictionForce) * rightDirection;

        Vector2 dampingForce = -dampingConstant * listVelocities[0] * listVelocities[0] * listVelocities[0].normalized;


        listForces[0] += totalSpringForce + dampingForce;

        //leftPlayer.UpdateCollisions();


        for (int i = 1; i < pointNumber; i++)
        {
            //listAllRopePoints[i].UpdateCollisions();
            //listAllRopePoints[i + 1].UpdateCollisions();
            

            ComputeSpringForce(i);

            //listRopePoints[i].UpdateCollisions();
            //listRopePoints[i + 1].UpdateCollisions();
        }

        //listAllRopePoints[pointNumber].UpdateCollisions();
        //rightPlayer.UpdateCollisions();


        rightVector = (listRopePoints[pointNumber].position - listRopePoints[pointNumber+1].position);
        rightStretching = rightVector.magnitude;
        rightDirection = rightVector.normalized;

        springForce = springConstant * (rightStretching - initialStretchingLeft);

        frictionForce = 0;
        //float frictionForce = frictionConstant * (Vector2.Dot((listVelocities[currentIndice + 1] - listVelocities[currentIndice - 1]), rightVector) / rightStretching);


        totalSpringForce = (springForce + frictionForce) * rightDirection;

        dampingForce = -dampingConstant * listVelocities[pointNumber + 1] * listVelocities[pointNumber + 1] * listVelocities[pointNumber + 1].normalized;


        listForces[pointNumber + 1] += totalSpringForce + dampingForce;


        
    }

    private void ComputeSpringForce(int currentIndice)
    {

        Vector2 leftVector = (listAllRopePoints[currentIndice - 1].position - listAllRopePoints[currentIndice].position);
        float leftStretching = leftVector.magnitude;
        Vector2 leftDirection = leftVector.normalized;

        float springForce = springConstant * (leftStretching - initialStretchingLeft);
        float frictionForce = frictionConstant * (Vector2.Dot((listVelocities[currentIndice - 1] - listVelocities[currentIndice + 1]), leftVector) / leftStretching);

        Vector2 totalSpringForce = (springForce + frictionForce) * leftDirection;

        Vector2 dampingForce = -dampingConstant * listVelocities[currentIndice] * listVelocities[currentIndice] * listVelocities[currentIndice].normalized;


        listForces[currentIndice] += totalSpringForce + dampingForce;
        


        Vector2 rightVector = (listAllRopePoints[currentIndice + 1].position - listAllRopePoints[currentIndice].position);
        float rightStretching = rightVector.magnitude;
        Vector2 rightDirection = rightVector.normalized;

        springForce = springConstant * (rightStretching - initialStretchingLeft);

        frictionForce = frictionConstant * (Vector2.Dot((listVelocities[currentIndice + 1] - listVelocities[currentIndice - 1]), rightVector) / rightStretching);


        totalSpringForce = (springForce + frictionForce) * rightDirection;

        dampingForce = -dampingConstant * listVelocities[currentIndice] * listVelocities[currentIndice] * listVelocities[currentIndice].normalized;


        listForces[currentIndice] += totalSpringForce + dampingForce;
    }



    void DisplayPoints()
    {
        for (int i = 0; i < pointNumber + 2; i++)
        {
            listAllRopePoints[i].UpdateCollisions();
            listAllRopePoints[i].Actualise();
        }
    }
}
