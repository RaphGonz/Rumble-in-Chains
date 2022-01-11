using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCommander : Commander
{
    public override void execute()
    {
        Application.Quit();
    }

}
