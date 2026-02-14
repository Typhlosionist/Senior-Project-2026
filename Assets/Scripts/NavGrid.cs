using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class NavGrid : MonoBehaviour   
{
    [SerializeField] GameObject Node;
    [SerializeField] int nodesVertical = 2;
    [SerializeField] int nodesHorizontal = 2;

    [SerializeField] float nodeSpacing = 5;

    //Debug Options
    [SerializeField] bool nodesVisible = true;

    List<GameObject> nodes = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0 ; i < nodesHorizontal; i++)
        {
            for(int j = 0 ; j < nodesVertical; j++)
            {
                float offset = nodeSpacing/2;
                
                Vector2 posVec = new Vector2(i*nodeSpacing, j*nodeSpacing);

                if(j%2 != 0)
                {
                    Debug.Log("hit");
                    posVec.x += offset;
                }

                GameObject nodeObj = Instantiate(Node, (Vector2)transform.position + posVec, Quaternion.identity);

                nodes.Add(nodeObj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nodesVisible){
            foreach(GameObject obj in nodes)
            {
                SpriteRenderer objSp = obj.GetComponent<SpriteRenderer>();
                objSp.color = Color.purple;
            }
        }
        else
        {
            foreach(GameObject obj in nodes)
            {
                SpriteRenderer objSp = obj.GetComponent<SpriteRenderer>();
                objSp.enabled = false;
            }
        }
    }
}


