using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct InputTime
{
    public InputButtons input;
    public float startTime;

    public InputTime(InputButtons newInput = InputButtons.NULL, float newTime = 0)
    {
        this.input = newInput;
        this.startTime = newTime;
    }

}

public class BufferManager : MonoBehaviour
{
    int bufferLength = 5;
    private float lifeTime = 0.2f;
    InputTime[] buffer;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        buffer = new InputTime[bufferLength];

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = new InputTime();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addToBuffer(InputButtons input)
    {
        removeOutOfDate();
        if (buffer[buffer.Length - 1].input == InputButtons.NULL)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].input == InputButtons.NULL)
                {
                    buffer[i] = new InputTime(input, Time.time);
                    return;
                }
            }
        }
        else
        {
            popBuffer();
            buffer[buffer.Length - 1] = new InputTime(input, Time.time);
        }
        
    }

    public void popBuffer()
    {
        removeOutOfDate();
        for (int i = 0; i < buffer.Length - 1; i++)
        {
            if (buffer[i].input == InputButtons.NULL)
            {
                return;
            }
            else
            {
                buffer[i] = buffer[i + 1];
            }
                
        }

        buffer[bufferLength - 1] = new InputTime();
    }


    public InputButtons getBufferElement()
    {
        removeOutOfDate();
        return buffer[0].input;
    }

    private void removeOutOfDate()
    {
        int i = 0;
        while (buffer[0].input != InputButtons.NULL && Time.time - buffer[0].startTime > lifeTime && i < bufferLength)
        {
            i++;
            popBuffer();
        }
    }

    public void setDirection(Vector2 newDir)
    {
        direction = newDir;
    }

    public Vector2 getDirection()
    {
        return direction;
    }
}
