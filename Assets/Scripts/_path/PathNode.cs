using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public Dictionary<GameObject, float> neighbour;
    public Dictionary<GameObject, float> neighbour_connnectors;
    public Node self;
    public float total_cost;
    public NodeZone my_zone;
    public bool is_connector;
    public bool hidden_point;

	// Use this for initialization

	
    void Awake()
    {
        neighbour = new Dictionary<GameObject, float>();
        neighbour_connnectors = new Dictionary<GameObject, float>();
        self = new Node();
        self.path.Add(gameObject);
    }
    public void AddNeighbour(GameObject neighbour_node)
    {
        if (!neighbour.ContainsKey(neighbour_node))
        {
            //is this neighbournode in same zone with this path node?
            if (neighbour_node.GetComponent<PathNode>().my_zone == this.my_zone)
            {
                float edge = Vector3.Distance(neighbour_node.transform.position, this.transform.position);
                //add into neighbour
                neighbour.Add(neighbour_node, edge);

                //Draw line between 2 nodes,if not draw yet---if this node is not in some_node's neighbour, then draw
                if (!neighbour_node.GetComponent<PathNode>().neighbour.ContainsKey(this.gameObject))
                {
                    Debug.DrawLine(this.transform.position, neighbour_node.transform.position, Color.red, 200);
                }

            }
            else
            {
                //if not same zone, detect if both node is connector
                if ((this.is_connector) && (neighbour_node.GetComponent<PathNode>().is_connector))
                {
                    float edge = Vector3.Distance(neighbour_node.transform.position, this.transform.position);
                    neighbour.Add(neighbour_node, edge);
                    //also add to neighbourconnector
                    neighbour_connnectors.Add(neighbour_node, edge);
                    //Draw line between 2 nodes,if not draw yet---if this node is not in some_node's neighbour, then draw
                    if (!neighbour_node.GetComponent<PathNode>().neighbour.ContainsKey(this.gameObject))
                    {
                        Debug.DrawLine(this.transform.position, neighbour_node.transform.position, Color.red, 200);
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        GameObject[] cheeses = GameObject.FindGameObjectsWithTag("Cheese");
        foreach (GameObject go in cheeses)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), go.GetComponent<Collider>());
        }
	}

    public void UpdatePath(List<GameObject> new_path)
    {
        self.path.Clear();
        self.path.AddRange(new_path);
        self.path.Add(this.gameObject);
    }

    public void ResetPath()
    {
        self.path.Clear();
        self.path.Add(this.gameObject);
    }
}
