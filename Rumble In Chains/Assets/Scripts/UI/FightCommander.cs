using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCommander : Commander
{
    public override void execute()
    {
        GameManager.Instance.LoadScene("CharacterSelectionScene");
    }
}
