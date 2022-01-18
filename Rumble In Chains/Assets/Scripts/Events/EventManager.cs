using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<EventManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("EventManager");
                    _instance = container.AddComponent<EventManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    public delegate void EventAnimation(string animationName);
    public event EventAnimation eventAnimation;
    public void OnEventAnimation(string animationName)
    {
        if (eventAnimation != null)
        {
            eventAnimation(animationName);
        }
    }


    public delegate void EventSound(string soundName);
    public event EventSound eventSound;
    
    public void OnEventSound(string soundName)
    {
        if (eventSound != null)
        {
            eventSound(soundName);
        }
        
    }
    

    public delegate void EventParticle(Vector2 position, string particleName);
    public event EventParticle eventParticle;
    public void OnEventParticle(Vector2 position, string particleName)
    {
        if (eventParticle != null)
        {
            eventParticle(position, particleName);
        }
    }

    public delegate void EventRopegrab(int playerNumber);
    public event EventRopegrab eventRopegrab;
    public void OnEventRopegrab(int playerNumber)
    {
        if (eventRopegrab != null)
        {
            eventRopegrab(playerNumber);
        }
    }

    public delegate void EventDash(int playerNumber);
    public event EventDash eventDash;
    public void OnEventDash(int playerNumber)
    {
        if (eventDash != null)
        {
            eventDash(playerNumber);
        }
    }

    public delegate void EventSpawnParticles(string name, Vector2 position, bool right);
    public event EventSpawnParticles eventSpawnParticles;
    public void OnEventSpawnParticles(string name, Vector2 position, bool right)
    {
        if (eventSpawnParticles != null)
        {
            eventSpawnParticles(name, position, right);
        }
    }
}
