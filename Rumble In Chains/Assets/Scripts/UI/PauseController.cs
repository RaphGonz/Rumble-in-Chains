using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseCanvas;
    void Update()
    {
        if ((Input.GetButtonDown("START1") || Input.GetButtonDown("START2")) && !pauseCanvas.activeInHierarchy)
        {
            if (pauseCanvas.transform.localScale.x == 0)
            {
                pauseCanvas.SetActive(true);
                StartCoroutine(ScaleIn(pauseCanvas));
                Time.timeScale = 0;
            }
            else if (pauseCanvas.transform.localScale.x == 1)
            {
                StartCoroutine(ScaleOut(pauseCanvas));
                pauseCanvas.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    IEnumerator ScaleIn(GameObject gameObject)
    {
        while (gameObject.transform.localScale.x <= 1)
        {
            gameObject.transform.localScale += Time.deltaTime * Vector3.one;
            yield return null;
        }
        if (gameObject.transform.localScale.x >= 1)
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }

    IEnumerator ScaleOut(GameObject gameObject)
    {
        while (gameObject.transform.localScale.x >= 0)
        {
            gameObject.transform.localScale -= Time.deltaTime * Vector3.one;
            yield return null;
        }
        if(gameObject.transform.localScale.x <= 0)
        {
            gameObject.transform.localScale = Vector3.zero;
        }
    }
}
