using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxCapsule : Hitbox // !!!
{
    Vector2 _centerOfTheFirstSphere;
    Vector2 _centerOfTheSecondSphere;
    float _radius;
    public Vector2 CenterOfTheFirstSphere { get => _centerOfTheFirstSphere; }
    public Vector2 CenterOfTheSecondSphere { get => _centerOfTheSecondSphere; }
    public float Radius { get => _radius; }

    public HitboxCapsule(float damage, float stunFactor, float startUpTiming, float durationOfHitbox, Vector2 expulsion, Vector2 centerOfTheFirstSphere, Vector2 centerOfTheSecondSphere, float radius) : base(damage, stunFactor, startUpTiming, durationOfHitbox, expulsion)
    {
        _centerOfTheFirstSphere = centerOfTheFirstSphere;
        _centerOfTheSecondSphere = centerOfTheSecondSphere;
        _radius = radius;
    }
}
