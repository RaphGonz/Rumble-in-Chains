using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseCanvas;
    bool pauseActivated;
    private void Start()
    {
        pauseActivated = !pauseCanvas.activeInHierarchy;
    }
    void Update()
    {
        if ((Input.GetButtonDown("START1") || Input.GetButtonDown("START2")) && pauseActivated)
        {
            if (pauseCanvas.transform.localScale.x == 0)
            {
                pauseCanvas.SetActive(true);
                StartCoroutine(ScaleIn(pauseCanvas));
                
            }
            else if (pauseCanvas.transform.localScale.x == 1)
            {
                StartCoroutine(ScaleOut(pauseCanvas));
                
                Time.timeScale = 1;
            }
        }
    }

    IEnumerator ScaleIn(GameObject gameObject)
    {
        while (gameObject.transform.localScale.x <= 1)
        {
            gameObject.transform.localScale += Time.deltaTime * 3 * Vector3.one;
            yield return null;
        }
        if (gameObject.transform.localScale.x >= 1)
        {
            gameObject.transform.localScale = Vector3.one;
        }
        Time.timeScale = 0;
    }

    IEnumerator ScaleOut(GameObject gameObject)
    {
        while (gameObject.transform.localScale.x >= 0)
        {
            gameObject.transform.localScale -= Time.deltaTime * 3 * Vector3.one;
            yield return null;
        }
        if(gameObject.transform.localScale.x <= 0)
        {
            gameObject.transform.localScale = Vector3.zero;
        }
        pauseCanvas.SetActive(false);
    }
}
