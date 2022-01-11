using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueCommander : Commander
{
    [SerializeField]
    GameObject pauseObject;
    public override void execute()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1;
    }
}
