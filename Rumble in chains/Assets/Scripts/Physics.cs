using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    //float timeInFall = 0;
    [SerializeField]
    float maxYSpeed = -1;
    float ySpeed ;
    //bool inFall = true;
    Vector3 movement;
    // Gravité
    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3();
        ySpeed = maxYSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        movement.y = ySpeed * Time.deltaTime;
        transform.Translate(movement);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Contact!");
        if (collision.gameObject.CompareTag("Platform"))
        {
            //timeInFall = 0;
            //inFall = false;
            ySpeed = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            //timeInFall = 0;
            //inFall = false;
            ySpeed = maxYSpeed;
        }
    }

}
