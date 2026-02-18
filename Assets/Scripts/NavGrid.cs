using UnityEngine;
using System.Collections.Generic;

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
                    posVec.x += offset;
                }


                //TODO: get collision detection working
                Collider2D hit = Physics2D.OverlapCircle(posVec, nodeSpacing/4);
                if(hit == null)
                {
                    GameObject nodeObj = Instantiate(Node, (Vector2)transform.position + posVec, Quaternion.identity);
                    nodeObj.GetComponent<node>().parentSpawner = this;

                    nodes.Add(nodeObj);
                }


            }
        }

        //Adjacency
        foreach(GameObject obj in nodes)
        {
            foreach(GameObject other in nodes)
            {
                float distance = Vector2.Distance(obj.transform.position, other.transform.position);

                if(obj != other && distance <= nodeSpacing*1.5)
                {
                    obj.GetComponent<node>().AddNeighboor(other);
                }
            }
        }

        //Rendering
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

    // Update is called once per frame
    void Update()
    {
        if(nodesVisible){
            foreach(GameObject obj in nodes)
            {
                obj.GetComponent<node>().DrawConnections();
            }
        }
        
    }
}


