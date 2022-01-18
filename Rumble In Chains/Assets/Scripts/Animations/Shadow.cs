using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private float xPos;
    private float yPos;
    private float initialAlpha;
    [SerializeField] private float distanceToInvisible;
    [SerializeField] private float initialDiffDistance;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        initialAlpha = this.GetComponent<SpriteRenderer>().color.a;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        xPos = transform.parent.transform.position.x;
        

        transform.position = new Vector2(xPos, yPos);

        float diffDistance = Mathf.Abs(yPos - transform.parent.transform.position.y);
        Color c = this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, ComputeAlpha(diffDistance));
    }

    float ComputeAlpha(float diff)
    {
        float alpha = initialAlpha;
        if (diff > distanceToInvisible)
        {
            alpha = 0;
        }
        else if (diff > initialDiffDistance)
        {
            alpha = (distanceToInvisible - diff) / (distanceToInvisible - initialDiffDistance) * initialAlpha;
        }
        return alpha;
    }


}
