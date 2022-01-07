using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightButtonSetter : ButtonSetter
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void setButton()
    {
        SceneManager.LoadScene("CharacterSelectionScene");
    }
}
