using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStunSlider : MonoBehaviour
{
    [SerializeField]
    Slider stunSlider;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaxStunValue(int value)
    {
        stunSlider.maxValue = value;
        
    }

    public void ChangeStunValue(int value)
    {
        stunSlider.value = value;
        print(stunSlider.maxValue);
    }
}
