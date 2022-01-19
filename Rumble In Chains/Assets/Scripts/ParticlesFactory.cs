using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFactory : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> particlesSystems;

    List<Queue<GameObject>> factories;

    private void Awake()
    {
        factories = new List<Queue<GameObject>>();
    }
    private void Start()
    {
        for (int i = 0; i < particlesSystems.Count; i++)
        {
            GameObject go = Instantiate(particlesSystems[i], new Vector3(-24, 0, 0), Quaternion.identity);
            go.GetComponent<ParticlesReturner>().factory = this;
            go.SetActive(false);
            factories[i].Enqueue(go);
        }

        EventManager.Instance.eventSpawnParticles += SpawnParticleSystem;

    }

    public void SpawnParticleSystem(int numberInTheList, Vector2 position, bool TurnedTowardsRight)
    {
        GameObject go;
        if (factories[numberInTheList].Count != 0)
        {
            go = factories[numberInTheList].Dequeue();
        }
        else
        {
            go = Instantiate(particlesSystems[numberInTheList]);
            go.SetActive(false);
        }
        go.transform.position = position;
        if (!TurnedTowardsRight)
        {
            go.transform.position.Scale(new Vector3(-1, 0, 0));
        }
        go.SetActive(true);
    }

    public void ReturnParticleSystem(GameObject go, int numberInTheList)
    {
        go.SetActive(false);
        factories[numberInTheList].Enqueue(go);
    }
}
