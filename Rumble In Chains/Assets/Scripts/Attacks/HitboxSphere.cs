using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSphere : Hitbox //!!!
{
    private Vector2 _center;
    private float _radius;
    public Vector2 Center { get => _center; }
    public float Radius { get => _radius; }

    public HitboxSphere(float damage, float stunFactor, float startUpTiming, float durationOfHitbox, Vector2 expulsion, Vector2 center, float radius) : base(damage, stunFactor, startUpTiming, durationOfHitbox, expulsion)
    {
        _center = center;
        _radius = radius;
    }
}
