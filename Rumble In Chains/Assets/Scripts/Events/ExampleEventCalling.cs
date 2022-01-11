using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEventCalling : MonoBehaviour
{
    private Vector2 position;
    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(0, -4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            position.y += 1;
            launchParticles();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            launchSound();
        }
    }


    void launchParticles()
    {
        EventManager.Instance.OnEventParticle(position, "particules nombre 1");
    }

    void launchSound()
    {
        EventManager.Instance.OnEventSound("son N°1");
    }
}
