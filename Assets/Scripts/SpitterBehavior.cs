using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEditor;

public class SpitterBehavior : EnemyBase
{

    [Header("Attack Values")]
    [SerializeField] float attackDistance = 10;
    float distToTarget;
    [SerializeField] float attackCooldown = 1F;

    [Header("Projectile Variables")]
    [SerializeField] GameObject projectile;
    [SerializeField] float shotSpeed = 1;

    //Behvior related variable
    bool canAttack = true;
    Animator spitterAnim;

    void Start()
    {
        //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite");
        spitterAnim = GetComponent<Animator>();

        AttackTarget = GameObject.Find("Player");

        path = navGrid.FindNodePath(transform.position, AttackTarget.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if(darknessController.isNight && !isNightmode)
        {
            BecomeNightmode();
        }
        distToTarget = Vector3.Distance (transform.position, AttackTarget.transform.position);
        if (!knockedBack)
        {
            Search();
        }
    }

    void Search()
    {
        StartCoroutine(Pathfind());

        //Attack Player
        if (LineOfSight && distToTarget <= attackDistance)
        {
            desiredVelocity = Vector2.zero;
            spitterAnim.SetFloat("Speed", 0);
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
            
        }

        //Move Towards Player
        else if(path != null)
        {

            GameObject travelNode;

            if(path.Count == 1)
            {
                travelNode = path[0];
            }
            else
            {
                travelNode = path[1];
            }


            spitterAnim.SetFloat("Speed", MoveSpeed);
            desiredVelocity = (travelNode.transform.position - transform.position).normalized * MoveSpeed; 
            if (desiredVelocity.x < 0)
            {
                spitterAnim.SetBool("BR", false);
                spitterAnim.SetBool("FL", true);
            }
            else if (desiredVelocity.x > 0)
            {
                spitterAnim.SetBool("FL", false);
                spitterAnim.SetBool("BR", true);
            }

        }
    }

    IEnumerator Attack()
    {
        canAttack = false;


        GameObject shot = Instantiate(projectile, transform.position, quaternion.identity);
        Vector2 dir = (AttackTarget.transform.position - transform.position).normalized;
        if (dir.x < 0)
        {
            spitterAnim.SetBool("BR", false);
            spitterAnim.SetBool("FL", true);
        }
        else if (dir.x > 0)
        {
            spitterAnim.SetBool("FL", false);
            spitterAnim.SetBool("BR", true);
        }
        spitterAnim.SetTrigger("Attack");
        shot.GetComponent<SpitterShot>().setValues(dir, shotSpeed, Damage, isNightmode);

        if (isNightmode)
        {
            yield return new WaitForSeconds(0.25f);
            shot = Instantiate(projectile, transform.position, quaternion.identity);
            dir = (AttackTarget.transform.position - transform.position).normalized;
            shot.GetComponent<SpitterShot>().setValues(dir, shotSpeed, Damage, isNightmode);

            yield return new WaitForSeconds(0.25f);
            shot = Instantiate(projectile, transform.position, quaternion.identity);
            dir = (AttackTarget.transform.position - transform.position).normalized;
            shot.GetComponent<SpitterShot>().setValues(dir, shotSpeed, Damage, isNightmode);
        }


        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;

    }

    void BecomeNightmode()
    {
        isNightmode = true;
    }
    //
    // protected override void OnTriggerEnter2D(Collider2D collision)
    // {
    //     base.OnTriggerEnter2D(collision); 
    // }
}
