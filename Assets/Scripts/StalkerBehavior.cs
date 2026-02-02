using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StalkerBehavior : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    SpriteRenderer sprite;
    Transform hurtBox;

    [SerializeField] float attackDistance = 1;
    float distToTarget;

    [SerializeField] float attackDelay = 0.1F;
    [SerializeField] float attackDuration = 0.1F;
    [SerializeField] float attackCooldown = 0.1F;
    [SerializeField] float lungeSpeed = 5;
    [SerializeField] float lungeReduction = 8;
    [SerializeField] float attackReach = 2;
    
    bool canAttack = true;


    string state;

    void Start()
    {
        //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hurtBox = transform.Find("AttackBox");

        hurtBox.gameObject.SetActive(false);
        

        //State
        state = "Search";
        
    }

    // Update is called once per frame
    void Update()
    {
        distToTarget = Vector3.Distance (transform.position, AttackTarget.transform.position);

        switch (state)
        {
            case "Search":
                MoveToTarget();
                sprite.color = Color.white;
                break;
            case "Attack":
                if(canAttack){
                    StartCoroutine(Attack());
                    state = "Attacking";
                }
                else
                {
                    state = "Search";
                }
                break;
            case "Attacking":
                break;
            default:
                state = "Search";
                break;

        }
    
    }

    //TODO make the attack actually attack
    IEnumerator Attack()
    {
        sprite.color = Color.red;
        canAttack = false;

        //Pause
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(attackDelay);

        //Lunge
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;

            //AttackBox
        hurtBox.transform.localPosition = Vector2.zero + (dir * attackReach);
        hurtBox.gameObject.SetActive(true);

        rb.linearVelocity = dir * lungeSpeed;
        yield return new WaitForSeconds(attackDuration);
        rb.linearVelocity = dir * lungeSpeed / lungeReduction;

        hurtBox.gameObject.SetActive(false);

            
        
        //Cooldown (Regains movement after half cooldown, can attack after full cooldown)
        yield return new WaitForSeconds(attackCooldown/2);
        state = "Search";

        yield return new WaitForSeconds(attackCooldown/2);
        canAttack = true;
    }

    public void MoveToTarget()
    {
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
        rb.linearVelocity = dir * MoveSpeed;

        if(distToTarget <= attackDistance) state = "Attack";
    }
}
