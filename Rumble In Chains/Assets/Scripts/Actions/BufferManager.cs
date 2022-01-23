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
    public bool shieldButton = false;

    private Vector2 direction;
    private Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        buffer = new InputTime[bufferLength];
        joystick = new Joystick();

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
        //print(buffer[0].input + " " + buffer[1].input + " " + buffer[2].input + " " + buffer[3].input + " " + buffer[4].input);
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

    public void setJoystick(Vector2 newValue)
    {
        joystick.SetVector(newValue);
    }

    public Joystick getJoystick()
    {
        return joystick;
    }

    public void setShieldButton(bool value)
    {
        shieldButton = value;
    }

    public bool getShieldButton()
    {
        return shieldButton;
    }
}
