using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitboxCapsule : Hitbox // !!!
{
    [SerializeField]
    static string jsonName;
    Vector2 _centerOfTheFirstSphere;
    Vector2 _centerOfTheSecondSphere;
    float _radius;
    public Vector2 CenterOfTheFirstSphere { get => _centerOfTheFirstSphere; }
    public Vector2 CenterOfTheSecondSphere { get => _centerOfTheSecondSphere; }
    public float Radius { get => _radius; }

    public HitboxCapsule(float damage, float stunFactor, int startUpTiming, int durationOfHitbox, Vector2 expulsion, Vector2 centerOfTheFirstSphere, Vector2 centerOfTheSecondSphere, float radius) : base(damage, stunFactor, startUpTiming, durationOfHitbox, expulsion)
    {
        _centerOfTheFirstSphere = centerOfTheFirstSphere;
        _centerOfTheSecondSphere = centerOfTheSecondSphere;
        _radius = radius;
    }
    public HitboxCapsule(HitboxCapsule hitboxCapsule) : base(hitboxCapsule.Damage, hitboxCapsule.StunFactor, hitboxCapsule.StartUpTiming, hitboxCapsule.DurationOfHitbox, hitboxCapsule.Expulsion)
    {
        _centerOfTheFirstSphere = hitboxCapsule.CenterOfTheFirstSphere;
        _centerOfTheFirstSphere = hitboxCapsule.CenterOfTheSecondSphere;
        _radius = hitboxCapsule.Radius;
    }
    public static HitboxCapsule CreateFromJson()
    {
        return JsonUtility.FromJson<HitboxCapsule>(jsonName);
    }
}
