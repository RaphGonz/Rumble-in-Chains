using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    protected Timer timer1;
    protected Timer timer2;
    protected Timer timer3;

    protected Timer cooldown;

    



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
        return cooldown.check();
    }

    virtual public void cancel() {}
}
