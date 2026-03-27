using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StalkerBehavior : EnemyBase
{
    Transform hurtBox;

    [Header("Attack Variables")]
    [SerializeField] float attackDistance = 1;
    float distToTarget;
    [SerializeField] float attackDelay = 0.1F;
    [SerializeField] float attackDuration = 0.1F;
    [SerializeField] float attackCooldown = 0.1F;
    [SerializeField] float attackReach = 2;

    [Header("Lunging Variable")]
    [SerializeField] float lungeSpeed = 5;
    [SerializeField] float lungeReduction = 8;

    [Header("Skulking Variables")]
    [SerializeField] float skulkTime = 1;
    [SerializeField] float skulkSpeed = 3;
    
    
    bool canAttack = true;


    string state;

    void Start()
    {
        //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite");
        hurtBox = transform.Find("AttackBox");

        AttackTarget = GameObject.Find("Player");

        hurtBox.gameObject.SetActive(false);
        

        //State
        state = "Search";
        
    }

    // Update is called once per frame
    void Update()
    {
        distToTarget = Vector3.Distance (transform.position, AttackTarget.transform.position);

        if (!isNightmode && darknessController.isNight)
        {
            BecomeNightmode();
        }

        switch (state)
        {
            case "Search":
                MoveToTarget();
                sprite.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case "Attack":
                if(canAttack){
                    StartCoroutine(Attack());
                    state = "Attacking";
                }
                else
                {
                    //Will not move to search if player is still withing attack distance
                    state = "Search";
                    if(distToTarget <= attackDistance) state = "Attack";
                }
                break;
            case "Attacking":
                //Running Coroutine Attack()
                break;
            case "Skulk":
                //Running Corouting Skulk()
                break;
            default:
                state = "Search";
                break;

        }
    
    }

    //TODO make the attack actually attack
    IEnumerator Attack()
    {
        sprite.GetComponent<SpriteRenderer>().color = Color.red;
        canAttack = false;

        //Pause
        rb.linearVelocity = Vector2.zero;
        desiredVelocity = Vector2.zero;
        yield return new WaitForSeconds(attackDelay);

        
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;

        //AttackBox
        hurtBox.transform.localPosition = Vector2.zero + (dir * attackReach);
        hurtBox.gameObject.SetActive(true);

        if(isNightmode){
            //Lunge
            rb.linearVelocity = dir * lungeSpeed;
            desiredVelocity = rb.linearVelocity;
            yield return new WaitForSeconds(attackDuration);
            desiredVelocity = Vector2.zero;
        }
        else
        {
            //Standing Attack
            yield return new WaitForSeconds(attackDuration);
        }

        hurtBox.gameObject.SetActive(false);

        //Pause after attack
        yield return new WaitForSeconds(attackCooldown/2);

        if(isNightmode){    
            //skulk TODO: Awaiting Implementation
            /*
            state = "Skulk";
            StartCoroutine(Skulk());
            */
            state = "Search";
        }
        else state = "Search";
        

        yield return new WaitForSeconds(attackCooldown/2);
        canAttack = true;
    }

    //Stalker Skulks away after attacking
    /*
    IEnumerator Skulk()
    {
        Note: use similar code to the frog jump in order to find a retreat direction that does not collide with other stalkers
    }
    */

    void BecomeNightmode()
    {
        sprite.GetComponent<SpriteRenderer>().color = Color.purple;
        isNightmode = true;
    }

    public void MoveToTarget()
    {
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
        rb.linearVelocity = dir * MoveSpeed;

        if(distToTarget <= attackDistance) state = "Attack";
    }
}
