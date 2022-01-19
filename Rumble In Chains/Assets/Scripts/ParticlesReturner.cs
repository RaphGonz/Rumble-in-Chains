using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesReturner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public ParticlesFactory factory;
    [SerializeField]
    int numberOfThisGoInTheFactory;
    [SerializeField]
    float durationOfThisParticleSystem;
    float time = 0;
    void Update()
    {
        if (time > durationOfThisParticleSystem || time > 1)
        {
            factory.ReturnParticleSystem(this.gameObject, numberOfThisGoInTheFactory);
        }
        time += Time.deltaTime;
    }
    
}
