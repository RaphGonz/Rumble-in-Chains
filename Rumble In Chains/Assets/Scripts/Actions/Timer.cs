using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    float startTime;
    float duration;

    bool active = false;

    public void start()
    {
        active = true;
        startTime = Time.time;
    }

    public bool check()
    {
        if (active && Time.time - startTime > duration)
        {
            return true;
        }
        return false;
    }

    public bool isActive()
    {
        return active;
    }

    public void setDuration(float dur)
    {
        duration = dur;
    }

    public void reset()
    {
        active = false;
    }

    public float getRatio()
    {
        float ratio = (Time.time - startTime) / duration;
        if (ratio < 1)
        {
            return ratio;
        }
        else
        {
            return 1;
        }
    }
}
