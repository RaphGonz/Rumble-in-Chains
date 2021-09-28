using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public int pointNumber = 10;
    public float stickLength;
    public float gravity = 100f;

    public GameObject pointPrefab;

    List<GameObject> listRopeGameobject;
    List<RopePoint> listRopePoints;

    public GameObject leftCharacter;
    public GameObject rightCharacter;
    //public RopePoint rightCharacter;

    Vector2 startPosition = new Vector2(5, 10);

    void Start()
    {
        listRopeGameobject = new List<GameObject>();
        listRopePoints = new List<RopePoint>();
        Vector2 position = startPosition;

        for (int i = 0; i < pointNumber; i++)
        {
            GameObject point = Instantiate(pointPrefab, position, Quaternion.Euler(0, 0, 0));
            listRopeGameobject.Add(point);
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
        SimulatePoints();
        DisplayPoints();

    }

    void SimulatePoints()
    {
        for (int i = 1; i < pointNumber; i++)
        {
            Vector2 positionBeforeUpdate = listRopePoints[i].position;
            listRopePoints[i].position += (listRopePoints[i].position - listRopePoints[i].previousPosition);
            listRopePoints[i].position += Vector2.down * gravity * Time.deltaTime * Time.deltaTime;
            listRopePoints[i].previousPosition = positionBeforeUpdate;
        }

        for (int i = 0; i < pointNumber - 1; i++)
        {
            Vector2 stickCenter = (listRopePoints[i].position + listRopePoints[i + 1].position) / 2;
            Vector2 stickDirection = (listRopePoints[i].position - listRopePoints[i + 1].position).normalized;
            if (!(i == 0))
            {
                listRopePoints[i].position = stickCenter + stickDirection * stickLength / 2;
            }
            
            listRopePoints[i+1].position = stickCenter - stickDirection * stickLength / 2;
        }
    }

    void DisplayPoints()
    {
        for (int i = 1; i < pointNumber; i++)
        {
            listRopePoints[i].Actualise();
        }
        listRopePoints[0].position = listRopeGameobject[0].transform.position;
    }

    
}
