using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XInputDotNetPure;


public class CharacterController : MonoBehaviour //!!!
{
    private float _pourcentages = 0;
    private float _points = 0;
    private int lastHitbox;
    [SerializeField]
    private int _framesBeforeShieldActive;
    private int _framesCounterForShield = 0;
    [SerializeField]
    private int _framesShieldPostlag;
    private bool shieldStun = false;

    private Weight _weight;
    private DashActivation _dashActivation;
    bool _soundIsPlayed = false;
    int _ouch;

    GamePadState prevState1;
    GamePadState prevState2;
    GamePadState state1;
    GamePadState state2;

    private bool attackAlreadyHit = false; //permet d'appliquer les dommages seulement avec une seule des hitbox.

    

    //private int _framesCounterForShieldStun = 0;


    private int _attackFrame = 0;
    public LayerMask enemyMask;
    public float Pourcentages { get => _pourcentages; set { _pourcentages = value; UIController.Instance.ChangePercentages(this.gameObject.name.Equals("PlayerLeft") ? 1 : 2, Pourcentages); } }
    public float Weight { get; }

    public float Points { get => _points; set { _points = value; UIController.Instance.ChangePoints(this.gameObject.name.Equals("PlayerLeft") ? 1 : 2, Points); StartCoroutine(Vibrate());} }
    public int FramesBeforeShieldActive { get => _framesBeforeShieldActive; set { _framesBeforeShieldActive = value; } }
    public int FramesCounterForShield { get => _framesCounterForShield; set { _framesCounterForShield = value; } }
    //public int FramesCounterForShieldStun { get => _framesCounterForShieldStun; set { _framesCounterForShieldStun = value; } }
    public int FramesShieldPostlag { get => _framesShieldPostlag; set { _framesShieldPostlag = value; } }
     //Sert a savoir si j'ai touch? quelque chose ou non : a rattacher a chaque hitbox

    public bool invincible = false; //Nombre de frame invicibilit? de base, en plus apr?s avoir ?t? stunned
                                     //A equilibrer et faire varier avec les percentages
    [SerializeField]
    float maxFramesInvincibility = 10;
    float framesInvicibility = 0;

    public bool recovering = false;

    public bool SoundIsPlayed { get => _soundIsPlayed; private set { _soundIsPlayed = value; } }
    public int Ouch { get => _ouch; private set { _ouch = value; } }

    [SerializeField] int playerNumber = 1;
    int pointStreak = 0;

    #region Attacks
    public int AttackFrame { get; set; }
    public Attack CurrentAttack { get; set; } = null;
    //public Attack NeutralAir { get; set; }
    //public Attack UpAir { get; set; }
    //public Attack SideAir { get; set; }
    public Attack DownAir { get; set; }
    public Attack Jab { get; set; }
    public Attack UpTilt { get; set; }
    public Attack SideTilt { get; set; }
    public Attack DownTilt { get; set; }
    #endregion

    #region debug
    private List<float> lastCircleRadius;
    private List<Vector2> lastCircleCenter;
    #endregion
    
    [SerializeField]
    PlayerController myPlayerController;
    [SerializeField]
    PlayerController opponentController;
    [SerializeField]
    ActionController opponentActionController;

    #region UI
    [SerializeField]
    UIStunSlider stunSlider;
    #endregion


    // Start is called before the first frame update
    void Start()
    { //17 c'est la layer Player1 : a voir comment faire ?a proprepement sans int
        lastCircleCenter = new List<Vector2>();//DEBUG 
        lastCircleRadius = new List<float>();//DEBUG 
        
        Character character = this.gameObject.layer == 17 ? GameManager.Instance.Character1: GameManager.Instance.Character2;
        //print(character.name);
        Jab = character.attacks[0];
        SideTilt = character.attacks[1];
        UpTilt = character.attacks[2];
        DownTilt = character.attacks[3];
        DownAir = character.attacks[4];
        enemyMask = gameObject.layer == 17 ? LayerMask.GetMask("PlayerRight") : LayerMask.GetMask("PlayerLeft");
        Ouch = character.ouch;

        EventManager.Instance.eventPlayerInZone += OnEventInZone;
    }

    // Update is called once per frame
    public void UpdateCharacter()
    {
        if (CurrentAttack != null)
        {
            CheckHitboxes(CurrentAttack);
        }
        if (recovering)
        {
            framesInvicibility++;
            if(framesInvicibility > maxFramesInvincibility)
            {
                recovering = false;
                invincible = false;
                framesInvicibility = 0;
            }
        }
        
    }


    public void TakePourcentages(float pourcentage)
    {
        if (!opponentActionController.isInvincible())
        {
            Pourcentages += pourcentage;
        }
        if(Random.Range(0,10) < 1)
        {
            SoundPlayer.Instance.PlaySound(Ouch);
        }
    }

    void CheckHitboxes(Attack attack)
    {
        //Debug.Log("current attack is not null");
        _attackFrame++;

        

        if (_attackFrame >= attack.Prelag) // no need to check if under attack.Prelag + attack.AttackDuration + attack.Postlag bcs currentAttack becomes null at the moment when the frame counter is greater than this amount
        {
            if (!SoundIsPlayed)
            {
                SoundPlayer.Instance.PlaySound(attack.AudioClip);
                SoundIsPlayed = true;
            }
            int thisHitbox = 0;
            foreach (var hitbox in attack.Hitboxes)
            {
                bool hit = false;
                thisHitbox++;
                //Si notre timer _attackframe est apr?s le prelage et avant la fin des apparitions d'hitbox alors
                if (_attackFrame >= attack.Prelag + hitbox.StartUpTiming && _attackFrame < attack.Prelag + hitbox.StartUpTiming + hitbox.DurationOfHitbox)
                {
                    //Debug.Log("attacking!");

                    if (hitbox is HitboxCapsule)
                    {
                        HitboxCapsule hitboxCapsule = (HitboxCapsule)hitbox;
                        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(hitboxCapsule.CenterOfTheFirstSphere + new Vector2(transform.position.x, transform.position.y), new Vector2(hitboxCapsule.CenterOfTheSecondSphere.x - hitboxCapsule.CenterOfTheFirstSphere.x, hitboxCapsule.CenterOfTheSecondSphere.y - hitboxCapsule.CenterOfTheFirstSphere.y), CapsuleDirection2D.Vertical, -30, enemyMask);
                        foreach(var collider in colliders)
                        {
                            collider.gameObject.SetActive(false) ;
                        }

                        /*if (collider != null && !hit)
                        {
                            hit = true;
                            collider.gameObject.GetComponent<CharacterController>().TakeDamages(hitboxCapsule.Damage);
                            collider.gameObject.GetComponent<CharacterController>().Expel(hitboxCapsule.Expulsion);
                        }*/
                        //Center of the first sphere, center of the second sphere, radius
                        // Vector2 Center, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, vector2 direction
                        //center = center ;; secondsphere - firstsphere = size
                        //Move the character (expulsion) 
                        //possibilit? d'utiliser un bool?en pour savoir si une hitbox peut expel. IE : une hitbox peut soit juste faire des d?g?ts, soit faire des d?g?ts + expel
                    }
                    else
                    {
                        HitboxSphere hitboxSphere = (HitboxSphere)hitbox;
                        Collider2D collider = Physics2D.OverlapCircle(new Vector2(myPlayerController.facing * hitboxSphere.Center.x,hitboxSphere.Center.y) +  new Vector2(transform.position.x, transform.position.y), hitboxSphere.Radius, enemyMask) ;
                        //FOR DEBUGGING PURPOSES
                        if (hitboxSphere.FirstLoop)
                        {
                            //print("firstLoop azerty");
                            EventManager.Instance.OnEventSpawnParticles(hitboxSphere.ParticleSystemName, transform.position + new Vector3(myPlayerController.facing * hitboxSphere.Center.x, hitboxSphere.Center.y), myPlayerController.facing >= 0);
                            hitboxSphere.FirstLoop = false;
                        }

                        //print("still In loop");
                        lastCircleRadius.Add(hitboxSphere.Radius);
                        lastCircleCenter.Add(new Vector2(myPlayerController.facing * hitboxSphere.Center.x, hitboxSphere.Center.y) + new Vector2(transform.position.x, transform.position.y));
                        //Si j'ai touch? quelque chose et que je n'avais rien touch? avant
                        if (collider != null && !hit && lastHitbox != thisHitbox && !attackAlreadyHit)
                        {
                            if (!opponentActionController.isInvincible() && !opponentActionController.isShieldActive())
                            {
                                lastHitbox = thisHitbox;
                                hit = true;
                                attackAlreadyHit = true;
                                collider.gameObject.GetComponent<CharacterController>().TakePourcentages(hitboxSphere.Damage); // changer le get component : l'adversaire est unique on peut donc le faire au start
                                float multiplier = (1 + opponentController.gameObject.GetComponent<CharacterController>().Pourcentages / 100);
                                opponentActionController.ExpelAndStun(new Vector2(hitbox.Expulsion.x*myPlayerController.facing, hitbox.Expulsion.y) * multiplier * 2, (int)(hitbox.StunFactor * multiplier * 20));
                            }
                            
                            // en vrai, vu qu'il y a un seul character controller adverse, il suffit de get en d?but de partie le character controller de l'adversaire
                        }

                        
                    }
                }
                else //On a rien touch? : peut etre a l'avenir hit devrait ?tre unique pour chaque hitbox
                {
                    hit = false;
                }
            }
        }
        if(_attackFrame >= attack.Prelag + attack.AttackDuration + attack.Postlag )
        {
            CurrentAttack = null;
            _attackFrame = 0;
            lastHitbox = 0;
            attackAlreadyHit = false;
            foreach (var hitbox in attack.Hitboxes)
            {
                hitbox.FirstLoop = true;
            }
            //myInputController.attacking = false
            SoundIsPlayed = false;
        }
        
    }

    void OnDrawGizmos()
    {
        for(int i = 0; i< lastCircleCenter.Count; i++)
        {
            Gizmos.DrawSphere(lastCircleCenter[i], lastCircleRadius[i]);
        }
    }

    //public void Shield()
    //{
    //    if (FramesCounterForShield > FramesBeforeShieldActive)
    //    {
    //        invincible = true;
    //    }
    //    shieldStun = true;
    //    FramesCounterForShield++;
        
    //}

    //public void ReleaseShield()
    //{
    //    invincible = false;
    //    FramesCounterForShield = 0;
    //    StopCoroutine(ShieldStun());
    //    StartCoroutine(ShieldStun());
    //}
    //private IEnumerator ShieldStun()
    //{
    //    for(int i = 0; i<FramesShieldPostlag; i++)
    //    {
    //        yield return null;
    //    }
    //    shieldStun = false;
        
    //}

    public Vector2 Attack(AttackType type)
    {
        lastCircleCenter.Clear();//DEBUG
        lastCircleRadius.Clear();//Debug
        switch (type)
        {
            case AttackType.Jab:
                CurrentAttack = Jab;
                break;
            case AttackType.UpTilt:
                CurrentAttack = UpTilt;
                break;
            case AttackType.SideTilt:
                CurrentAttack = SideTilt;
                break;
            case AttackType.DownTilt:
                CurrentAttack = DownTilt;
                break;
            //case AttackType.NeutralAir:
            //    CurrentAttack = NeutralAir;
            //    break;
            //case AttackType.UpAir:
            //    CurrentAttack = UpAir;
            //    break;
            //case AttackType.SideAir:
            //    CurrentAttack = SideAir;
            //    break;
            case AttackType.DownAir:
                CurrentAttack = DownAir;
                break;
            //case AttackType.DashAttackLight:
            //    break;
            //case AttackType.DashAttackStrong:
            //    break;

        }

        int value = 0;
        for (int i = 0; i < CurrentAttack.Hitboxes.Count; i++)
        {
            int newValue = CurrentAttack.Hitboxes[i].StartUpTiming + CurrentAttack.Hitboxes[i].DurationOfHitbox;
            if (newValue > value)
            {
                value = newValue;
            }
        }

        if (CurrentAttack != null)
        {
            foreach (var hitbox in CurrentAttack.Hitboxes)
            {
                hitbox.FirstLoop = true;
            }
        }

        return new Vector2(CurrentAttack.Prelag + value, CurrentAttack.Postlag);
    }

    public void InterruptAttack()
    {
        CurrentAttack = null;
    }

    IEnumerator Vibrate()
    {
        GamePad.SetVibration(PlayerIndex.One, 1, 1);
        GamePad.SetVibration(PlayerIndex.Two, 1, 1);
        yield return new WaitForSeconds(.3f);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
        GamePad.SetVibration(PlayerIndex.Two, 0, 0);

    }

    private void OnEventInZone(int i, bool b)
    {
        if (playerNumber == i)
        {
            if (!b)
            {
                pointStreak = 0;
            }
        }
    }

    public void GainPoint()
    {
        pointStreak++;
        Points += pointStreak;
    }

    
}
