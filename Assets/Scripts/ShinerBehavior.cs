using System.Collections;
using UnityEngine;

public class ShinerBehavior : EnemyBase
{

    Transform sprite;
    Transform hurtBox;
    CapsuleCollider2D collider;

    [Header("Attack Variables")]
    [SerializeField] float attackDistance = 1;
    [SerializeField] float attackDelay = 0.1F;
    [SerializeField] float attackCooldown = 0.1F;
    [SerializeField] float attackDuration = 0.1F;
    [SerializeField] float attackReach = 1;

    [Header("Movement Variables")]
    [SerializeField] float walkDist = 5;
    [SerializeField] float hopSpeed = 3;
    [SerializeField] float hopSpread = 90f;
    [SerializeField] float minHopDist = 4;
    [SerializeField] float maxHopDist = 8;
    [SerializeField] float hopCooldown = 5;
    [SerializeField] float hopHeight = 0.5f;
    
    
    [Header("Decision Variables")]
    bool canAttack = true;
    bool canHop = true;
    float distToTarget;


    string state;

    void Start()
    {
        //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite");
        hurtBox = transform.Find("AttackBox");
        collider = GetComponent<CapsuleCollider2D>();

        AttackTarget = GameObject.Find("Player");

        hurtBox.gameObject.SetActive(false);

        path = navGrid.FindNodePath(transform.position, AttackTarget.transform.position);
        
        //State
        state = "Search";
        
    }

    // Update is called once per frame
    void Update()
    {
        distToTarget = Vector3.Distance (transform.position, AttackTarget.transform.position);
        StartCoroutine(Pathfind());

        if (knockedBack) return;
        
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
                    //Will not move to search if player is still withing attack distance
                    state = "Search";
                    if(distToTarget <= attackDistance) state = "Attack";
                }
                break;
            case "Attacking":
                //Running Coroutine Attack()
                break;
            case "Hopping":
                break;
            default:
                state = "Search";
                break;

        }
    
    }

    void Search()
    {
        if(distToTarget <= attackDistance)
        {
            state = "Attack";
        } 
        else if(distToTarget < walkDist && LineOfSight)
        {
            //Walk towards target
            Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
            rb.linearVelocity = dir * MoveSpeed;

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            //Hop
            if (canHop)
            {
                state = "Hopping";
                StartCoroutine(Hop());
            }
        }
    }

    IEnumerator Hop()
    {
        canHop = false;

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        //Hop Calculation
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
        Vector2 variedDirection = Quaternion.Euler(0, 0, Random.Range(-hopSpread, hopSpread)) * dir;

        Vector2 hopSpot;

        if(distToTarget < maxHopDist){
            hopSpot = (Vector2)transform.position + (variedDirection * Random.Range(minHopDist, distToTarget));
        }
        else
        {
            hopSpot = (Vector2)transform.position + (variedDirection * Random.Range(minHopDist, maxHopDist));
        }

        GameObject hopNode = navGrid.FindClosestNode(hopSpot);

        //Makes sure the hopspot in not within a wall
        hopSpot = hopNode.transform.position;

        //Hop Motion
        collider.enabled = false;
        float progress = 0;
        float distance = Vector3.Distance(transform.position, hopSpot);

        rb.linearVelocity = variedDirection*hopSpeed;

        while(progress < 1f)
        {
            progress = (distance - Vector3.Distance(transform.position, hopSpot)) / distance;

            Vector2 toTarget = hopSpot - (Vector2)transform.position;
            if (Vector2.Dot(toTarget, variedDirection) <= 0)
            {
                progress = 1f;
            }

            float height = Mathf.Sin(progress * Mathf.PI) * hopHeight;
            sprite.localPosition = new Vector3(0, height, 0);

            yield return new WaitForFixedUpdate(); //Allows 1 frame of physics to happen
        }

        rb.linearVelocity = Vector2.zero;

        collider.enabled = true;

        //Hop Cooldown
        yield return new WaitForSeconds(0.25f);
        state = "Search";
        yield return new WaitForSeconds(hopCooldown);

        canHop = true;
    }


    IEnumerator Attack()
    {
        canAttack = false;

        //Pause
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(attackDelay);

        //Move AttackBox
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;

        hurtBox.transform.localPosition = Vector2.zero + (dir * attackReach);
        hurtBox.gameObject.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        hurtBox.gameObject.SetActive(false);
        state = "Search";

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        
    }

}
