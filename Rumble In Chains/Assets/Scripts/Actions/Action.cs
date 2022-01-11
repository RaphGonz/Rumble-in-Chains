using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    protected Timer timer1 = new Timer();
    protected Timer timer2 = new Timer();
    protected Timer timer3 = new Timer();

    protected Timer cooldown = new Timer();

    private bool firstCall = true;



    // Start is called before the first frame update
    virtual public void start()
    {
        cooldown.start();
    }

    // Update is called once per frame
    virtual public bool update()
    {
        return false;
    }

    public bool getCooldown()
    {
        if (firstCall)
        {
            firstCall = false;
            return true;
        }
        else
        {
            return cooldown.check();
        }
    }

    virtual public void cancel() {}
}
