using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicAction : Action
{
    [SerializeField] private float preLag;
    [SerializeField] private float postLag;
    [SerializeField] private float cooldownBetweenTwoPoses;

    [SerializeField] CharacterController characterController;

    public ParticleSystem backGroundConfetti;
    public Image flash;
    private float flashAlpha;
    private Vector4 flashColor;

    private void Start()
    {
        timer1.setDuration(preLag);
        timer2.setDuration(postLag);
        cooldown.setDuration(cooldownBetweenTwoPoses);

        flashAlpha = 1;
        flashColor = new Vector4(1, 1, 1, flashAlpha);
    }


    // Start is called before the first frame update
    override public void start()
    {
        timer1.start();
        cooldown.start();
    }

    // Update is called once per frame
    override public bool update()
    {
        if (timer1.isActive())
        {
            if (timer1.check())
            {
                timer1.reset();
                timer2.start();

                StartCoroutine("Flash");

                gainPoint();
            }
            return false;
        }
        if (timer2.isActive())
        {
            if (timer2.check())
            {
                backGroundConfetti.Play();

                timer2.reset();
            }


            return false;
        }

        return true;
    }

    IEnumerator Flash()
    {
        flash.color = flashColor; //Le flash est lancé : alpha set au maximum

        while(flashColor.w > 0)
        {
            flashColor.w = (1 - timer2.getRatio());
            flash.color = flashColor;
            yield return new WaitForEndOfFrame();
        }

    }

    private void gainPoint()
    {
        characterController.Points++;
        print(characterController.Points);
    }



    override public void cancel()
    {
        timer1.reset();
        timer2.reset();
        timer3.reset();
    }
}
