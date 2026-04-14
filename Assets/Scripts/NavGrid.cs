using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class NavGrid : MonoBehaviour   
{
    //Configuration options
    [SerializeField] GameObject Node;
    [SerializeField] public int nodesVertical = 2;
    [SerializeField] public int nodesHorizontal = 2;
    [SerializeField] float nodeSpacing = 5;
    [SerializeField] float detectionRadius = 0.4f;

    [SerializeField] public int enemiesPerWave = 3;
    [SerializeField] public int waveCount = 3;
    public List<int> spawnWeights;
    bool spawned = false;

    //Debug Options
    [SerializeField] bool nodesVisible = true;

    //Grid
    public List<GameObject> nodes = new List<GameObject>();

    private bool initialized = false;

    void Start()
    {
        if (!initialized)
            Initialize();
    }

    public void Initialize()
    {
        initialized = true;

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
                if(hit == null)
                {
                    GameObject nodeObj = Instantiate(Node, (Vector2)transform.position + posVec, Quaternion.identity);
                    nodeObj.transform.SetParent(transform, true);
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

        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(nodesHorizontal * nodeSpacing, nodesVertical * nodeSpacing);
        collider.offset = new Vector2((nodesHorizontal - 1) / 2.0f * nodeSpacing, (nodesVertical - 1) / 2.0f * nodeSpacing);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (spawned) return;
        if (!other.CompareTag("Player")) return;

        spawned = true;

        var spawner = GetComponentInChildren<Spawner>();
        if (spawner != null)
        {
            spawner.InitiateSpawn(enemiesPerWave, waveCount, spawnWeights);
        }

        var roomLock = GetComponentInChildren<RoomLock>();
        if (roomLock != null)
        {
            roomLock.LockRoom();
        }
        
        Destroy(gameObject.GetComponent<BoxCollider2D>());
    }
}


