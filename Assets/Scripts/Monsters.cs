using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { hide, flee, wander, eat};

public class Monsters : NpcCharacters {

    public GameObject start_node;
    public GameObject end_node;
    static public GameObject cheese;
    public float max_speed;
    public PlayerController player;


    public List<GameObject> m_path;
    bool has_path = false;
    public int path_index = 0;


    public static bool CatHere;
    State mouseState = State.wander;

    GameObject leftSencer;
    GameObject RightSencer;

    GameObject[] pipes;
    List<GameObject> hiddenNodes;

	// Use this for initialization
	void Start () {
        Init();
        scales = transform.localScale / 2;
        pipes = GameObject.FindGameObjectsWithTag("Pipes");
        
        //Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Cheese (1)").GetComponent<Collider>());
    }

	public void Init()
    {
        m_path = new List<GameObject>();
        hiddenNodes = new List<GameObject>();
        //leftSencer = transform.GetChild(1).gameObject;
        //RightSencer = transform.GetChild(2).gameObject;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.name == "In Tube")
        {
            Hidden = true;
        }
    }

	public void SetStartNode()
    {
        RaycastHit hit;
        float dis = 100000;
        float max = 1;
        foreach (GameObject go in PathGenerator.path_nodes["PathNode"])
        {
            if (mouseState == State.hide)
            {
                if (GetComponent<FieldOfView>().visibleTargets.Contains(go.transform) && Vector3.Distance(go.transform.position, transform.position) < dis && max < Vector3.Distance(GameObject.Find("Sesame").transform.position, go.transform.position))
                {
                    start_node = go;
                    max = Vector3.Distance(GameObject.Find("Sesame").transform.position, go.transform.position);
                    dis = Vector3.Distance(go.transform.position, transform.position);
                }
            }
            if (mouseState == State.flee)
            {
                if (go.name != "Cheese (1)" && !go.name.Contains("In Tube") && GetComponent<FieldOfView>().visibleTargets.Contains(go.transform) && max < Vector3.Distance(GameObject.Find("Sesame").transform.position, go.transform.position))
                {
                    start_node = go;
                    max = Vector3.Distance(GameObject.Find("Sesame").transform.position, go.transform.position);
                    dis = Vector3.Distance(go.transform.position, transform.position);
                }
            }
        }
    }

    public void SetEndNode()
    {
        if (end_node == null)
        {
            int index = Random.Range(0, hiddenNodes.Count);

            float distance = 1000000;

            foreach (GameObject g in hiddenNodes)
            {
                if (distance > Vector3.Distance(start_node.transform.position, g.transform.position))
                {
                    distance = Vector3.Distance(start_node.transform.position, g.transform.position);
                    end_node = g;
                }
            }
                //Debug.Log("end_node is"+end_node.name);
           
        }

    }
    
    protected void Move()
    {
        if (path_index < m_path.Count)
        {
            if (Vector3.Distance(transform.position, m_path[path_index].transform.position) < 0.3f)
            {
                //if (path_index > 0)
                //{
                //    m_path[path_index - 1].GetComponent<LineRenderer>().enabled = false;
                //}
                //Debug.Log("Next");
                path_index++;
            }
            else
            {
                Vector3 direction = (m_path[path_index].transform.position - transform.position).normalized;
                transform.LookAt(direction);
                rigidbody.velocity = direction * max_speed;
                transform.LookAt(new Vector3(m_path[path_index].transform.position.x, transform.position.y, m_path[path_index].transform.position.z));

                
            }
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            m_path.Clear();
            path_index = 0;
            start_node = null;
            end_node = null;
            has_path = false;
        }
    }
    public void CreatePath()
    {
        if (!has_path)
        {
            Debug.Log("start:" + start_node.name);
            Debug.Log("end: " + end_node.name);
            m_path = PathGenerator.AlgorithmA(start_node, end_node);
            has_path = true;
        }
        //for (int i = 0; i < m_path.Count - 1; i++)
        //{
        //    m_path[i].GetComponent<LineRenderer>().enabled = true;
        //    m_path[i].GetComponent<LineRenderer>().SetPosition(0, m_path[i].transform.position);
        //    m_path[i].GetComponent<LineRenderer>().SetPosition(1, m_path[i + 1].transform.position);
        //}
    }

    public void hide()
    {
        if (!hiddenNodes.Contains(end_node))
        {
            m_path.Clear();
            path_index = 0;
            start_node = null;
            end_node = null;
        }
        if (start_node == null && end_node == null) 
        {
            SetStartNode();
            SetEndNode();
            m_path = PathGenerator.AlgorithmA(start_node, end_node);
        }
        if(Vector3.Distance(end_node.transform.position, transform.position) < 0.2f)
        {
            max_speed = 0;
        }
    }

    public void wander(string tag)
    {
        if(start_node == null && end_node == null)
        {
            m_path.Clear();
            path_index = 0;
            int random = Random.Range(0, PathGenerator.path_nodes[tag].Count);
            start_node = findClosestNode(tag);
            end_node = PathGenerator.path_nodes[tag][random];
            m_path = PathGenerator.AlgorithmA(start_node, end_node);
        }
    }

    void flee()
    {
        if(m_path.Contains(GameObject.Find("In Tube")) || m_path.Contains(GameObject.Find("Cheese (1)")) || start_node == GameObject.Find("In Tube"))
        {
            path_index = 0;
            m_path.Clear();
            start_node = null;
            end_node = null;
        }
        if (start_node == null && end_node == null)
        {
            SetStartNode();
            m_path.Add(start_node);
        }
    }

    void eat()
    {
        if (!m_path.Contains(GameObject.Find("Cheese (1)")))
        {
            m_path.Clear();
            path_index = 0;
            start_node = null;
            end_node = null;
        }

        if(start_node == null && end_node == null)
        {
            start_node = findClosestNode("PathNode");
            end_node = GameObject.Find("Cheese (1)");
            m_path = PathGenerator.AlgorithmA(start_node, end_node);
        }
        if (end_node == GameObject.Find("Cheese (1)"))
        {
            if (Vector3.Distance(end_node.transform.position, transform.position) < 0.2f)
            {
                max_speed = 0;
            }
        }
    }

    GameObject findClosestNode(string tag)
    {
        float dis = 100000;

        GameObject closestNode = null;
        foreach (GameObject go in PathGenerator.path_nodes[tag])
        {
            Vector3 direction = (go.transform.position - transform.position).normalized;
            direction.y = 0;
            float distance = Vector3.Distance(go.transform.position, transform.position);

            if (Vector3.Distance(go.transform.position, transform.position) < dis)
            {
                closestNode = go;
                dis = Vector3.Distance(go.transform.position, transform.position);
            }

        }
        return closestNode;
    }
    //public void PathToCheese()
    //{
    //    SetStartNode();
    //}
    void FiniteStateMechine()
    {
        switch (mouseState)
        {
            case State.hide:
                hide();
                break;
            case State.flee:
                flee();
                break;
            case State.wander:
                wander("PathNode");
                break;
            case State.eat:
                eat();
                break;
            default:
                break;
        }
    }
    void Update()
    {
        foreach (GameObject g in pipes)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), g.GetComponent<Collider>());
        }

        foreach (GameObject g in PathGenerator.path_nodes["PathNode"])
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), g.GetComponent<Collider>());
            if (g.name.Contains("In Tube"))
            {
                hiddenNodes.Add(g);
            }
        }
        if (!CatHere && !PathGenerator.path_nodes["PathNode"].Contains(cheese))
        {
            max_speed = 5;
            mouseState = State.wander;
        }
        if (!PathGenerator.path_nodes["PathNode"].Contains(cheese) && GameObject.Find("Sesame") && CatHere)
        {
            Debug.Log("hiding");

            mouseState = State.hide;
        }
        if (GetComponent<FieldOfView>().visibleTargets.Contains(GameObject.Find("Sesame").transform) && PathGenerator.path_nodes["PathNode"].Contains(cheese))
        {
            Debug.Log("fleeing");
            max_speed = 5;
            mouseState = State.flee;
        }
        if (!GetComponent<FieldOfView>().visibleTargets.Contains(GameObject.Find("Sesame").transform) && PathGenerator.path_nodes["PathNode"].Contains(cheese))
        {
            Debug.Log("eating");
            max_speed = 5;
            mouseState = State.eat;
        }
        FiniteStateMechine();
        Move();
    }
}
