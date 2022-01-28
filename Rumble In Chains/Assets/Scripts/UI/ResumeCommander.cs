using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeCommander : Commander
{
    [SerializeField]
    GameObject pauseCanvas;
    public override void execute()
    {
        StartCoroutine(ScaleOut(pauseCanvas));
        Time.timeScale = 1;
    }

    IEnumerator ScaleOut(GameObject gameObject)
    {
        while (gameObject.transform.localScale.x >= 0)
        {
            gameObject.transform.localScale -= Time.deltaTime * Vector3.one;
            yield return null;
        }
        if (gameObject.transform.localScale.x <= 0)
        {
            gameObject.transform.localScale = Vector3.zero;
        }
    }
}
