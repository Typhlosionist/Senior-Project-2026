using Unity.VisualScripting;
using UnityEngine;

public class SwooperBehavior : EnemyBase
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      //Stats
        CurrentHealth = MaxHealth;

        //Objects and Components
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite");

        AttackTarget = GameObject.Find("Player");

        path = navGrid.FindNodePath(transform.position, AttackTarget.transform.position);  
    }

    // Update is called once per frame
    void Update()
    {
      StartCoroutine(Pathfind());

      GameObject furthestNode = path[0];

      for (int i = 0; i < path.Count; i++)
      {
        GameObject currentNode = path[i];

        Vector2 origin = transform.position;
        Vector2 target = currentNode.transform.position;
        Vector2 direction = (target - origin).normalized;
        float distance = Vector2.Distance(origin, target);

        LayerMask mask = LayerMask.GetMask("Default");

        // Get collider half width (for left/right offsets)
        float halfWidth = GetComponent<CapsuleCollider2D>().bounds.extents.x;

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

      //Velocity towards pathfinding node
      Vector2 dir = (furthestNode.transform.position - transform.position).normalized;
      desiredVelocity = dir * MoveSpeed;

    }
}
