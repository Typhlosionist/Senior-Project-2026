using UnityEngine;
using System.Collections.Generic;

public class NavGrid : MonoBehaviour   
{
    //Configuration options
    [SerializeField] GameObject Node;
    [SerializeField] int nodesVertical = 2;
    [SerializeField] int nodesHorizontal = 2;
    [SerializeField] float nodeSpacing = 5;
    [SerializeField] float detectionRadius = 0.4f;

    //Debug Options
    [SerializeField] bool nodesVisible = true;

    //Grid
    public List<GameObject> nodes = new List<GameObject>();


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

                Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + posVec, detectionRadius, LayerMask.GetMask("Wall"));
                //Debug.Log(hit);
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

        //Cleanup
        KeepLargestCluster();



    }

    //Helper
    void KeepLargestCluster()
{
    HashSet<GameObject> visited = new HashSet<GameObject>();
    List<List<GameObject>> clusters = new List<List<GameObject>>();

    // Find connected clusters using BFS
    foreach (GameObject startNode in nodes)
    {
        if (visited.Contains(startNode))
            continue;

        List<GameObject> cluster = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            cluster.Add(current);

            List<GameObject> neighbors = current.GetComponent<node>().neighbors;

            foreach (GameObject neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        clusters.Add(cluster);
    }

    // Find largest cluster
    List<GameObject> largestCluster = null;
    int maxSize = 0;

    foreach (var cluster in clusters)
    {
        if (cluster.Count > maxSize)
        {
            maxSize = cluster.Count;
            largestCluster = cluster;
        }
    }

    if (largestCluster == null)
        return;

    // Remove nodes not in largest cluster
    for (int i = nodes.Count - 1; i >= 0; i--)
    {
        if (!largestCluster.Contains(nodes[i]))
        {
            Destroy(nodes[i]);
            nodes.RemoveAt(i);
        }
    }
}

    // Update is called once per frame
    void Update()
    {
        //Debug
        if(nodesVisible){
            foreach(GameObject obj in nodes)
            {
                obj.GetComponent<node>().DrawConnections();
            }
        }

        //Rendering
        if(nodesVisible){
            foreach(GameObject obj in nodes)
            {
                SpriteRenderer objSp = obj.GetComponent<SpriteRenderer>();
                objSp.color = Color.purple;
                objSp.enabled = true;
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

    //Returns a path of nodes from one node to another
    public List<GameObject> FindNodePath(Vector2 startPos, Vector2 endPos)
    {
        GameObject startNode = FindClosestNode(startPos);
        GameObject endNode = FindClosestNode(endPos);

        Queue<GameObject> queue = new Queue<GameObject>();
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        HashSet<GameObject> visited = new HashSet<GameObject>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            if (current == endNode)
                return ReconstructPath(cameFrom, startNode, endNode);

            foreach (GameObject neighbor in current.GetComponent<node>().neighbors)
            {
                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                cameFrom[neighbor] = current;
                queue.Enqueue(neighbor);
            }
        }

        return null; // No path found
    }

    //Helper Function for FindNodePath
    static List<GameObject> ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject start, GameObject goal)
    {
        List<GameObject> path = new List<GameObject>();
        GameObject current = goal;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    //Returns the closest node to the given position
    public GameObject FindClosestNode(Vector2 position)
    {
        GameObject closestNode = nodes[0];

        float distToCN = Vector2.Distance(position, closestNode.transform.position);

        foreach(GameObject obj in nodes)
        {
            float newDist = Vector2.Distance(position, obj.transform.position);

            if (newDist < distToCN)
            {
                closestNode = obj;
                distToCN = newDist;
            }
        }

        return closestNode;
    }
}


