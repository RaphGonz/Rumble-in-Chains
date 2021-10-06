using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public int pointNumber = 10;
    public float stickLength;
    public float playerPointStickLength;
    public float gravity = 100f;

    public float playerRopePointRatio = 1f;

    public GameObject pointPrefab;


    List<RopePoint> listRopePoints;

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
        positionsLeft = new Vector2[pointNumber + 2];
        positionsRight = new Vector2[pointNumber + 2];

        leftPlayer = leftCharacter.GetComponent<Player>();
        rightPlayer = rightCharacter.GetComponent<Player>();

        listRopePoints = new List<RopePoint>();
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
        //UpdatePlayerLeft();
        SimulatePoints();
        DisplayPoints();
        //UpdatePlayerRight();
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
        //leftPlayer.TranslatePosition(Vector2.down * 2 * gravity * Time.deltaTime * Time.deltaTime);
        leftPlayer.previousPosition = positionBeforeUpdateLeft;

        Vector2 positionBeforeUpdateRight = rightPlayer.transform.position;
        UpdatePlayerRight();
        //rightPlayer.TranslatePosition(Vector2.down * 2 * gravity * Time.deltaTime * Time.deltaTime);
        rightPlayer.previousPosition = positionBeforeUpdateRight;


        for (int i = 0; i < pointNumber; i++)
        {
            Vector2 positionBeforeUpdate = listRopePoints[i].transform.position;
            listRopePoints[i].TranslatePosition(listRopePoints[i].position - listRopePoints[i].previousPosition);
            listRopePoints[i].TranslatePosition(Vector2.down * gravity * Time.deltaTime * Time.deltaTime);
            listRopePoints[i].previousPosition = positionBeforeUpdate;
        }

        Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition;

        Vector2 stickCenterLeft = (leftPlayer.position + listRopePoints[0].position) / 2 + playerRopePointRatio * differencePositionLeft;
        Vector2 stickDirectionLeft = (leftPlayer.position - listRopePoints[0].position).normalized;

        leftPlayer.SetPosition(stickCenterLeft + stickDirectionLeft * playerPointStickLength / 2);
        listRopePoints[0].SetPosition(stickCenterLeft - stickDirectionLeft * playerPointStickLength / 2);


        for (int i = 0; i < pointNumber - 1; i++)
        {
            Vector2 stickCenter = (listRopePoints[i].position + listRopePoints[i + 1].position) / 2;
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i + 1].position).normalized;
            
            listRopePoints[i].SetPosition(stickCenter + stickDirection * stickLength / 2);

            
            listRopePoints[i+1].SetPosition(stickCenter - stickDirection * stickLength / 2);
        }

        Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (rightPlayer.position + listRopePoints[pointNumber - 1].position) / 2 + playerRopePointRatio * differencePositionRight;
        Vector2 stickDirectionRight = (listRopePoints[pointNumber - 1].position - rightPlayer.position).normalized;

        listRopePoints[pointNumber - 1].SetPosition(stickCenterRight + stickDirectionRight * playerPointStickLength / 2);
        rightPlayer.SetPosition(stickCenterRight - stickDirectionRight * playerPointStickLength / 2);

        positionsLeft[0] = leftPlayer.position;
        for (int i = 0; i < pointNumber; i++)
        {
            positionsLeft[i + 1] = listRopePoints[i].position;
        }
        positionsLeft[positionsRight.Length - 1] = rightPlayer.position;
    }

    /*
    private void LeftComputePosition()
    {
        Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition;

        Vector2 stickCenterLeft = (leftPlayer.position + listRopePoints[0].position) / 2 + playerRopePointRatio * differencePositionLeft;
        Vector2 stickDirectionLeft = (leftPlayer.position - listRopePoints[0].position).normalized;

        leftPlayer.SetPosition(stickCenterLeft + stickDirectionLeft * stickLength / 2);
        listRopePoints[0].SetPosition(stickCenterLeft - stickDirectionLeft * stickLength / 2);


        for (int i = 0; i < pointNumber - 1; i++)
        {
            Vector2 stickCenter = (listRopePoints[i].position + listRopePoints[i + 1].position) / 2;
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i + 1].position).normalized;
          
       
            listRopePoints[i].SetPosition(stickCenter + stickDirection * stickLength / 2);
           

            listRopePoints[i + 1].SetPosition(stickCenter - stickDirection * stickLength / 2);
        }

        Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (rightPlayer.position + listRopePoints[pointNumber - 1].position) / 2 + playerRopePointRatio * differencePositionRight;
        Vector2 stickDirectionRight = (listRopePoints[pointNumber - 1].position - rightPlayer.position).normalized;

        listRopePoints[pointNumber - 1].SetPosition(stickCenterRight + stickDirectionRight * stickLength / 2);
        rightPlayer.SetPosition(stickCenterRight - stickDirectionRight * stickLength / 2);

        positionsLeft[0] = leftPlayer.position;
        for (int i = 0; i < pointNumber; i++)
        {
            positionsLeft[i + 1] = listRopePoints[i].position;
        }
        positionsLeft[positionsRight.Length - 1] = rightPlayer.position;
    }

    private void RightComputePosition()
    {
        Vector2 differencePositionRight = rightPlayer.position - rightPlayer.previousPosition;

        Vector2 stickCenterRight = (rightPlayer.position + listRopePoints[pointNumber - 1].position) / 2 + playerRopePointRatio * differencePositionRight;
        Vector2 stickDirectionRight = (rightPlayer.position - listRopePoints[pointNumber - 1].position).normalized;

        rightPlayer.SetPosition(stickCenterRight + stickDirectionRight * stickLength / 2);
        listRopePoints[pointNumber - 1].SetPosition(stickCenterRight - stickDirectionRight * stickLength / 2);


        for (int i = pointNumber - 1; i > 0; i++)
        {
            Vector2 stickCenter = (listRopePoints[i].position + listRopePoints[i - 1].position) / 2;
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i - 1].position).normalized;
            
            listRopePoints[i].SetPosition(stickCenter + stickDirection * stickLength / 2);
            

            listRopePoints[i + 1].SetPosition(stickCenter - stickDirection * stickLength / 2);
        }

        
        Vector2 differencePositionLeft = leftPlayer.position - leftPlayer.previousPosition;

        Vector2 stickCenterLeft = (leftPlayer.position + listRopePoints[0].position) / 2 + playerRopePointRatio * differencePositionLeft;
        Vector2 stickDirectionLeft = (listRopePoints[0].position - leftPlayer.position).normalized;

        listRopePoints[0].SetPosition(stickCenterLeft + stickDirectionLeft * stickLength / 2);
        leftPlayer.SetPosition(stickCenterLeft - stickDirectionLeft * stickLength / 2);



        positionsLeft[0] = leftPlayer.position;
        for (int i = 0; i < pointNumber; i++)
        {
            positionsLeft[i + 1] = listRopePoints[i].position;
        }
        positionsLeft[positionsRight.Length - 1] = rightPlayer.position;
    }

    */

    void DisplayPoints()
    {
        leftPlayer.Actualise();
        for (int i = 0; i < pointNumber; i++)
        {
            listRopePoints[i].Actualise();
        }
        //listRopePoints[0].position = listRopeGameobject[0].transform.position;

        rightPlayer.Actualise();

        //leftPlayer.previousPosition = leftPlayer.transform.position;
        //rightPlayer.previousPosition = rightPlayer.transform.position;

        
    }

    
}
