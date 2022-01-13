using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterConverter : MonoBehaviour
{
    public float convertSpeed(Speed force)
    {
        float speed = 0;
        switch (force)
        {
            case Speed.LOW:
                speed = 1;
                break;
            case Speed.MEDIUM:
                speed = 1.25f;
                break;
            case Speed.HIGH:
                speed = 1.5f;
                break;
        }

        return speed;
    }
    public float convertWeight(Weight force)
    {
        float weight = 0;
        switch (force)
        {
            case Weight.LOW:
                weight = 1;
                break;
            case Weight.MEDIUM:
                weight = 2;
                break;
            case Weight.HIGH:
                weight = 3;
                break;
        }

        return weight;
    }

    public float convertJumpHeight(JumpHeight force)
    {
        float distance = 0;
        switch (force)
        {
            case JumpHeight.LOW:
                distance = 5;
                break;
            case JumpHeight.MEDIUM:
                distance = 6.5f;
                break;
            case JumpHeight.HIGH:
                distance = 8;
                break;
        }

        return distance;
    }

    public float convertDashActivation(DashActivation force)
    {
        float prelag = 0;
        switch (force)
        {
            case DashActivation.LOW:
                prelag = 0.2f;
                break;
            case DashActivation.MEDIUM:
                prelag = 0.3f;
                break;
            case DashActivation.HIGH:
                prelag = 0.4f;
                break;
        }

        return prelag;
    }
    public float convertDashDistance(DashDistance force)
    {
        float distance = 0;
        switch (force)
        {
            case DashDistance.LOW:
                distance = 3;
                break;
            case DashDistance.MEDIUM:
                distance = 5;
                break;
            case DashDistance.HIGH:
                distance = 7;
                break;
        }

        return distance;
    }

    public float convertRopegrabAngle(RopePulling force)
    {
        float angle = 0;
        switch (force)
        {
            case RopePulling.LOW:
                angle = 90;
                break;
            case RopePulling.MEDIUM:
                angle = 135;
                break;
            case RopePulling.HIGH:
                angle = 180;
                break;
        }

        return angle;
    }

    public float convertRopegrabDistance(RopePulling force)
    {
        float relativeDistance = 0;
        switch (force)
        {
            case RopePulling.LOW:
                relativeDistance = 0.5f;
                break;
            case RopePulling.MEDIUM:
                relativeDistance = 0.75f;
                break;
            case RopePulling.HIGH:
                relativeDistance = 1;
                break;
        }

        return relativeDistance;
    }
}
