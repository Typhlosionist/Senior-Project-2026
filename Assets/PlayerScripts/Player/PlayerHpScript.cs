using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpScript : MonoBehaviour
{

    public bool god = false;
    private int maxHp = 3;
    public int currentHp;
    
    public SpriteRenderer hp1;
    public SpriteRenderer hp2;
    public SpriteRenderer hp3;

    public float iFrames;
    public int flashes;
    public SpriteRenderer sprite;
    
    void Start()
    {
        currentHp = maxHp;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void heal(int health)
    {
        if (currentHp < 3)
        {
            currentHp += health;
            increaseHp();
        }
    }

    public void hurt(int damage)
    {
        if (!god)
        {
            currentHp -= damage;
            reduceHp();
            if (currentHp <= 0)
            {
                die();
            }
        }
    }

    public void die()
    {
        //play death anim
        //go to gameover
        Destroy(gameObject);
        Debug.Log("RIP");
    }
    
    public void reduceHp()
    {
        if (currentHp == 0)
        {
            hp3.color = new Color(hp3.color.r, hp3.color.g, hp3.color.b, 0.5f);
            hp2.color = new Color(hp2.color.r, hp2.color.g, hp2.color.b, 0.5f);
            hp1.color = new Color(hp1.color.r, hp1.color.g, hp1.color.b, 0.5f);
        }
        if (currentHp == 1)
        {
            hp3.color = new Color(hp3.color.r, hp3.color.g, hp3.color.b, 0.5f);
            hp2.color = new Color(hp2.color.r, hp2.color.g, hp2.color.b, 0.5f);
        }
        if (currentHp == 2)
        {
            hp3.color = new Color(hp3.color.r, hp3.color.g, hp3.color.b, 0.5f);
        }

        StartCoroutine(invuln());
    }
    
    public void increaseHp()
    {
        if (currentHp == 3)
        {
            hp3.color = new Color(hp3.color.r, hp3.color.g, hp3.color.b, 1f);
        }
        if (currentHp == 2)
        {
            hp2.color = new Color(hp2.color.r, hp2.color.g, hp2.color.b, 1f);
        }
    }

    private IEnumerator invuln()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < flashes; i++)
        {
            sprite.color = new Color(1, 0, 0, .5f);
            yield return new WaitForSeconds(iFrames / (flashes * 2));
            sprite.color = Color.white;
            yield return new WaitForSeconds(iFrames / (flashes *2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
