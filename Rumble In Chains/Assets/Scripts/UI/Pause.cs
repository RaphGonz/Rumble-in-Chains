using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("START1"))
        {
            if (!paused) { }//Time.timeScale = 0; paused = true; }
            else { Time.timeScale = 1; paused = false; }
        }
    }
}
