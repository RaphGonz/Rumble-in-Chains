using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    [SerializeField]
    private float _prelag;
    [SerializeField]
    private float _postlag;
    [SerializeField]
    private List<Hitbox> _hitboxes;
    [SerializeField]
    private float _attackDuration = 0;
    [SerializeField]
    private int _audioClip;
    public float Prelag { get => _prelag; }
    public float Postlag { get => _postlag; }
    public List<Hitbox> Hitboxes { get => _hitboxes; }
    public float AttackDuration { get => _attackDuration; }
    public int AudioClip { get => _audioClip; }

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
