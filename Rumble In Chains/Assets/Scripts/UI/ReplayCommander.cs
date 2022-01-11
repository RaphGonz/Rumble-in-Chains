using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCommander : Commander
{
    public override void execute()
    {
        GameManager.Instance.LoadScene("FightScene");
    }
}
