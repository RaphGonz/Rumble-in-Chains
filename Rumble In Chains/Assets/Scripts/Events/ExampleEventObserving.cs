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
        EventManager.Instance.eventSpawnParticles += SpawnParticle;
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

    private void SpawnParticle(int id, Vector2 pos, bool right)
    {
        Debug.Log("particle spawned, id : " + id + " ;   position : " + pos + ";    orientation : " + right);
    }
}
