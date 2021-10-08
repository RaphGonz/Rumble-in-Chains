using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour //!!!
{
    private float _damages = 0;
    private float _weight;
    private int _attackFrame = 0;
    private Attack _currentAttack = null;
    private Attack _jab;
    private LayerMask enemyMask;
    public float Damages { get => _damages; }
    public float Weight { get => _weight; }
    public int AttackFrame { get => AttackFrame; set { _attackFrame = value; } }
    public Attack CurrentAttack { get => _currentAttack; set { _currentAttack = value; } }
    public Attack Jab{ get => _jab; set { _jab = value; } }

    [SerializeField]
    InputController myInputController;

    // Start is called before the first frame update
    void Start()
    {
        HitboxSphere hitbox1 = new HitboxSphere(5, 0, 3, Vector2.one, new Vector2(1, 0), 1);
        List<Hitbox> hitboxList = new List<Hitbox>();
        hitboxList.Add(hitbox1);
        _jab = new Attack(4, 4, hitboxList);
        enemyMask = this.gameObject.name.Equals("Player1") ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1");
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentAttack != null)
        {
            CheckHitboxes(_currentAttack);
        }
    }


    public void TakeDamages(float purcentages)
    {
        _damages += purcentages;
    }

    void CheckHitboxes(Attack attack)
    {
        Debug.Log("current attack is not null");
        _attackFrame++;
        if (_attackFrame >= attack.Prelag) // no need to check if under attack.Prelag + attack.AttackDuration + attack.Postlag bcs currentAttack becomes null at the moment when the frame counter is greater than this amount
        {
            foreach (var hitbox in attack.Hitboxes)
            {
                if (_attackFrame >= attack.Prelag + hitbox.StartUpTiming && _attackFrame < attack.Prelag + hitbox.StartUpTiming + hitbox.DurationOfHitbox)
                {
                    Debug.Log("attacking!");

                    if (hitbox is HitboxCapsule)
                    {
                        HitboxCapsule hitboxCapsule = (HitboxCapsule)hitbox;
                        Collider2D collider = Physics2D.OverlapCapsule(hitboxCapsule.CenterOfTheFirstSphere + new Vector2(transform.position.x, transform.position.y), new Vector2(hitboxCapsule.CenterOfTheSecondSphere.x - hitboxCapsule.CenterOfTheFirstSphere.x, hitboxCapsule.CenterOfTheSecondSphere.y - hitboxCapsule.CenterOfTheFirstSphere.y), CapsuleDirection2D.Horizontal, 0, enemyMask);
                        if (collider != null)
                        {
                            collider.gameObject.GetComponent<CharacterController>().TakeDamages(hitboxCapsule.Damage);
                            collider.gameObject.GetComponent<CharacterController>().Expel(hitboxCapsule.Expulsion);
                        }
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
                        
                        if (collider != null)
                        {
                            Debug.Log(collider.gameObject.ToString());
                            collider.gameObject.GetComponent<CharacterController>().TakeDamages(hitboxSphere.Damage); // des fois on touche la push box et pas la hurt box ce qui peut provoquer une erreur.
                            collider.gameObject.GetComponent<CharacterController>().Expel(hitboxSphere.Expulsion);
                            // en vrai, vu qu'il y a un seul character controller adverse, il suffit de get en début de partie le character controller de l'adversaire
                        }
                    }
                }
            }
        }
        if(_attackFrame >= attack.Prelag + attack.AttackDuration + attack.Postlag )
        {
            _currentAttack = null;
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

    public void Attack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Jab:
                _currentAttack = _jab;
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
