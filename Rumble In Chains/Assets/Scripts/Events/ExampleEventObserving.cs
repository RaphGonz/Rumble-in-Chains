using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEventObserving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.eventSound += LaunchSound;
        EventManager.Instance.eventParticle += LaunchParticle;
    }
    void OnDestroy()
    {
        EventManager.Instance.eventSound -= LaunchSound;
        EventManager.Instance.eventParticle -= LaunchParticle;
    }


    private void LaunchSound(string s)
    {
        Debug.Log("Launch Sound named : " + s);
    }

    private void LaunchParticle(Vector2 position, string s)
    {
        Debug.Log("Launch Particles named : " + s);
        transform.position = position;
    }
}
