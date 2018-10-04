using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;
using Coroutine = System.Collections.IEnumerator;

public class SteeringFlee : SteeringBehavior
{
    public Transform target;
    public Vector3 fixedTarget;

    public GameObject start_node;
    public GameObject[] hidden_node;
    public GameObject end_node;
    public List<GameObject> path;
    public int i = 0;
    bool path_genrated = false;

    void OnEnable()
    {
        SetStartNode();
        i = 0;
        path_genrated = false;
    }

    void SetStartNode()
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
        int index = (int)Random.Range(0, 2);
        end_node = hidden_node[index];
    }

    void GeneratePath()
    {
        SetStartNode();
        SetEndNode();
        path = PathGenerator.AlgorithmA(start_node, end_node);
    }

    void NextNode(ref int index)
    {
        if (!path_genrated)
        {
            GeneratePath();
            target = path[0].transform;
            path_genrated = true;
        }
        else
        {
            if (index < path.Count)
            {
                target = path[index].transform;
                index++;
            }
            else
            {
                path_genrated = false;
                index = 0;
            }
        }
    }

    public override Vector3 Acceleration
    {
        get
        {
            if (i == 0)
            {
                NextNode(ref i);
            }
            Vector3 toVector = DisplacementVector(transform.position, target.position);
            float distance = toVector.magnitude;
            if (distance <= 0.2f)
            {
                NextNode(ref i);
            }

            return MaxAcceleration * toVector.normalized;
        }
    }

}