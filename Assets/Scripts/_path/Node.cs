using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public List<GameObject> path;
    public float cost;
    public float heuristic_vlaue;
    public HeuristicType heuristic;

    public Node()
    {
        path = new List<GameObject>();
        cost = 0;
        heuristic_vlaue = 0;
    }

    public Node(List<GameObject> path, float cost, HeuristicType heuristic)
    {
        this.path = path;
        this.cost = cost;
        switch (heuristic)
        {
            case HeuristicType.Dijkstra:
                break;
            case HeuristicType.Euclidean:
                break;
            case HeuristicType.Cluster:
                break;
            default:
                break;
        }
    }

    public void Set(List<GameObject> path, float cost, HeuristicType heuristic)
    {
        this.path = path;
        this.cost = cost;
        switch (heuristic)
        {
            case HeuristicType.Dijkstra:
                break;
            case HeuristicType.Euclidean:
                break;
            case HeuristicType.Cluster:
                break;
            default:
                break;
        }
    }

    public static float Dijaskra ()
    {
        return 0;
    }

    public static float Euclidean ()
    {
        return 0;
    }
}
