using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitboxSphere : Hitbox //!!!
{
    string jsonName;
    private Vector2 _center;
    private float _radius;
    public Vector2 Center { get => _center; }
    public float Radius { get => _radius; }

    public HitboxSphere(float damage, float stunFactor, float startUpTiming, float durationOfHitbox, Vector2 expulsion, Vector2 center, float radius) : base(damage, stunFactor, startUpTiming, durationOfHitbox, expulsion)
    {
        _center = center;
        _radius = radius;
    }
    public HitboxSphere(HitboxSphere hitboxSphere) : base(hitboxSphere.Damage, hitboxSphere.StunFactor, hitboxSphere.StartUpTiming, hitboxSphere.DurationOfHitbox, hitboxSphere.Expulsion)
    {
        _center = hitboxSphere.Center;
        _radius = hitboxSphere.Radius;
    }
    public HitboxSphere CreateFromJson()
    {
        return JsonUtility.FromJson<HitboxSphere>(jsonName);
    }
}
