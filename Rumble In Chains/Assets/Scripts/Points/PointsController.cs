using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    private static PointsController _instance;
    private PointsController(){}
    public static PointsController Instance { get; private set; }

    int points1 = 0;
    int points2 = 0;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    public void add(int player)
    {
        if (player == 1)
            points1++; 
        else 
            points2++;
    }
}
