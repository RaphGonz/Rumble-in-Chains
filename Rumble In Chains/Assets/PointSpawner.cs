using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    private static PointSpawner _instance;
    public static PointSpawner Instance { get; private set; }

    PointSpawner() { }

    [SerializeField]
    private GameObject point1;
    [SerializeField]
    private GameObject point2;
    private Queue<GameObject> points;
    private List<GameObject> pointsOnScene;
    private List<Rect> rectsToAvoid;
    private Queue<Rect> emptyPlaces;
    [SerializeField]
    private Vector2 currentPos;

    [SerializeField]
    private int numberOfPointsPerPlayer;
    [SerializeField]
    private bool waves;
    [SerializeField]
    private int maxNumberOfPointsOnScene;
    private int pointsToSpawn1;
    private int pointsToSpawn2;

    private void Awake()
    {
        emptyPlaces = new Queue<Rect>();
        rectsToAvoid = new List<Rect>();
        points = new Queue<GameObject>();
        pointsOnScene = new List<GameObject>();
        pointsToSpawn1 = maxNumberOfPointsOnScene;
        pointsToSpawn2 = maxNumberOfPointsOnScene;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        int firstWave = waves ? maxNumberOfPointsOnScene : numberOfPointsPerPlayer;
        for (int i = 0; i <= firstWave; i++)
        {
            //Coroutine pour timer ?
            //Randomiser l'ordre des deux
            Spawn(1);
            Spawn(2);
        }

        for(int i = 0; i < maxNumberOfPointsOnScene; i++)
        {
            foreach(Rect rect in rectsToAvoid)
            {
                int x = maxNumberOfPointsOnScene / 4;
                int y = maxNumberOfPointsOnScene % 8; 
                /*if (!inRect(rect,))
                {

                }*/
            }
            
        }
        
    }

    void Spawn(int point)
    {
        if (points.Count == 0)
        {
            pointsOnScene.Add(Instantiate(point == 1 ? point1 : point2, computePosition(), new Quaternion()));
        }
        else
        {
            points.Dequeue().SetActive(true);
        }
    }

    public void Despawn(GameObject point)
    {
        pointsOnScene.Remove(point);
        point.SetActive(false);
        points.Enqueue(point);
        emptyPlaces.Enqueue(new Rect(point.transform.position));
        if (point.GetComponent<Point>().pointType == 1 && pointsToSpawn1 != 0)
        {
            Spawn(1);
        }
        else if (point.GetComponent<Point>().pointType == 2 && pointsToSpawn2 != 0)
        {
            Spawn(2);
        }
    }

    private Vector3 computePosition()
    {
        return emptyPlaces.Dequeue().toVector2();

        Random.Range(-4, 4);

        
        Vector3 posToReturn = new Vector3();
        return posToReturn;
    }
    private struct Rect
    {
        public Vector2 topRight;
        public Vector2 botLeft;

        public Rect(Vector2 tr, Vector2 bl)
        {
            topRight = tr;
            botLeft = bl;
        }
        public Rect(Vector2 vec)
        {
            topRight = new Vector2(vec.x - 0.5f, vec.y - 0.5f);
            botLeft = new Vector2(vec.x + 0.5f, vec.y + 0.5f);
        }
        public Vector2 toVector2()
        {
            return new Vector2(topRight.x - 0.5f, topRight.y - 0.5f);
        }
    }

    private bool inRect(Point point, Rect rect)
    {
        Vector2 pos = point.transform.position;
        if (pos.x >= rect.botLeft.x && pos.y >= rect.botLeft.y && pos.x <= rect.topRight.x && pos.y <= rect.topRight.y)
        {
            return true;
        }
        return false;
    }
}
