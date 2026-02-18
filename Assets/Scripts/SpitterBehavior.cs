using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class SpitterBehavior : EnemyBase
{
    SpriteRenderer sprite;

    [SerializeField] float attackDistance = 8;
    float distToTarget;

    [SerializeField] float attackDelay = 0.1F;
    [SerializeField] float attackCooldown = 1F;

    [SerializeField] GameObject projectile;
    [SerializeField] float damage = 1;
    [SerializeField] float shotSpeed = 1;
    
    
    bool canAttack = true;


    string state;

    void Start()
    {
        //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        AttackTarget = GameObject.Find("Player");

        //State
        state = "Search";
        
        StartCoroutine(Attack());
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
                    //Will not move to search if player is still within attack distance
                    state = "Search";
                    if(distToTarget <= attackDistance) state = "Attack";
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
        

        GameObject shot = Instantiate(projectile, transform.position, quaternion.identity);

        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
        
        shot.GetComponent<SpitterShot>().setValues(dir, shotSpeed, damage);


        yield return new WaitForSeconds(attackDelay);

        state = "Search";
    }
}
