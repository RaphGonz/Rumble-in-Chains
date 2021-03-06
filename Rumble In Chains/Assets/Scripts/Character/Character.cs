using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Speed speed;
    public Weight weight;
    public JumpHeight jumpHeight;
    public DashActivation dashActivation;
    public DashDistance dashDistance;
    public RopePulling ropePulling;
    public Sprite spriteNormal;
    public Sprite spriteAttacked;
    public List<Attack> attacks;
    public readonly CharacterConverter characterConverter = new CharacterConverter();
    public int ouch;
}
