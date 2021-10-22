using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour //!!!
{
    private float _damages = 0;
    private float _weight;

    private int _attackFrame = 0;
    private LayerMask enemyMask;
    public float Damages { get; }
    public float Weight { get; }

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
    InputController myInputController;

    // Start is called before the first frame update
    void Start()
    {
        //HitboxSphere hitbox1 = new HitboxSphere(5, 0, 3, Vector2.one, new Vector2(1, 0), 1);
        HitboxCapsule hitbox2 = new HitboxCapsule(5, 0, 3, Vector2.one, Vector2.zero, 1*Vector2.one, 1);
        List<Hitbox> hitboxList = new List<Hitbox>();
        hitboxList.Add(hitbox2);
        Jab = new Attack(4, 4, hitboxList);
        
        enemyMask = this.gameObject.name.Equals("Player1") ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1");
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentAttack != null)
        {
            CheckHitboxes(CurrentAttack);
        }
    }


    public void TakeDamages(float purcentages)
    {
        _damages += purcentages;
        UIController.Instance.ChangePercentages(this.gameObject.name.Equals("Player1") ? 1 : 2, _damages);

    }

    void CheckHitboxes(Attack attack)
    {
        Debug.Log("current attack is not null");
        _attackFrame++;
        if (_attackFrame >= attack.Prelag) // no need to check if under attack.Prelag + attack.AttackDuration + attack.Postlag bcs currentAttack becomes null at the moment when the frame counter is greater than this amount
        {
            bool hit = false;
            foreach (var hitbox in attack.Hitboxes)
            {
                if (_attackFrame >= attack.Prelag + hitbox.StartUpTiming && _attackFrame < attack.Prelag + hitbox.StartUpTiming + hitbox.DurationOfHitbox)
                {
                    Debug.Log("attacking!");

                    if (hitbox is HitboxCapsule)
                    {
                        HitboxCapsule hitboxCapsule = (HitboxCapsule)hitbox;
                        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(hitboxCapsule.CenterOfTheFirstSphere + new Vector2(transform.position.x, transform.position.y), new Vector2(hitboxCapsule.CenterOfTheSecondSphere.x - hitboxCapsule.CenterOfTheFirstSphere.x, hitboxCapsule.CenterOfTheSecondSphere.y - hitboxCapsule.CenterOfTheFirstSphere.y), CapsuleDirection2D.Vertical, -30, enemyMask);
                        foreach(var collider in colliders)
                        {
                            Debug.Log(collider);
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
                        Collider2D collider = Physics2D.OverlapCircle(hitboxSphere.Center + new Vector2(transform.position.x, transform.position.y), hitboxSphere.Radius, enemyMask) ;
                        //FOR DEBUGGING PURPOSES
                        lastCircleRadius = hitboxSphere.Radius;
                        lastCircleCenter = hitboxSphere.Center + new Vector2(transform.position.x, transform.position.y);
                        if (collider != null && !hit)
                        {
                            hit = true;
                            Debug.Log(collider.gameObject.ToString());
                            collider.gameObject.GetComponent<CharacterController>().TakeDamages(hitboxSphere.Damage); // changer le get component : l'adversaire est unique on peut donc le faire au start
                            collider.gameObject.GetComponent<CharacterController>().Expel(hitboxSphere.Expulsion);
                            // en vrai, vu qu'il y a un seul character controller adverse, il suffit de get en début de partie le character controller de l'adversaire
                        }

                        
                    }
                }
            }
        }
        if(_attackFrame >= attack.Prelag + attack.AttackDuration + attack.Postlag )
        {
            CurrentAttack = null;
            Debug.Log("current attack is now null");
            _attackFrame = 0;
            myInputController.attacking = false;
        }
        
    }
    
    void Expel(Vector2 expelForce)
    {
        //PlayerController.instance.velocity += expelForce;
        Debug.Log("expelled !");
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(lastCircleCenter, lastCircleRadius);
       
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
