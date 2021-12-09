using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour //!!!
{
    private float _pourcentages = 0;
    private float _weight;
    private int _points = 0;



    private int _attackFrame = 0;
    public LayerMask enemyMask;
    public float Pourcentages { get => _pourcentages; set { _pourcentages = value; UIController.Instance.ChangePercentages(this.gameObject.name.Equals("PlayerLeft") ? 1 : 2, Pourcentages); } }
    public float Weight { get; }

    public int Points { get => _points; set { _points = value; UIController.Instance.ChangePoints(this.gameObject.name.Equals("PlayerLeft") ? 1 : 2, Points); } }

    bool hit = false; //Sert a savoir si j'ai touché quelque chose ou non : a rattacher a chaque hitbox

    public bool invincible = false; //Nombre de frame invicibilité de base, en plus après avoir été stunned
                                     //A equilibrer et faire varier avec les percentages
    [SerializeField]
    float maxFramesInvincibility = 1000;
    float framesInvicibility = 0;

    public bool recovering = false;


    #region Attacks
    public int AttackFrame { get; set; }
    public Attack CurrentAttack { get; set; } = null;
    public Attack NeutralAir { get; set; }
    public Attack UpAir { get; set; }
    public Attack SideAir { get; set; }
    public Attack DownAir { get; set; }
    public Attack Jab { get; set; }
    public Attack UpTilt { get; set; }
    public Attack SideTilt { get; set; }
    public Attack DownTilt { get; set; }
    #endregion

    #region debug
    private float lastCircleRadius;
    private Vector2 lastCircleCenter;
    #endregion
    

    [SerializeField]
    InputManager myInputController;
    [SerializeField]
    PlayerController myPlayerController;
    [SerializeField]
    PlayerController opponentController;

    #region UI
    [SerializeField]
    UIStunSlider stunSlider;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        HitboxSphere hitbox1 = new HitboxSphere(5, 5, 0, 3, -1 * Vector2.one, new Vector2(1, 0), 5);
        
        //HitboxCapsule hitbox2 = new HitboxCapsule(5, 0, 3, Vector2.one, Vector2.zero, 1*Vector2.one, 1);
        List<Hitbox> hitboxList = new List<Hitbox>();
        hitboxList.Add(hitbox1);
        Jab = new Attack(4, 4, hitboxList);
        
        enemyMask = this.gameObject.layer == 17 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1"); //17 c'est la layer Player1 : a voir comment faire ça proprepement sans int

        

    }

    // Update is called once per frame
    void Update()
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

        if (!invincible)
        {
            Pourcentages += pourcentage;
            
        }
        print(invincible);
        
    }

    void CheckHitboxes(Attack attack)
    {
        //Debug.Log("current attack is not null");
        _attackFrame++;
        if (_attackFrame >= attack.Prelag) // no need to check if under attack.Prelag + attack.AttackDuration + attack.Postlag bcs currentAttack becomes null at the moment when the frame counter is greater than this amount
        {
            
            foreach (var hitbox in attack.Hitboxes)
            {
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
                        
                        HitboxSphere hitboxSphere = (HitboxSphere)hitbox;
                        Collider2D collider = Physics2D.OverlapCircle(myPlayerController.facing * hitboxSphere.Center +  new Vector2(transform.position.x, transform.position.y), hitboxSphere.Radius, enemyMask) ;
                        //FOR DEBUGGING PURPOSES
                        lastCircleRadius = hitboxSphere.Radius;
                        lastCircleCenter = myPlayerController.facing * hitboxSphere.Center + new Vector2(transform.position.x, transform.position.y);
                        //Si j'ai touché quelque chose et que je n'avais rien touché avant
                        if (collider != null && !hit)
                        {
                            
                            hit = true;
                            collider.gameObject.GetComponent<CharacterController>().TakePourcentages(hitboxSphere.Damage); // changer le get component : l'adversaire est unique on peut donc le faire au start
                            
                            Expel(hitbox.Expulsion);
                            
                            Stun(hitbox.StunFactor);
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
            //myInputController.attacking = false;
            
        }
        
    }
    
    void Expel(Vector2 expelForce)
    {
        Vector2 finalExpelForce = opponentController.gameObject.GetComponent<CharacterController>().Pourcentages * expelForce * myPlayerController.facing ;
        opponentController.velocity += finalExpelForce;
        //print(opponentController.gameObject.GetComponent<CharacterController>().Pourcentages);
        //print(expelForce);
        //print(finalExpelForce);
        //print(opponentController.velocity);
        //Debug.Log("expelled !");
    }

    void Stun(float stunFactor) //On lance un timer dans l'input manager qui change le comportement des boutons pendant un nombre de frame donné, ici stun * pourcentages
    {
        if (!invincible)
        {
            int stunValueInFrames = (int)(stunFactor * opponentController.gameObject.GetComponent<CharacterController>().Pourcentages);
            opponentController.gameObject.GetComponent<InputManager>().Stun(stunValueInFrames);
            opponentController.gameObject.GetComponent<CharacterController>().invincible = true;

        }
        
    }

    public void KeepInvincible()
    {
        if (framesInvicibility < maxFramesInvincibility)
        {
            framesInvicibility++;
        }
        invincible = false;
        //gameObject.GetComponent<InputManager>().coroutineStarted = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(lastCircleCenter, lastCircleRadius);
       
    }

    public void Attack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Jab:
                CurrentAttack = Jab;
                break;
            case AttackType.UpTilt:
                break;
            case AttackType.SideTilt:
                break;
            case AttackType.DownTilt:
                break;
            case AttackType.NeutralAir:
                break;
            case AttackType.UpAir:
                break;
            case AttackType.SideAir:
                break;
            case AttackType.DownAir:
                break;
            case AttackType.DashAttackLight:
                break;
            case AttackType.DashAttackStrong:
                break;

        }
    }

    
}
