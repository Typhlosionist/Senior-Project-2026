using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    EnemyBase parentScript;

    private void Start() {
        parentScript = GetComponentInParent<EnemyBase>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        parentScript.ObjectCollide(collision);
    }
}
