using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicAction : Action
{
    [SerializeField] private float preLag;
    [SerializeField] private float postLag;
    [SerializeField] private float cooldownBetweenTwoPoses;

    [SerializeField] CharacterController characterController;



    private void Start()
    {
        timer1.setDuration(preLag);
        timer2.setDuration(postLag);
        cooldown.setDuration(cooldownBetweenTwoPoses);
    }


    // Start is called before the first frame update
    override public void start()
    {
        timer1.start();
        cooldown.start();
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            if (timer1.check())
            {
                timer1.reset();
                timer2.start();
                gainPoint();
            }
            return false;
        }
        if (timer2.isActive())
        {
            if (timer2.check())
            {
                timer2.reset();
            }
            return false;
        }

        return true;
    }

    private void gainPoint()
    {
        characterController.Points++;
        print(characterController.Points);
    }



    override public void cancel()
    {
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
