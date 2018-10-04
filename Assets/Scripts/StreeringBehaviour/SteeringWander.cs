using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;
using Coroutine = System.Collections.IEnumerator;

public class SteeringWander : SteeringBehavior 
{
    private static float minTargetDistance = 0.5f;

    public Vector3 target;

    public List<GameObject> path;
    public GameObject start_node;
    public GameObject end_node;

    int index = 0;


    public override void Start()
    {
        base.Start();
        RandomizeTarget(ref index);
    }

    void OnEnable()
    {
        SetStartNode();
        index = 0;
    }

    void RandomizeTarget()
    {
        float randomX = Random.Range(map.left_bound, map.right_bound) + map.center.x;
        float randomZ = Random.Range(map.Lower_bound, map.Upper_bound) + map.center.z;

        target = new Vector3(randomX, map.center.y, randomZ);
    }

    void  SetStartNode()
    {
        RaycastHit hit;
        float dis = 100000;
        foreach (GameObject go in PathGenerator.all_path_nodes)
        {
            Vector3 direction = (go.transform.position - transform.position).normalized;
            direction.y = 0;
            float distance = Vector3.Distance(go.transform.position, transform.position);

            if (Vector3.Distance(go.transform.position, transform.position) < dis)
            {
                start_node = go;
                dis = Vector3.Distance(go.transform.position, transform.position);
            }
        }
    }
    void SetEndNode()
    {
        int i = 0;
        bool done = false;
        do {
            i = Random.Range(0, PathGenerator.all_path_nodes.Count);
            if (PathGenerator.all_path_nodes[i] != start_node)
            {
                end_node = PathGenerator.all_path_nodes[i];
                done = true;
                //Debug.Log("end_node is"+end_node.name);
            }
        } while (!done);
    }

    void GeneratePath()
    {
        SetStartNode();
        SetEndNode();
        path = PathGenerator.AlgorithmA(start_node, end_node);
    }

    //this method is used for path finding
    //Add path finding code here
    void RandomizeTarget(ref int index)
    {
        if (index < path.Count)
        {
            target = path[index].transform.position;
            index++;
        }
        else
        {
            GeneratePath();
            target = path[0].transform.position;
            index = 0;
        }
    }

    public override Vector3 Acceleration
    {
        get
        {
            Vector3 toTarget = DisplacementVector(transform.position, target);
            float distance = toTarget.magnitude;

            if (distance <= minTargetDistance)
                RandomizeTarget(ref index);

            return MaxAcceleration * (toTarget / distance);
        }
    }
}
