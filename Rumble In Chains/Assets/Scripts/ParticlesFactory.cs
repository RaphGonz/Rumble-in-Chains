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
        foreach(var particleSys in particlesSystems)
        {
            GameObject go = Instantiate(particleSys);
            go.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
