using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonSetter : MonoBehaviour
{

    [SerializeField]
    Button button;
    // Start is called before the first frame update
    public virtual void Start()
    {
        button.onClick.AddListener(setButton);
    }
    protected abstract void setButton();
}
