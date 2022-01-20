using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : Action
{
    [SerializeField] PlayerController playerController;

    [SerializeField] private float shieldTime;
    [SerializeField] private float shieldCooldown;

    public Material shieldMaterial;
    private float height = 3f; //Valeur qui nous permet de faire slider le reflet metallique de haut en bas sur le personnage

    private void Start()
    {
        timer1.setDuration(shieldTime);
        cooldown.setDuration(shieldCooldown);
        //Au niveau du shader pour le bouclier : on se transforme en gris
        shieldMaterial.SetInt("_isShielding", 1);
        shieldMaterial.SetFloat("_Height", height);
    }


    // Start is called before the first frame update
    override public void start()
    {
        timer1.start();
        shieldMaterial.SetInt("_isShielding", 0);
        cooldown.start();
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            phase1Shield();

            height -= 3*Time.deltaTime/shieldTime;
            if (height <= 0) height += 3; //On fait boucler height sur lui même
            shieldMaterial.SetFloat("_Height", height);

            return false;
        }


        return true;
    }

    private void phase1Shield()
    {
        if (!timer1.check())
        {
            playerController.velocity = new Vector2(0, 0);
        }
        else
        {
            timer1.reset();
            shieldMaterial.SetInt("_isShielding", 1);
        }
    }

    override public void cancel()
    {
        //playerController.velocity = new Vector2(0, 0);
        timer1.reset();
        timer2.reset();
        timer3.reset();
        shieldMaterial.SetInt("_isShielding", 1);
    }
}
