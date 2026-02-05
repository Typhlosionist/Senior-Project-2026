using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    
    private Camera cam;
    public Vector3 targetPos {get; set;}
    public GameObject bullet;
    public Transform bulletTrans;
    public bool canFire = true;
    private float timer;
    public float shootCooldown = .3f;
    
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (gameObject.tag == "Player")
        {
            targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (gameObject.tag == "bad")
        {
            targetPos =  GameObject.Find("Player").transform.position;
        }

        Vector3 rotation = targetPos - transform.position;
        
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > shootCooldown)
            {
                canFire = true;
                timer = 0;
            }
        }
        
        if (Input.GetMouseButton(0) && canFire && gameObject.tag == "Player")
        {
            canFire = false;
            GameObject bull;
            bull = Instantiate(bullet, bulletTrans.position, Quaternion.identity);
            bull.tag = gameObject.tag;
        }

        if (canFire && gameObject.tag != "Player")
        {
            canFire = false;
            GameObject bull;
            bull = Instantiate(bullet, bulletTrans.position, Quaternion.identity);
            bull.tag = gameObject.tag;
        }
    }
}
