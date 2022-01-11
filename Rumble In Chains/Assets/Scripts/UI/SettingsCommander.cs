using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCommander : Commander
{
    [SerializeField]
    GameObject options;
    public override void execute()
    {
        options.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetButtonDown("START1") || Input.GetButtonDown("START2"))
        {
            options.SetActive(options.activeSelf);
        }
    }
}
