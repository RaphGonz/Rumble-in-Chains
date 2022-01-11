using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hitbox", menuName = "Hitbox")]
public class HitboxSphere : Hitbox //!!!
{
    [SerializeField]
    private Vector2 _center;
    [SerializeField]
    private float _radius;
    public Vector2 Center { get => _center; }
    public float Radius { get => _radius; }

    public HitboxSphere(float damage, float stunFactor, int startUpTiming, int durationOfHitbox, Vector2 expulsion, Vector2 center, float radius) : base(damage, stunFactor, startUpTiming, durationOfHitbox, expulsion)
    {
        _center = center;
        _radius = radius;
    }
    public HitboxSphere(HitboxSphere hitboxSphere) : base(hitboxSphere.Damage, hitboxSphere.StunFactor, hitboxSphere.StartUpTiming, hitboxSphere.DurationOfHitbox, hitboxSphere.Expulsion)
    {
        _center = hitboxSphere.Center;
        _radius = hitboxSphere.Radius;
    }
}
