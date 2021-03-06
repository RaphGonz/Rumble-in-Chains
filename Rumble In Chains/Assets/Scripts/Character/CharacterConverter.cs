using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterConverter
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
                weight = 1f;
                break;
            case Weight.MEDIUM:
                weight = 1.25f;
                break;
            case Weight.HIGH:
                weight = 1.5f;
                break;
        }

        return weight;
    }

    public int convertWeightRope(Weight force)
    {
        int weight = 0;
        switch (force)
        {
            case Weight.LOW:
                weight = 12;
                break;
            case Weight.MEDIUM:
                weight = 6;
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
                distance = 2.5f;
                break;
            case DashDistance.MEDIUM:
                distance = 3.5f;
                break;
            case DashDistance.HIGH:
                distance = 4.5f;
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
                relativeDistance = 0.6f;
                break;
            case RopePulling.MEDIUM:
                relativeDistance = 0.8f;
                break;
            case RopePulling.HIGH:
                relativeDistance = 1;
                break;
        }

        return relativeDistance;
    }
}
