using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEditor;

public class SpitterBehavior : EnemyBase
{
    //Components
    SpriteRenderer sprite;

    [Header("Attack Values")]
    [SerializeField] float attackDistance = 10;
    float distToTarget;
    [SerializeField] float attackCooldown = 1F;

    [Header("Projectile Variables")]
    [SerializeField] GameObject projectile;
    [SerializeField] float shotSpeed = 1;

    //Behvior related variable
    bool canAttack = true;
    string state = "Search";

    void Start()
    {
        //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        AttackTarget = GameObject.Find("Player");

        path = navGrid.FindNodePath(transform.position, AttackTarget.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        distToTarget = Vector3.Distance (transform.position, AttackTarget.transform.position);
        if (!knockedBack)
        {
            Search();
        }
    }

    void Search()
    {
        StartCoroutine(Pathfind());

        if (LineOfSight && distToTarget <= attackDistance)
        {
            rb.linearVelocity = Vector2.zero;
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
            
        }
        else if(path != null)
        {
            GameObject travelNode = path[0];
            float distToNode = Vector2.Distance(transform.position, travelNode.transform.position);

            if(distToNode < moveToNodeDist)
            {
                if(path.Count >= 2)
                {
                    travelNode = path[1];
                    rb.linearVelocity = (travelNode.transform.position - transform.position).normalized * MoveSpeed;  
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
            else
            {
                rb.linearVelocity = (travelNode.transform.position - transform.position).normalized * MoveSpeed;  
            }

        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        
        GameObject shot = Instantiate(projectile, transform.position, quaternion.identity);

        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
        
        shot.GetComponent<SpitterShot>().setValues(dir, shotSpeed, Damage);


        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;

    }
}
