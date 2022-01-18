using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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
    

    //private int _framesCounterForShieldStun = 0;


    private int _attackFrame = 0;
    public LayerMask enemyMask;
    public float Pourcentages { get => _pourcentages; set { _pourcentages = value; /*UIController.Instance.ChangePercentages(this.gameObject.name.Equals("PlayerLeft") ? 1 : 2, Pourcentages);*/ } }
    public float Weight { get; }

    public float Points { get => _points; set { _points = value; UIController.Instance.ChangePoints(this.gameObject.name.Equals("PlayerLeft") ? 1 : 2, Points); /*Debug.Log(Points);*/ } }
    public int FramesBeforeShieldActive { get => _framesBeforeShieldActive; set { _framesBeforeShieldActive = value; } }
    public int FramesCounterForShield { get => _framesCounterForShield; set { _framesCounterForShield = value; } }
    //public int FramesCounterForShieldStun { get => _framesCounterForShieldStun; set { _framesCounterForShieldStun = value; } }
    public int FramesShieldPostlag { get => _framesShieldPostlag; set { _framesShieldPostlag = value; } }
     //Sert a savoir si j'ai touché quelque chose ou non : a rattacher a chaque hitbox

    public bool invincible = false; //Nombre de frame invicibilité de base, en plus après avoir été stunned
                                     //A equilibrer et faire varier avec les percentages
    [SerializeField]
    float maxFramesInvincibility = 10;
    float framesInvicibility = 0;

    public bool recovering = false;


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
    { //17 c'est la layer Player1 : a voir comment faire ça proprepement sans int
        lastCircleCenter = new List<Vector2>();//DEBUG 
        lastCircleRadius = new List<float>();//DEBUG 
        Character character = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + (this.gameObject.layer == 17 ? GameManager.Instance.characterPlayer1 : GameManager.Instance.characterPlayer2)+ ".asset");
        print((gameObject.layer == 17 ? "player1" : "player2") + " : " + character.name);
        Jab = character.attacks[0];
        SideTilt = character.attacks[1];
        UpTilt = character.attacks[2];
        DownTilt = character.attacks[3];
        DownAir = character.attacks[4];
        enemyMask = gameObject.layer == 17 ? LayerMask.GetMask("PlayerRight") : LayerMask.GetMask("PlayerLeft");
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
        Debug.Log("ouch!");
        if (!opponentActionController.isInvincible())
        {
            Pourcentages += pourcentage;
            
        }        
    }

    void CheckHitboxes(Attack attack)
    {
        //Debug.Log("current attack is not null");
        _attackFrame++;
        if (_attackFrame >= attack.Prelag) // no need to check if under attack.Prelag + attack.AttackDuration + attack.Postlag bcs currentAttack becomes null at the moment when the frame counter is greater than this amount
        {
            int thisHitbox = 0;
            foreach (var hitbox in attack.Hitboxes)
            {
                bool hit = false;
                thisHitbox++;
                //Si notre timer _attackframe est après le prelage et avant la fin des apparitions d'hitbox alors
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
                        //possibilité d'utiliser un booléen pour savoir si une hitbox peut expel. IE : une hitbox peut soit juste faire des dégâts, soit faire des dégâts + expel
                    }
                    else
                    {
                        //Debug.Break();
                        HitboxSphere hitboxSphere = (HitboxSphere)hitbox;
                        Collider2D collider = Physics2D.OverlapCircle(new Vector2(myPlayerController.facing * hitboxSphere.Center.x,hitboxSphere.Center.y) +  new Vector2(transform.position.x, transform.position.y), hitboxSphere.Radius, enemyMask) ;
                        print(collider);
                        //FOR DEBUGGING PURPOSES
                        lastCircleRadius.Add(hitboxSphere.Radius);
                        lastCircleCenter.Add(new Vector2(myPlayerController.facing * hitboxSphere.Center.x, hitboxSphere.Center.y) + new Vector2(transform.position.x, transform.position.y));
                        //Si j'ai touché quelque chose et que je n'avais rien touché avant
                        if (collider != null && !hit && lastHitbox != thisHitbox)
                        {
                            lastHitbox = thisHitbox;
                            hit = true;
                            collider.gameObject.GetComponent<CharacterController>().TakePourcentages(hitboxSphere.Damage); // changer le get component : l'adversaire est unique on peut donc le faire au start
                            Debug.Log(new Vector2(hitbox.Expulsion.x * myPlayerController.facing, hitbox.Expulsion.y));

                            if (!opponentActionController.isInvincible() && !opponentActionController.isShieldActive())
                            {
                                Debug.Log(hitbox.Expulsion);
                                float multiplier = (1 + opponentController.gameObject.GetComponent<CharacterController>().Pourcentages / 100);
                                opponentActionController.ExpelAndStun(new Vector2(hitbox.Expulsion.x*myPlayerController.facing, hitbox.Expulsion.y) * multiplier, (int)(hitbox.StunFactor * multiplier));
                            }
                            
                            // en vrai, vu qu'il y a un seul character controller adverse, il suffit de get en début de partie le character controller de l'adversaire
                        }

                        
                    }
                }
                else //On a rien touché : peut etre a l'avenir hit devrait être unique pour chaque hitbox
                {
                    hit = false;
                }
            }
        }
        if(_attackFrame >= attack.Prelag + attack.AttackDuration + attack.Postlag )
        {
            CurrentAttack = null;
            //Debug.Log("current attack is now null");
            _attackFrame = 0;
            lastHitbox = 0;
            //myInputController.attacking = false;

            
            
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

    public void Attack(AttackType type)
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
    }

    public void InterruptAttack()
    {
        CurrentAttack = null;
    }

    
}
