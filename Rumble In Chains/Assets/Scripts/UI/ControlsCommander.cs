using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsCommander : Commander
{
    [SerializeField]
    GameObject controls;
    public override void execute()
    {
        controls.transform.localScale.Scale(new Vector3());
        StartCoroutine(ScaleIn(controls));
    }
     
    IEnumerator ScaleIn(GameObject gameObject)
    {
        if(gameObject.transform.localScale.x >= 1)
        {
            gameObject.transform.localScale = Vector3.one;
        }
        while (gameObject.transform.localScale != Vector3.one)
        {
            gameObject.transform.localScale += Time.deltaTime * Vector3.one;
            yield return null;
        }
    }
    
    IEnumerator ScaleOut(GameObject gameObject)
    {
        if (gameObject.transform.localScale.x <= 0)
        {
            gameObject.transform.localScale = Vector3.zero;
        }
        while (gameObject.transform.localScale != Vector3.zero)
        {
            gameObject.transform.localScale -= Time.deltaTime * Vector3.one;
            yield return null;
        }
    }

    private void Update()
    {
        if((Input.GetButtonDown("START1") || Input.GetButtonDown("START2")) && controls.activeInHierarchy)
        {
            StartCoroutine(ScaleOut(controls));
        }
    }

}
