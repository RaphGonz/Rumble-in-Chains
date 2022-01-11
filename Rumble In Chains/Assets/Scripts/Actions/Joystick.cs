using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick
{
    Vector2 direction;
    float magnitude;

    const float PI1ON8 = Mathf.PI / 8;
    const float PI2ON8 = 2 * Mathf.PI / 8;
    const float PI3ON8 = 3 * Mathf.PI / 8;
    const float PI5ON8 = 5 * Mathf.PI / 8;
    const float PI6ON8 = 6 * Mathf.PI / 8;
    const float PI7ON8 = 7 * Mathf.PI / 8;
    readonly float diagValue = 1 / Mathf.Sqrt(2);



    public Vector2 GetDirection()
    {
        return direction;
    }

    public void SetDirection(Vector2 newDir)
    {
        direction = newDir.normalized;
    }

    public float GetMagnitude()
    {
        return magnitude;
    }

    public void SetMagnitude(float newMag)
    {
        magnitude = newMag;
    }

    public void SetVector(Vector2 vec)
    {
        direction = vec.normalized;
        magnitude = vec.magnitude;
    }

    public Vector2 GetVector()
    {
        return magnitude * direction;
    }

    public Vector2 getFilter4()
    {
        Vector2 direction4 = new Vector2(0, 0);

        float angle = Mathf.Atan2(direction.y, direction.x);

        if (magnitude > 0.5f)
        {
            if (angle >= -PI2ON8 && angle <= PI2ON8)
            {
                direction4.x = 1;
            }
            else if (angle >= PI2ON8 && angle <= PI6ON8)
            {
                direction4.y = 1;
            }
            else if (Mathf.Abs(angle) >= PI6ON8)
            {
                direction4.x = -1;
            }
            else if (angle >= -PI6ON8 && angle <= -PI2ON8)
            {
                direction4.y = -1;
            }   
        }
        return direction4;
    }

    public Vector2 getFilter8()
    {
        Vector2 direction8 = new Vector2(0, 0);

        float angle = Mathf.Atan2(direction.y, direction.x);

        if (magnitude > 0.5f)
        {
            if (angle >= -PI1ON8 && angle <= PI1ON8)
            {
                direction8.x = 1;
            }
            else if (angle >= PI1ON8 && angle <= PI3ON8)
            {
                direction8.x = diagValue;
                direction8.y = diagValue;
            }
            else if (angle >= PI3ON8 && angle <= PI5ON8)
            {
                direction8.y = 1;
            }
            else if (angle >= PI5ON8 && angle <= PI7ON8)
            {
                direction8.x = -diagValue;
                direction8.y = diagValue;
            }
            else if (Mathf.Abs(angle) >= PI7ON8)
            {
                direction8.x = -1;
            }
            else if (angle >= -PI7ON8 && angle <= -PI5ON8)
            {
                direction8.x = -diagValue;
                direction8.y = -diagValue;
            }
            else if (angle >= -PI5ON8 && angle <= -PI3ON8)
            {
                direction8.y = -1;
            }
            else if (angle >= -PI3ON8 && angle <= -PI1ON8)
            {
                direction8.x = diagValue;
                direction8.y = -diagValue;
            }
        }

        return direction8;
    }
}
