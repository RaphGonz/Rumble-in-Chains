using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
    int length = 20;
    float initialSize = 9.81f;
    float cameraSize = 9.81f;
    float targetSize = 9.81f;
    float minRatio = 0.8f;

    float minXposition;
    float maxXposition;

    float minYposition;
    float maxYposition;

    Vector2 mean;
    List<Vector2> lastPositions;

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        lastPositions = new List<Vector2>(length);
        for (int i = 0; i < length; i++)
        {
            lastPositions.Add((player1.position + player2.position) / 2);
        }
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        computeNewPosition();
        print(mean);

        changeSizeToTarget();
    }

    void computeNewPosition()
    {
        mean -= lastPositions[0] / length;

        lastPositions.RemoveAt(0);
        lastPositions.Add((player1.position + player2.position) / 2);

        mean += lastPositions[length - 1] / length;
    }

    void changeSizeToTarget()
    {
        if (cameraSize > targetSize)
        {
            ReduceSize();
        }
        else if (cameraSize < targetSize)
        {
            IncreaseSize();
        }
    }

    void ReduceSize()
    {
        cameraSize -= initialSize / 100;
        if (cameraSize < minRatio * initialSize)
        {
            cameraSize = minRatio * initialSize;
        }
    }

    void IncreaseSize()
    {
        cameraSize += initialSize / 100;
        if (cameraSize > initialSize)
        {
            cameraSize = initialSize;
        }
    }
}
