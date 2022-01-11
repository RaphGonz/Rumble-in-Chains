using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterCommander : Commander
{
    [SerializeField]
    int player;
    [SerializeField]
    string character;
    public override void execute()
    {
        GameManager.Instance.setCharacter(character, player);
    }
}
