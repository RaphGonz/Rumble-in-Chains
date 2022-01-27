using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireActivation : MonoBehaviour
{
    [SerializeField] GameObject fire1;
    [SerializeField] GameObject fire2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            fire1.SetActive(!fire1.activeSelf);
            fire2.SetActive(!fire2.activeSelf);
        }
    }
}
