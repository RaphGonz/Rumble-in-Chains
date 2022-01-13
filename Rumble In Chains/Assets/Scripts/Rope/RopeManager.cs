using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public int pointNumber = 10;
    public int storageLength = 4;
    public int initialStorage = 2;

    public float maxSpeedRatio = 1.5f;

    private int variablePointNumber;
    private int leftStorage;
    private int rightStorage;



    public float stickLength;
    public float playerPointStickLength;
    public float gravity = 100f;

    public float simulationLoopIterations = 2;
    public float ropeLoopIterations = 10;
    

    public float playerRopePointRatio = 1f;

    private bool inRopeGrab = false;

    public GameObject pointPrefab;


    List<RopePoint> listRopePoints;
    List<RopePoint> listLeftStorage;
    List<RopePoint> listRightStorage;

    public GameObject leftPlayerGameObject;
    public GameObject rightCharacterGameObject;

    private PlayerController leftPlayer;
    private PlayerController rightPlayer;

    private Vector2 directionRopegrab;
    private Vector2 lastDirectionRopegrab;
    private int playerNumber;
    private float extentionRatio;
    private float initialElongation;
    private bool ropegrabCollision = false;
    private bool ropeAttraction = false;
    [SerializeField] private float ropegrabAttenuation = 0.2f;

    Vector2[] positionsLeft;
    Vector2[] positionsRight;

    //public RopePoint rightCharacter;

    Vector2 startPosition = new Vector2(5, 10);

    void Start()
    {
        leftStorage = initialStorage;
        rightStorage = initialStorage;
        variablePointNumber = pointNumber;

        listRopePoints = new List<RopePoint>();
        listLeftStorage = new List<RopePoint>();
        listRightStorage = new List<RopePoint>();

        listRopePoints.Capacity = pointNumber + initialStorage * 2;
        listLeftStorage.Capacity = storageLength;
        listRightStorage.Capacity = storageLength;

        for (int i = 0; i < initialStorage; i++)
        {
            GameObject point = Instantiate(pointPrefab, new Vector2(0,0), Quaternion.Euler(0, 0, 0));
            listLeftStorage.Add(point.GetComponent<RopePoint>());
            point.SetActive(false);

            GameObject point2 = Instantiate(pointPrefab, new Vector2(0, 0), Quaternion.Euler(0, 0, 0)); 
            listRightStorage.Add(point2.GetComponent<RopePoint>());
            point2.SetActive(false);
        }


        positionsLeft = new Vector2[pointNumber + 2];
        positionsRight = new Vector2[pointNumber + 2];

        leftPlayer = leftPlayerGameObject.GetComponent<PlayerController>();
        rightPlayer = rightCharacterGameObject.GetComponent<PlayerController>();

        startPosition = leftPlayer.position;

        
        Vector2 position = startPosition;


        for (int i = 0; i < pointNumber; i++)
        {
            GameObject point = Instantiate(pointPrefab, position, Quaternion.Euler(0, 0, 0));
            listRopePoints.Add(point.GetComponent<RopePoint>());
            position.x += stickLength;
        }
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        
    }

    public void startRopeGab(Vector2 initialGrabDirection)
    {
        inRopeGrab = true;
        lastDirectionRopegrab = initialGrabDirection;
        directionRopegrab = initialGrabDirection;
        ropegrabCollision = false;
        float distanceBetweenPlayers = (leftPlayer.position - rightPlayer.position).magnitude;
        initialElongation = distanceBetweenPlayers / (variablePointNumber * stickLength);
    }

    public void endRopeGrab()
    {
        inRopeGrab = false;
    }

    public void startRopeAttraction()
    {
        ropeAttraction = true;
    }

    public void endRopeAttraction()
    {
        ropeAttraction = false;
    }

    public bool UpdateRopegrabValues(int playerNumber, Vector2 direction, float extentionRatio)
    {
        lastDirectionRopegrab = this.directionRopegrab;
        this.playerNumber = playerNumber;
        this.directionRopegrab = direction;
        this.extentionRatio = extentionRatio;

        return ropegrabCollision;
    }

    public void placePointsTowardDirection(int iteration)
    {
        iteration += 1;
        float distanceBetweenPlayers = (leftPlayer.position - rightPlayer.position).magnitude;
        float newElongation = distanceBetweenPlayers / (variablePointNumber * stickLength);
        float wantedElongation = 1.2f;
        float currentElongation = extentionRatio * wantedElongation + (1 - extentionRatio) * initialElongation;

        Vector2 currentDirection = lastDirectionRopegrab + (directionRopegrab - lastDirectionRopegrab) * iteration / ropeLoopIterations;
        

        if (playerNumber == 1)
        {
            Vector2 pos = leftPlayer.position;
            for (int i = 0; i < listRopePoints.Count; i++)
            {
                
                Vector2 newPos = pos + currentDirection * stickLength * currentElongation * i;

                if ((newPos - listRopePoints[i].position).magnitude > ropegrabAttenuation * (1 + i * stickLength * wantedElongation))
                {
                    newPos = ropegrabAttenuation * (1 + i * stickLength * wantedElongation) * (newPos - listRopePoints[i].position).normalized + listRopePoints[i].position;
                }
                
                if (i != 0 && i != listRopePoints.Count-1)
                {
                    float maxDelta = Mathf.Max((listRopePoints[i].position - listRopePoints[i - 1].position).magnitude, (listRopePoints[i].position - listRopePoints[i + 1].position).magnitude);
                    if (maxDelta < stickLength * wantedElongation * 3f)
                    {
                        listRopePoints[i].SetPosition(newPos);
                    }
                    else
                    {
                        ropegrabCollision = true;
                    }
                }
                else
                {
                    listRopePoints[i].SetPosition(newPos);
                } 

                if (listRopePoints[i].UpdateCollisions())
                {
                    ropegrabCollision = true;
                }
            }
        }
        else
        {
            Vector2 pos = rightPlayer.position;
            for (int i = listRopePoints.Count-1; i >= 0; i--)
            {
                Vector2 newPos = pos + currentDirection * stickLength * currentElongation * (listRopePoints.Count - 1 - i);

                //listRopePoints[i].UpdateCollisions();
                if ((newPos - listRopePoints[i].position).magnitude > ropegrabAttenuation * (1 + (listRopePoints.Count - 1 - i) * stickLength * currentElongation))
                {
                    newPos = ropegrabAttenuation * (1 + (listRopePoints.Count - 1 - i) * stickLength * currentElongation) * (newPos - listRopePoints[i].position).normalized + listRopePoints[i].position;
                }

                if (i != 0 && i != listRopePoints.Count - 1)
                {
                    float maxDelta = Mathf.Max((listRopePoints[i].position - listRopePoints[i - 1].position).magnitude, (listRopePoints[i].position - listRopePoints[i + 1].position).magnitude);
                    if (maxDelta < stickLength * wantedElongation * 3f)
                    {
                        listRopePoints[i].SetPosition(newPos);
                    }
                    else
                    {
                        ropegrabCollision = true;
                    }
                }
                else
                {
                    listRopePoints[i].SetPosition(newPos);
                }

                if (listRopePoints[i].UpdateCollisions())
                {
                    ropegrabCollision = true;
                }
            }
        }
        
    }

    public bool attractPoints(int playerNumber, float attractionRatio, float relativeDistance)
    {
        bool collision = false;

        if (playerNumber == 1)
        {
            Vector2 pos = leftPlayer.position;
            for (int i = 0; i < listRopePoints.Count; i++)
            {
                listRopePoints[i].SetPosition(listRopePoints[i].position + (pos - listRopePoints[i].position) * attractionRatio * relativeDistance);
                if (listRopePoints[i].UpdateCollisions())
                {
                    collision = true;
                }
            }
        }
        else
        {
            Vector2 pos = rightPlayer.position;
            for (int i = listRopePoints.Count - 1; i >= 0; i--)
            {
                listRopePoints[i].SetPosition(listRopePoints[i].position + (pos - listRopePoints[i].position) * attractionRatio * relativeDistance);

                if (listRopePoints[i].UpdateCollisions())
                {
                    collision = true;
                }
            }
        }

        return collision;
    }

    


    public void UpdateRope()
    {
        for (int i = 0; i < Mathf.Max(simulationLoopIterations, ropeLoopIterations); i++)
        {
            if (i < simulationLoopIterations)
            {
                SimulatePoints(i);
            }
            if (i < ropeLoopIterations)
            {
                if (inRopeGrab && !ropegrabCollision)
                {
                    placePointsTowardDirection(i);
                }
                ComputePosition();
            }
        }
        DisplayPoints();
    }

    private void UpdatePlayerLeft()
    {
        leftPlayer.UpdatePlayerVelocityAndPosition();
    }

    private void UpdatePlayerRight()
    {
        rightPlayer.UpdatePlayerVelocityAndPosition();
    }

    void SimulatePoints(int iterationNumber)
    {

        //UpdatePlayerLeft(); //On a modif la prochaine position chez le joueur, en fonction des collisions
        //UpdatePlayerRight();


        if (!inRopeGrab && !ropeAttraction || iterationNumber == 0)
        {
            for (int i = 0; i < variablePointNumber; i++) //Points de la corde uniquement
            {
                Vector2 positionBeforeUpdate = listRopePoints[i].position;
                if (!inRopeGrab && !ropeAttraction)
                {
                    listRopePoints[i].TranslatePosition((listRopePoints[i].position - listRopePoints[i].previousPosition) * 1.0f);
                    listRopePoints[i].TranslatePosition(Vector2.down * gravity * Time.deltaTime * Time.deltaTime);
                    listRopePoints[i].UpdateCollisions();

                }

                listRopePoints[i].previousPosition = positionBeforeUpdate;
            }
        }

        
        

        //ComputePosition();



        /*
        LeftComputePosition();
        RightComputePosition();
        
        leftPlayer.SetPosition((positionsLeft[0] + positionsRight[0]) / 2);
        rightPlayer.SetPosition((positionsLeft[positionsLeft.Length - 1] + positionsRight[positionsRight.Length - 1]) / 2);

        for (int i = 0; i < pointNumber; i++)
        {
            listRopePoints[i].SetPosition((positionsLeft[i + 1] + positionsRight[i + 1]) / 2);
        }
        */
        

        /*
        LeftComputePosition();

        leftPlayer.SetPosition((positionsLeft[0]));
        rightPlayer.SetPosition((positionsLeft[positionsLeft.Length - 1]));

        for (int i = 0; i < pointNumber; i++)
        {
            listRopePoints[i].SetPosition((positionsLeft[i + 1]));
        }
        */

    }


    
    private void ComputePosition()
    {
        Vector2 leftVelocity = listRopePoints[0].position - listRopePoints[0].previousPosition;
        if (leftPlayer.velocity.y > maxSpeedRatio * Mathf.Abs(leftVelocity.y))
        {
            leftPlayer.velocity.y = maxSpeedRatio * Mathf.Abs(leftVelocity.y);
        }
        else if (leftPlayer.velocity.y < - maxSpeedRatio * Mathf.Abs(leftVelocity.y))
        {
            leftPlayer.velocity.y = - maxSpeedRatio * Mathf.Abs(leftVelocity.y);
        }


        Vector2 rightVelocity = listRopePoints[variablePointNumber - 1].position - listRopePoints[variablePointNumber - 1].previousPosition;
        if (rightPlayer.velocity.y > maxSpeedRatio * Mathf.Abs(rightVelocity.y)) 
        {
            if (rightPlayer.hit)
            {
                print(rightPlayer.velocity);
            }
            rightPlayer.velocity.y = maxSpeedRatio * Mathf.Abs(rightVelocity.y);
            if (rightPlayer.hit)
            {
                print(rightPlayer.velocity);
            }
        }
        else if (rightPlayer.velocity.y < -maxSpeedRatio * Mathf.Abs(rightVelocity.y))
        {
            rightPlayer.velocity.y = -maxSpeedRatio * Mathf.Abs(rightVelocity.y);
            if (rightPlayer.hit)
            {
                print(rightPlayer.velocity);
            }
        }

        leftPlayer.UpdatePositionInRegardsOfCollision();

        listRopePoints[0].UpdateCollisions();

        //Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition; //l'idée = 

        Vector2 stickCenterLeft = (leftPlayer.position + listRopePoints[0].position) / 2  /*+ playerRopePointRatio * differencePositionLeft*/;
        Vector2 stickDirectionLeft = (leftPlayer.position - listRopePoints[0].position).normalized;

        leftPlayer.SetPosition(stickCenterLeft + stickDirectionLeft * stickLength / 2);
        listRopePoints[0].SetPosition(stickCenterLeft - stickDirectionLeft * stickLength / 2);

        leftPlayer.UpdatePositionInRegardsOfCollision();//On le fait avant et après histoire d'être sûr qu'il soit pas dans un collider

        


        for (int i = 0; i < variablePointNumber - 1; i++)
        {
            listRopePoints[i].UpdateCollisions();
            listRopePoints[i + 1].UpdateCollisions(); //Pour empêcher de les faire entrer dans un collider

            Vector2 stickCenter = (listRopePoints[i].position + listRopePoints[i + 1].position) / 2;
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i + 1].position).normalized;


            listRopePoints[i].SetPosition(stickCenter + stickDirection * stickLength / 2);


            listRopePoints[i + 1].SetPosition(stickCenter - stickDirection * stickLength / 2); //logique : cf demonstration algébrique

            listRopePoints[i].UpdateCollisions();
                //listRopePoints[i + 1].UpdateCollisions();
        }
        
        

        

        rightPlayer.UpdatePositionInRegardsOfCollision();

        listRopePoints[variablePointNumber - 1].UpdateCollisions();
        

        //Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (rightPlayer.position + listRopePoints[variablePointNumber - 1].position) / 2 /*+ playerRopePointRatio * differencePositionRight*/;
        Vector2 stickDirectionRight = (listRopePoints[variablePointNumber - 1].position - rightPlayer.position).normalized;

        listRopePoints[variablePointNumber - 1].SetPosition(stickCenterRight + stickDirectionRight * stickLength / 2);
        rightPlayer.SetPosition(stickCenterRight - stickDirectionRight * stickLength / 2);

        listRopePoints[variablePointNumber - 1].UpdateCollisions();

        rightPlayer.UpdatePositionInRegardsOfCollision(); //On le fait avant et après histoire d'être sûr qu'il soit pas dans un collider
    }
    

    /*
    private void LeftComputePosition()
    {
        float modifierCoefficient = 1f;

        Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition;

        Vector2 stickCenterLeft = (modifierCoefficient * leftPlayer.position + (1 - modifierCoefficient) * listRopePoints[0].position) + playerRopePointRatio * differencePositionLeft;
        Vector2 stickDirectionLeft = (leftPlayer.position - listRopePoints[0].position).normalized;

        positionsLeft[0] = (stickCenterLeft + stickDirectionLeft * stickLength * (1 - modifierCoefficient));
        positionsLeft[1] = (stickCenterLeft - stickDirectionLeft * stickLength * modifierCoefficient);


        for (int i = 0; i < pointNumber - 1; i++)
        {
            Vector2 stickCenter = (modifierCoefficient * listRopePoints[i].position + (1 - modifierCoefficient) * listRopePoints[i + 1].position);
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i + 1].position).normalized;
          
       
            //positionsLeft[i + 1] = (stickCenter + stickDirection * stickLength * (1 - modifierCoefficient));


            positionsLeft[i + 2] = (stickCenter - stickDirection * stickLength * modifierCoefficient);
        }

        Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (modifierCoefficient * listRopePoints[pointNumber - 1].position + (1 - modifierCoefficient) * rightPlayer.position) + playerRopePointRatio * differencePositionRight;
        Vector2 stickDirectionRight = (listRopePoints[pointNumber - 1].position - rightPlayer.position).normalized;

        //positionsLeft[positionsLeft.Length - 2] = (stickCenterRight + stickDirectionRight * stickLength * (1 - modifierCoefficient));
        positionsLeft[positionsLeft.Length - 1] = (stickCenterRight - stickDirectionRight * stickLength * modifierCoefficient);
    }

    private void RightComputePosition()
    {
        float modifierCoefficient = 1f;

        Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (modifierCoefficient * rightPlayer.position + (1 - modifierCoefficient) * listRopePoints[pointNumber - 1].position) + playerRopePointRatio * differencePositionRight;
        Vector2 stickDirectionRight = (rightPlayer.position - listRopePoints[pointNumber - 1].position).normalized;

        positionsRight[positionsLeft.Length - 1] = (stickCenterRight + stickDirectionRight * stickLength * (1 - modifierCoefficient));
        positionsRight[positionsLeft.Length - 2] = (stickCenterRight - stickDirectionRight * stickLength * modifierCoefficient);


        for (int i = pointNumber - 1; i > 0; i--)
        {
            Vector2 stickCenter = (modifierCoefficient * listRopePoints[i].position + (1 - modifierCoefficient) * listRopePoints[i - 1].position);
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i - 1].position).normalized;

            //positionsRight[i + 1] = (stickCenter + stickDirection * stickLength * (1 - modifierCoefficient));


            positionsRight[i] = (stickCenter - stickDirection * stickLength * modifierCoefficient);
        }

        
        Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition;

        Vector2 stickCenterLeft = (modifierCoefficient * listRopePoints[0].position + (1 - modifierCoefficient) * leftPlayer.position) + playerRopePointRatio * differencePositionLeft;
        Vector2 stickDirectionLeft = (listRopePoints[0].position - leftPlayer.position).normalized;

        //positionsRight[1] = (stickCenterLeft + stickDirectionLeft * stickLength * (1 - modifierCoefficient));
        positionsRight[0] = (stickCenterLeft - stickDirectionLeft * stickLength * modifierCoefficient);
    }
    */
    

    void DisplayPoints()
    {
        //leftPlayer.UpdateCollisions();
        leftPlayer.ApplyNewPosition();
        for (int i = 0; i < variablePointNumber; i++)
        {

            //listRopePoints[i].UpdateCollisions();
            listRopePoints[i].UpdateCollisions();
            listRopePoints[i].Actualise(); //On applique les nouvelles positions calculées aux gameObject
        }


        //rightPlayer.UpdateCollisions();
        rightPlayer.ApplyNewPosition();
    }


    
    private void TestInput()
    {
        if (Input.GetButtonDown("RB1"))
        {
            AddRopeLeft();
        }
        else if (Input.GetButtonDown("LB1"))
        {
            ReduceRopeLeft();
        }

        if (Input.GetButtonDown("RB2"))
        {
            AddRopeRight();
        }
        else if (Input.GetButtonDown("LB2"))
        {
            ReduceRopeRight();
        }
    }
    

    private void ReduceRopeLeft()
    {
        if (leftStorage < storageLength)
        {
            listRopePoints[0].gameObject.SetActive(false);
            listLeftStorage.Add(listRopePoints[0]);
            listRopePoints.RemoveAt(0);

            leftStorage++;
            variablePointNumber--;
        }
    }

    private void AddRopeLeft()
    {
        if (leftStorage > 0)
        {
            listLeftStorage[listLeftStorage.Count - 1].gameObject.SetActive(true);
            listRopePoints.Insert(0, listLeftStorage[listLeftStorage.Count - 1]);
            listLeftStorage.RemoveAt(listLeftStorage.Count - 1);

            variablePointNumber++;
            leftStorage--;
        }
    }

    private void ReduceRopeRight()
    {
        if (rightStorage < storageLength)
        {
            listRopePoints[variablePointNumber - 1].gameObject.SetActive(false);
            listRightStorage.Add(listRopePoints[variablePointNumber-1]);
            listRopePoints.RemoveAt(variablePointNumber - 1);

            variablePointNumber--;
            rightStorage++;
        }
    }

    private void AddRopeRight()
    {
        if (rightStorage > 0)
        {
            listRightStorage[listRightStorage.Count - 1].gameObject.SetActive(true);
            listRopePoints.Insert(variablePointNumber - 1, listRightStorage[listRightStorage.Count - 1]);
            listRightStorage.RemoveAt(listRightStorage.Count - 1);

            variablePointNumber++;
            rightStorage--;
        }
    }




}
