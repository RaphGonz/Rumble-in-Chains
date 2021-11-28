using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeWithLineRenderer : MonoBehaviour
{
    public float springRigidity;


    public float mass;




    public Vector2 gravityForce = new Vector2(0, -1);





    public int maximumStretchIterations = 2;

    public float lengthOfSection;
    public float numberOfSections;
    public float ropeWidth = 0.1f;

    public float springDamping;
    public float springAirDamping;

    public GameObject leftCharacter;
    public GameObject rightCharacter;
    public List<RopeSection> listRopeSection;
    public LineRenderer lineRenderer;

    public List<Vector3> listForce;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        listRopeSection = new List<RopeSection>();
        listForce = new List<Vector3>();

        Vector2 pos = leftCharacter.transform.position;



        for (int i = 0; i < numberOfSections; i++)
        {
            listRopeSection.Add(new RopeSection(pos));
            listForce.Add(Vector3.zero);

            pos.x += lengthOfSection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowRope();

        //computeLeftCharacter();

    }

    void FixedUpdate()
    {
        if (listRopeSection.Count > 0)
        {
            int iterations = 1;
            float dt = Time.fixedDeltaTime / (float)iterations;

            for (int i = 0; i < iterations; i++)
            {
                UpdateRopePosition(dt);
            }
        }
    }

    public void computeLeftCharacter()
    {
        leftCharacter.transform.position = listRopeSection[0].position;
        leftCharacter.transform.LookAt(listRopeSection[1].position);
    }

    public void ShowRope()
    {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;



        Vector3[] positions = new Vector3[listRopeSection.Count];

        for (int i = 0; i < listRopeSection.Count; i++)
        {
            positions[i] = listRopeSection[i].position;
        }

        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }


    public void UpdateRopePosition(float dt)
    {
        RopeSection lastRopeSection = listRopeSection[listRopeSection.Count - 1];

        lastRopeSection.position = rightCharacter.transform.position;

        listRopeSection[listRopeSection.Count - 1] = lastRopeSection;


        RopeSection firstRopeSection = listRopeSection[0];

        firstRopeSection.position = leftCharacter.transform.position;

        listRopeSection[0] = firstRopeSection;




        List<Vector3> accelerationFromForces = ComputeAccelerations(listRopeSection);
        List<RopeSection> nextPosVelForwardEuler = new List<RopeSection>();
        nextPosVelForwardEuler.Add(listRopeSection[0]);

        //Euler computation
        for (int i = 1; i < listRopeSection.Count - 1; i++)
        {
            RopeSection thisRopeSection = RopeSection.zero;

            //vel = vel + acc * t
            thisRopeSection.velocity = listRopeSection[i].velocity + accelerationFromForces[i] * dt;

            //pos = pos + vel * t
            thisRopeSection.position = listRopeSection[i].position + listRopeSection[i].velocity * dt;

            //Save the new data in a temporarily list
            nextPosVelForwardEuler.Add(thisRopeSection);
        }

        //Add the last which is always the same because it's attached to something
        nextPosVelForwardEuler.Add(listRopeSection[listRopeSection.Count - 1]);



        List<Vector3> accelerationFromEuler = ComputeAccelerations(nextPosVelForwardEuler);

        List<RopeSection> nextPosVelHeunsMethod = new List<RopeSection>();
        nextPosVelHeunsMethod.Add(listRopeSection[0]);

        //Loop through all line segments (except the last because it's always connected to something)
        for (int i = 1; i < listRopeSection.Count - 1; i++)
        {
            RopeSection thisRopeSection = RopeSection.zero;

            //Heuns method
            //vel = vel + (acc + accFromForwardEuler) * 0.5 * t
            thisRopeSection.velocity = listRopeSection[i].velocity + (accelerationFromForces[i] + accelerationFromEuler[i]) * 0.5f * dt;

            //pos = pos + (vel + velFromForwardEuler) * 0.5f * t
            thisRopeSection.position = listRopeSection[i].position + (listRopeSection[i].velocity + nextPosVelForwardEuler[i].velocity) * 0.5f * dt;

            //Save the new data in a temporarily list
            nextPosVelHeunsMethod.Add(thisRopeSection);
        }

        nextPosVelHeunsMethod.Add(listRopeSection[listRopeSection.Count - 1]);



        //From the temp list to the main list
        for (int i = 1; i < listRopeSection.Count; i++)
        {
            listRopeSection[i] = nextPosVelHeunsMethod[i];

            //allRopeSections[i] = nextPosVelForwardEuler[i];
        }


        //Implement maximum stretch to avoid numerical instabilities
        //May need to run the algorithm several times

        /*
        for (int i = 0; i < maximumStretchIterations; i++)
        {
            ImplementMaximumStretch();
        }
        */

    }



    public List<Vector3> ComputeAccelerations(List<RopeSection> ropeSections)
    {
        List<Vector3> accelerations = new List<Vector3>();


        for (int i = 0; i < ropeSections.Count - 1; i++)
        {

            Vector3 vectorBetweenTwoSections = ropeSections[i + 1].position - ropeSections[i].position;

            float distance = vectorBetweenTwoSections.magnitude;
            Vector3 direction = vectorBetweenTwoSections.normalized;



            float springForce = springRigidity * (distance - lengthOfSection);

            float frictionForce = springDamping * ((Vector3.Dot(ropeSections[i + 1].velocity - ropeSections[i].velocity, vectorBetweenTwoSections)) / distance);



            Vector3 totalForce = (springForce + frictionForce) * direction;

            listForce[i] = totalForce;
        }


        for (int i = 0; i < ropeSections.Count - 1; i++)
        {
            Vector3 finalSpringForce = Vector3.zero;

            finalSpringForce += listForce[i];

            if (i != 0)
            {
                finalSpringForce -= listForce[i - 1];
            }



            float vel = ropeSections[i].velocity.magnitude;

            Vector3 dampingForce = springAirDamping * vel * vel * ropeSections[i].velocity.normalized;



            float springMass = mass;

            if (i == 0)
            {
                springMass += leftCharacter.GetComponent<Rigidbody2D>().mass;
            }
            else if (i == ropeSections.Count - 1)
            {
                springMass += rightCharacter.GetComponent<Rigidbody2D>().mass;
            }



            Vector3 gravityForce = springMass * new Vector3(0f, -9.81f, 0f);

            Vector3 totalForce = finalSpringForce + gravityForce - dampingForce;

            Vector3 acceleration = totalForce / springMass;

            accelerations.Add(acceleration);

        }

        accelerations.Add(Vector3.zero);

        return accelerations;
    }



    private void ImplementMaximumStretch()
    {
        //Make sure each spring are not less compressed than 90% nor more stretched than 110%
        float maxStretch = 1.1f;
        float minStretch = 0.9f;

        //Loop from the end because it's better to adjust the top section of the rope before the bottom
        //And the top of the rope is at the end of the list
        for (int i = listRopeSection.Count - 1; i > 0; i--)
        {
            RopeSection topSection = listRopeSection[i];

            RopeSection bottomSection = listRopeSection[i - 1];

            //The distance between the sections
            float dist = (topSection.position - bottomSection.position).magnitude;

            //What's the stretch/compression
            float stretch = dist / lengthOfSection;

            if (stretch > maxStretch)
            {
                //How far do we need to compress the spring?
                float compressLength = dist - (lengthOfSection * maxStretch);

                //In what direction should we compress the spring?
                Vector3 compressDir = (topSection.position - bottomSection.position).normalized;

                Vector3 change = compressDir * compressLength;

                MoveSection(change, i - 1);
            }
            else if (stretch < minStretch)
            {
                //How far do we need to stretch the spring?
                float stretchLength = (lengthOfSection * minStretch) - dist;

                //In what direction should we compress the spring?
                Vector3 stretchDir = (bottomSection.position - topSection.position).normalized;

                Vector3 change = stretchDir * stretchLength;

                MoveSection(change, i - 1);
            }
        }
    }

    //Move a rope section based on stretch/compression
    private void MoveSection(Vector3 finalChange, int listPos)
    {
        RopeSection bottomSection = listRopeSection[listPos];

        bottomSection.position += finalChange;

        listRopeSection[listPos] = bottomSection;
    }

}
