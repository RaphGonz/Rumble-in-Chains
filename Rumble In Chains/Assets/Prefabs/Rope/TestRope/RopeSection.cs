using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class that will hold information about each rope section
public struct RopeSection
{
    public Vector3 position;
    public Vector3 velocity;

    //To write RopeSection.zero
    public static readonly RopeSection zero = new RopeSection(Vector3.zero);

    public RopeSection(Vector3 pos)
    {
        this.position = pos;

        this.velocity = Vector3.zero;
    }
}