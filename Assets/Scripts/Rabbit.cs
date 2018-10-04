using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RabbitSate { wander, flee}

public class Rabbit : Monsters {

    RabbitSate rabbitState;
    GameObject flee_node;
    public static GameObject[] rabbitNode;
    public GameObject deadend;
    public GameObject jumpNode;
    public GameObject[] walls;
    bool jumping;
    AudioSource jump_ad;
    public Collider[] radius;
	// Use this for initialization
	void Start () {
        Init();
        rabbitNode = GameObject.FindGameObjectsWithTag("RabbitNode");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        jump_ad = this.GetComponent<AudioSource>();
        foreach (GameObject go in rabbitNode)
        {
            Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>());
        }

        foreach (GameObject go in walls)
        {
            Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>());
        }
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Big Carrot").GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update () {
        radius = Physics.OverlapSphere(transform.position, 4);
        if (fow.visibleTargets.Contains(sesame.transform))
        {
            rabbitState = RabbitSate.flee;
        }
        else
            rabbitState = RabbitSate.wander;

        if (jumping)
        {
            max_speed = 8;
        }
        else
        {
            max_speed = 4;
        }
        StateMechine();
        Move();
    }

    void flee()
    {
        if (end_node != findNodeToFlee() && jumpNode == null)
        {
            m_path.Clear();
            path_index = 0;
            start_node = null;
            end_node = null;
        }
        if(start_node == null && end_node == null)
        {
            if (!jumping && m_path.Count == 0 && !m_path.Contains(findNodeToFlee()))
            {
                Debug.Log("I want to jump, but " + findNodeToFlee());
                m_path.Add(findNodeToFlee());
            }

            if(!m_path[0].name.Contains("Sphere"))
            {
                jumping = true;
                Debug.Log("jump");
                jump_ad.Play();
                //deadend = findNodeToFlee();
                //if (Vector3.Distance(deadend.transform.position, transform.position) < 0.2f)
                //{
                if(jumpNode == null)
                {
                    jumpNode = findNodeToJump();
                }
                m_path.Clear();
                path_index = 0;
                Debug.Log(jumpNode);
                m_path.Add(jumpNode);
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 30, 0), ForceMode.Impulse);
                //transform.position = Vector3.Slerp(transform.position, findNodeToJump().transform.position, 10.5f);
                //}
            }
        }
    }

    GameObject findNodeToJump()
    {
        float dis = 100000;
        float max = 1;
        GameObject nodeToJump = null;
        foreach (GameObject go in PathGenerator.path_nodes["RabbitNode"])
        {
            if (!sesame.GetComponent<FieldOfView>().visibleTargets.Contains(go.transform) && !fow.visibleTargets.Contains(go.transform) && dis > Vector3.Distance(transform.position, go.transform.position))
            {
                dis = Vector3.Distance(transform.position, go.transform.position);
                nodeToJump = go;
            }
        }
        return nodeToJump;
    }

    GameObject findNodeToFlee()
    {
        float dis = 100000;
        float max = 1;
        GameObject nodeToFlee = new GameObject();
        foreach (GameObject go in PathGenerator.path_nodes["RabbitNode"])
        {
            if (!sesame.GetComponent<FieldOfView>().visibleTargets.Contains(go.transform) && fow.visibleTargets.Contains(go.transform) && max < Vector3.Distance(sesame.transform.position, go.transform.position))
            {
                max = Vector3.Distance(sesame.transform.position, go.transform.position);
                nodeToFlee = go;
            }
        }
            return nodeToFlee;

    }

    void rabbitWander(string tag)
    {
        flee_node = null;
        if (!jumpNode)
        {
            wander(tag);
        }
        else
        {
            if (Vector3.Distance(jumpNode.transform.position, transform.position) < 0.2f)
            {
                jumpNode = null;
                jumping = false;
            }
        }

    }

    void StateMechine()
    {
        switch (rabbitState)
        {
            case RabbitSate.wander:
                rabbitWander("RabbitNode");
                break;
            case RabbitSate.flee:
                flee();
                break;
            default:
                break;
        }
    }
}
