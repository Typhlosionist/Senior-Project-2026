using UnityEngine;
using System.Collections.Generic;

public class node : MonoBehaviour
{
    public NavGrid parentSpawner;
    public int ID;
    public List<GameObject> neighbors = new List<GameObject>();

    public void DrawConnections()
    {
        foreach(GameObject neighbor in neighbors)
        {
            Debug.DrawLine(this.transform.position, neighbor.transform.position, Color.white, 0f);
        }
    }

    public void AddNeighboor(GameObject neighbor)
    {
        neighbors.Add(neighbor);
    }

}
