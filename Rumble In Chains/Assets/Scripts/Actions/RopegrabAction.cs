using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum RopegrabType
{
    NEUTRAL,
    UP,
    DOWN
}

public class RopegrabAction : Action
{

    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerController enemyPlayerController;
    [SerializeField] RopeManager ropeManager;
    [SerializeField] PlayerAnimation playerAnimation;

    int playerNumber;

    [SerializeField] private float maxGrabAngle;
    private float maxGrabRelativeDistance;

    private float currentGrabAngle = 0;
    private float initialGrabAngle = 0;
    private float finalGrabAngle = 0;
    private Vector2 joystickDirection;
    private RopegrabType type;


    [SerializeField] private float ropegrabFreezeTime;
    [SerializeField] private float ropegrabGrabTime;
    [SerializeField] private float ropegrabCooldown;


    private void Start()
    {
        Character character = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + (this.gameObject.layer == 17 ? GameManager.Instance.characterPlayer1 : GameManager.Instance.characterPlayer2) + ".asset");
        maxGrabAngle = character.characterConverter.convertRopegrabAngle(character.ropePulling);
        maxGrabRelativeDistance = character.characterConverter.convertRopegrabDistance(character.ropePulling);


        timer1.setDuration(ropegrabFreezeTime);
        timer2.setDuration(ropegrabGrabTime);
        cooldown.setDuration(ropegrabCooldown);
    }


    // Start is called before the first frame update
    public void start(Vector2 direction, int newPlayerNumber)
    {
        joystickDirection = direction;
        playerNumber = newPlayerNumber;

        type = getTypeOfRopegrab();


        Vector2 directionPlayers = enemyPlayerController.position - playerController.position;
        initialGrabAngle = Mathf.Rad2Deg * Mathf.Atan2(directionPlayers.y, directionPlayers.x);
        if (type == RopegrabType.UP)
        {
            finalGrabAngle = initialGrabAngle + maxGrabAngle;
        }
        else
        {
            finalGrabAngle = initialGrabAngle - maxGrabAngle;
        }
        playerAnimation.AnimationState = AnimationState.FOCUSROPEGRAB;




        timer3.setDuration(0);
        timer1.start();
        cooldown.start();
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            phase1Freeze();
            return false;
        }
        else if (timer2.isActive())
        {
            EventManager.Instance.OnEventRopegrab(playerNumber);
            if (type == RopegrabType.NEUTRAL)
            {
                phase2Attraction();
            }
            else
            {
                phase2GrabWithAdjustment();
            }
            return false;
        }
        else if (timer3.isActive())
        {
            EventManager.Instance.OnEventRopegrab(playerNumber);
            phase3PrelagIfCollision();
            return false;
        }

        return true;
    }

    private void phase1Freeze()
    {
        if (!timer1.check())
        {
            playerController.velocity = new Vector2(0, 0);
        }
        else
        {
            timer2.start();
            timer1.reset();
            if (type == RopegrabType.NEUTRAL)
            {
                playerAnimation.AnimationState = AnimationState.BACKROPEGRAB;
                ropeManager.startRopeAttraction(playerNumber, maxGrabRelativeDistance);
            }
            else
            {
                if (type == RopegrabType.UP)
                {
                    playerAnimation.AnimationState = AnimationState.UPROPEGRAB;
                }
                else
                {
                    playerAnimation.AnimationState = AnimationState.DOWNROPEGRAB;
                }

                ropeManager.startRopeGab(new Vector2(Mathf.Cos(Mathf.Deg2Rad * initialGrabAngle), Mathf.Sin(Mathf.Deg2Rad * initialGrabAngle)));
            }
            currentGrabAngle = 0;
        }
    }

    private void phase2GrabWithAdjustment()
    {
        if (!timer2.check())
        {
            playerController.velocity = new Vector2(0, 0);
            currentGrabAngle = timer2.getRatio() * finalGrabAngle + (1 - timer2.getRatio()) * initialGrabAngle;
            Vector2 newDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * currentGrabAngle), Mathf.Sin(Mathf.Deg2Rad * currentGrabAngle));

            
            if (ropeManager.UpdateRopegrabValues(playerNumber, newDir, timer2.getRatio()))
            {
                /*
                timer3.setDuration(ropegrabGrabTime * (1 - timer2.getRatio()));
                timer3.start();
                timer2.reset();
                ropeManager.endRopeGrab();
                */
            }
            

        }
        else
        {
            timer3.start();
            timer2.reset();
            ropeManager.endRopeGrab();
            playerAnimation.AnimationState = AnimationState.IDLE;
        }
    }


    private void phase2Attraction()
    {
        if (!timer2.check())
        {
            playerController.velocity = new Vector2(0, 0);

            if (ropeManager.attractPoints(playerNumber, timer2.getRatio(), maxGrabRelativeDistance))
            {
                timer3.setDuration(ropegrabGrabTime * (1 - timer2.getRatio()));
                timer3.start();
                timer2.reset();
                ropeManager.endRopeAttraction();
            }
        }
        else
        {
            timer3.start();
            timer2.reset();
            ropeManager.endRopeAttraction();
            playerAnimation.AnimationState = AnimationState.IDLE;
        }
    }


    private void phase3PrelagIfCollision()
    {
        if (!timer3.check())
        {
            timer2.reset();
        }
        else
        {
            timer3.reset();
            ropeManager.endRopeGrab();
            ropeManager.endRopeAttraction();
            playerAnimation.AnimationState = AnimationState.IDLE;
        }
    }

    override public void cancel()
    {
        ropeManager.endRopeGrab();
        ropeManager.endRopeAttraction();
        //playerController.velocity = new Vector2(0, 0);
        timer1.reset();
        timer2.reset();
        timer3.reset();
        playerAnimation.AnimationState = AnimationState.IDLE;
    }


    private RopegrabType getTypeOfRopegrab()
    {
        Vector2 directionPlayers = (enemyPlayerController.position - playerController.position).normalized;
        float dotProduct = Vector2.Dot(directionPlayers, joystickDirection);
        if (dotProduct * dotProduct > 0.85f){
            return RopegrabType.NEUTRAL;
        }
        else
        {
            if (Vector3.Cross(directionPlayers, joystickDirection).z > 0)
            {
                return RopegrabType.UP;
            }
            else
            {
                return RopegrabType.DOWN;
            }
        }
    }

}
