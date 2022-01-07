using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Hitbox //!!!
{
    protected float _damage;
    protected float _stunFactor; //C'est le multiplicateur de stun, en général > 1.f
    protected float _startUpTiming;// number of frames at which this hitbox starts. 1st hitbox starts at startUpTiming = 0;
    protected float _durationOfHitbox;
    protected Vector2 _expulsion;
    public float Damage { get => _damage; }
    public float StunFactor { get => _stunFactor; }
    public float StartUpTiming { get => _startUpTiming; }// number of frames at which this hitbox starts. 1st hitbox starts at startUpTiming = 0;
    public float DurationOfHitbox { get => _durationOfHitbox; } //number of frames that this hitbox lasts;
    public Vector2 Expulsion { get => _expulsion; }

    protected Hitbox(float damage,float stunFactor, float startUpTiming, float durationOfHitbox, Vector2 expulsion)
    {
        _damage = damage;
        _stunFactor = stunFactor;
        _startUpTiming = startUpTiming;
        _durationOfHitbox = durationOfHitbox;
        _expulsion = expulsion;
    }
    protected Hitbox(Hitbox hitbox)
    {
        _damage = hitbox.Damage;
        _stunFactor = hitbox.StunFactor;
        _startUpTiming = hitbox.StartUpTiming;
        _durationOfHitbox = hitbox.DurationOfHitbox;
        _expulsion = hitbox.Expulsion;
    }
}
