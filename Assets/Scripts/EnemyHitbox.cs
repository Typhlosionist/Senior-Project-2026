using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    EnemyBase parentEnemy;

    void Start()
    {
        parentEnemy = GetComponentInParent<EnemyBase>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        parentEnemy.ObjectCollide(collision);
    }
}
