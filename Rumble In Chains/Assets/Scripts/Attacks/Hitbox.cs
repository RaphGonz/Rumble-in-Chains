using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitbox : ScriptableObject //!!!
{
    [SerializeField]
    protected float _damage;
    [SerializeField]
    protected float _stunFactor = 1; //C'est le multiplicateur de stun, en général > 1.f
    [SerializeField]
    protected int _startUpTiming;// number of frames at which this hitbox starts. 1st hitbox starts at startUpTiming = 0;
    [SerializeField]
    protected int _durationOfHitbox;
    [SerializeField]
    protected Vector2 _expulsion;
    [SerializeField]
    protected int _particleSystemId;
    [SerializeField]
    protected bool _firstLoop = true;
    public float Damage { get => _damage; }
    public float StunFactor { get => _stunFactor; }
    public int StartUpTiming { get => _startUpTiming; }// number of frames at which this hitbox starts. 1st hitbox starts at startUpTiming = 0;
    public int DurationOfHitbox { get => _durationOfHitbox; } //number of frames that this hitbox lasts;
    public Vector2 Expulsion { get => _expulsion; }
    public int ParticleSystemName { get => _particleSystemId; }
    public bool FirstLoop { get => _firstLoop; set { _firstLoop = value; } }


    protected Hitbox(float damage,float stunFactor, int startUpTiming, int durationOfHitbox, Vector2 expulsion)
    {
        _damage = damage;
        _stunFactor = stunFactor;
        _startUpTiming = startUpTiming;
        _durationOfHitbox = durationOfHitbox;
        _expulsion = expulsion;
        _firstLoop = true;
    }
    protected Hitbox(Hitbox hitbox)
    {
        _damage = hitbox.Damage;
        _stunFactor = hitbox.StunFactor;
        _startUpTiming = hitbox.StartUpTiming;
        _durationOfHitbox = hitbox.DurationOfHitbox;
        _expulsion = hitbox.Expulsion;
        _firstLoop = true;
    }
}
