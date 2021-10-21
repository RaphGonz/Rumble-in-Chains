using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public int pointNumber = 10;
    public int storageLength = 4;
    public int initialStorage = 2;

    private int variablePointNumber;
    private int leftStorage;
    private int rightStorage;



    public float stickLength;
    public float playerPointStickLength;
    public float gravity = 100f;

    public float simulationLoopIterations = 3;

    public float playerRopePointRatio = 1f;

    public GameObject pointPrefab;


    List<RopePoint> listRopePoints;
    List<RopePoint> listLeftStorage;
    List<RopePoint> listRightStorage;

    public GameObject leftCharacter;
    public GameObject rightCharacter;

    private Player leftPlayer;
    private Player rightPlayer;

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

        leftPlayer = leftCharacter.GetComponent<Player>();
        rightPlayer = rightCharacter.GetComponent<Player>();

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


    void Update()
    {
        TestInput();

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


        for (int i = 0; i < variablePointNumber; i++)
        {
            Vector2 positionBeforeUpdate = listRopePoints[i].transform.position;
            listRopePoints[i].TranslatePosition((listRopePoints[i].position - listRopePoints[i].previousPosition) * 0.9f);
            listRopePoints[i].TranslatePosition(Vector2.down * gravity * Time.deltaTime * Time.deltaTime);
            listRopePoints[i].previousPosition = positionBeforeUpdate;
        }

        ComputePosition();

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
        leftPlayer.UpdateCollisions();
        listRopePoints[0].UpdateCollisions();

        Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition;

        Vector2 stickCenterLeft = (leftPlayer.position + listRopePoints[0].position) / 2 + playerRopePointRatio * differencePositionLeft;
        Vector2 stickDirectionLeft = (leftPlayer.position - listRopePoints[0].position).normalized;

        leftPlayer.SetPosition(stickCenterLeft + stickDirectionLeft * stickLength / 2);
        listRopePoints[0].SetPosition(stickCenterLeft - stickDirectionLeft * stickLength / 2);

        leftPlayer.UpdateCollisions();


        for (int i = 0; i < variablePointNumber - 1; i++)
        {
            listRopePoints[i].UpdateCollisions();
            listRopePoints[i+1].UpdateCollisions();

            Vector2 stickCenter = (listRopePoints[i].position + listRopePoints[i + 1].position) / 2;
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i + 1].position).normalized;


            listRopePoints[i].SetPosition(stickCenter + stickDirection * stickLength / 2);


            listRopePoints[i + 1].SetPosition(stickCenter - stickDirection * stickLength / 2);

            listRopePoints[i].UpdateCollisions();
            //listRopePoints[i + 1].UpdateCollisions();
        }

        listRopePoints[variablePointNumber - 1].UpdateCollisions();
        rightPlayer.UpdateCollisions();

        Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (rightPlayer.position + listRopePoints[variablePointNumber - 1].position) / 2 + playerRopePointRatio * differencePositionRight;
        Vector2 stickDirectionRight = (listRopePoints[variablePointNumber - 1].position - rightPlayer.position).normalized;

        listRopePoints[variablePointNumber - 1].SetPosition(stickCenterRight + stickDirectionRight * stickLength * 2.5f / 2);
        rightPlayer.SetPosition(stickCenterRight - stickDirectionRight * stickLength * 2.5f / 2);

        listRopePoints[variablePointNumber - 1].UpdateCollisions();
        rightPlayer.UpdateCollisions();
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
        leftPlayer.Actualise();
        for (int i = 0; i < variablePointNumber; i++)
        {

            //listRopePoints[i].UpdateCollisions();
            listRopePoints[i].Actualise();
        }


        //rightPlayer.UpdateCollisions();
        rightPlayer.Actualise();
    }



    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddRopeLeft();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ReduceRopeLeft();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddRopeRight();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
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
