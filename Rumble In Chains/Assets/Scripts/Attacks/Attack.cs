using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack // !!!
{
    private float _prelag;
    private float _postlag;
    private List<Hitbox> _hitboxes;
    private float _attackDuration = 0;
    public float Prelag { get => _prelag; }
    public float Postlag { get => _postlag; }
    public List<Hitbox> Hitboxes { get => _hitboxes; }
    public float AttackDuration { get => _attackDuration; }

    public Attack(float prelag, float postlag, List<Hitbox> hitboxes)
    {
        this._prelag = prelag;
        this._postlag = postlag;
        this._hitboxes = hitboxes;
        foreach(Hitbox hitbox in hitboxes)
        {
            _attackDuration = Mathf.Max(hitbox.DurationOfHitbox + hitbox.StartUpTiming, _attackDuration);
        }
    }

    public Attack()
    {
        this._prelag = 0;
        this._postlag = 0;
        this._hitboxes = new List<Hitbox>();
    }

    public override string ToString() => $"(prelag : {_prelag}, postlag : {_postlag}, hitboxes : {_hitboxes})";

    
}
