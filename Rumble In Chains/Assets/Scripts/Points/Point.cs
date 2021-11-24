using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField]
    public int pointType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (pointType == 1)
                PointsController.Instance.add(1);
            else
                PointsController.Instance.add(2);
        }
    }
}
