using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullter : MonoBehaviour
{
    private Vector3 targetPos;
    private Camera cam;
    public InputActionReference aim;
    private Rigidbody2D rb;
    public float force;

    private IEnumerator coroutine;
    public float time = 10f;
    void Start()
    {
        if (gameObject.tag == "Player")
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            targetPos = GameObject.Find("rotationPoint").GetComponent<Shooting>().targetPos;
        }
        else
        {
            targetPos = GameObject.Find("Player").transform.position;
        }
        
        fire();
    }

    void fire()
    {
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        //targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = targetPos - transform.position;
        Vector3 rotation = transform.position - targetPos;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        coroutine = destroyAfterTime(time);
        StartCoroutine(coroutine);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator destroyAfterTime(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            Destroy(this.gameObject);
        }
    }
}