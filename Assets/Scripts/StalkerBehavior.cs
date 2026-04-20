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

    [Header("SFX")]
    [SerializeField] private AudioClip attackSFX;
    
    [SerializeField] Animator stalkerAnim;
    
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
        hurtBox.GetComponent<AttackBox>().setDamage(Damage);
        //stalkerAnim = GetComponentInChildren<Animator>();
        

        //State
        state = "Search";
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Pathfind());

        distToTarget = Vector3.Distance (transform.position, AttackTarget.transform.position);

        if(distToTarget <= attackDistance)
        {
            state = "Attack";
        }        

        if (!isNightmode && darknessController.isNight)
        {
            BecomeNightmode();
        }

        switch (state)
        {
            case "Search":
                Search();
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
                //Running Coroutine Attack()
                break;
            default:
                state = "Search";
                break;

        }
    
    }

    IEnumerator Attack()
    {
        canAttack = false;

        //Pause
        rb.linearVelocity = Vector2.zero;
        desiredVelocity = Vector2.zero;
        yield return new WaitForSeconds(attackDelay);

        stalkerAnim.SetFloat("Speed", 0);

        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;

        stalkerAnim.SetTrigger("Attack");
        SFXManager.instance.PlaySFX(attackSFX, transform, .3f);

        //AttackBox
        hurtBox.transform.localPosition = Vector2.zero + (dir * attackReach);
        hurtBox.gameObject.SetActive(true);

        if(isNightmode){
            //Lunge
            rb.linearVelocity = dir * lungeSpeed;
            desiredVelocity = rb.linearVelocity;
            yield return new WaitForSeconds(attackDuration);
            desiredVelocity = Vector2.zero;
            stalkerAnim.SetFloat("Speed", 0);
        }
        else
        {
            //Standing Attack
            yield return new WaitForSeconds(attackDuration);
        }

        hurtBox.gameObject.SetActive(false);

        //Pause after attack
        yield return new WaitForSeconds(attackCooldown/2);

        state = "Search";
        
        yield return new WaitForSeconds(attackCooldown/2);
        canAttack = true;
    }


    void BecomeNightmode()
    {
        isNightmode = true;

        attackDistance = attackDistance * 2;

        originalMoveSpeed = originalMoveSpeed * 1.25f;
        MoveSpeed = originalMoveSpeed;
    }

    public void Search()
    {
        GameObject furthestNode = path[0];

        for (int i = 0; i < path.Count; i++)
        {
            GameObject currentNode = path[i];

            Vector2 origin = transform.position;
            Vector2 target = currentNode.transform.position;
            Vector2 direction = (target - origin).normalized;
            float distance = Vector2.Distance(origin, target);

            LayerMask mask = LayerMask.GetMask("Wall");

            // Get collider half width (for left/right offsets)
            float halfWidth = GetComponent<CircleCollider2D>().bounds.extents.x;

            // Perpendicular to direction (for side offsets)
            Vector2 perp = new Vector2(-direction.y, direction.x);

            // Two ray origins (left and right edges)
            Vector2 originLeft = origin + perp * halfWidth;
            Vector2 originRight = origin - perp * halfWidth;

            RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, direction, distance, mask);
            RaycastHit2D hitRight = Physics2D.Raycast(originRight, direction, distance, mask);

            // BOTH rays must be clear
            if (hitLeft.collider == null && hitRight.collider == null)
            {
            furthestNode = currentNode;
            }
            else
            {
            break;
            }

        }

        Vector2 dir;

        //Set Velocity
        if (LineOfSight)
        {
            dir = (AttackTarget.transform.position - transform.position).normalized;
        }
        else{
            dir = (furthestNode.transform.position - transform.position).normalized;
        }

        desiredVelocity = dir * MoveSpeed;
        stalkerAnim.SetFloat("Speed", MoveSpeed);
        if (desiredVelocity.x < 0)
        {
            stalkerAnim.SetBool("BR", false);
            stalkerAnim.SetBool("FL", true);
        }
        else if (desiredVelocity.x > 0)
        {
            stalkerAnim.SetBool("FL", false);
            stalkerAnim.SetBool("BR", true);
        }

    }


}
