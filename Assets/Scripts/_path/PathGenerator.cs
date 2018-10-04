using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[System.Serializable]
public class PathGenerator : MonoBehaviour
{
 
    [SerializeField] GameObject start_node;
    [SerializeField]
    GameObject end_node;
    public List<GameObject> path;
    public string[] tag;

    static public HeuristicType heuristic_type;
    static public List<GameObject> all_path_nodes;
    static public Dictionary<string, List<GameObject>> path_nodes;
    static List<GameObject> all_connector_nodes;
    static Dictionary<GameObject, Dictionary<GameObject, Node>> connecters_path_table;

    bool is_calculated = false;

    // Use this for initialization
    private void Awake()
    {
        all_path_nodes = new List<GameObject>();
        all_connector_nodes = new List<GameObject>();
        connecters_path_table = new Dictionary<GameObject, Dictionary<GameObject, Node>>();
        path = new List<GameObject>();
        path_nodes = new Dictionary<string, List<GameObject>>();

        heuristic_type = HeuristicType.Dijkstra;
    }

    void Start()
    {
        GetAllNodes(tag);
        FindNeighbourForEachNode(tag);
        //CreateAConnectorTable();

    }

    public void GenerateNewNodeNetwork()
    {
        path_nodes.Clear();
        GetAllNodes(tag);
        FindNeighbourForEachNode(tag);
    }


    public void GetAllNodes(string[] tag)
    {
        foreach (string t in tag)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag(t);
            List<GameObject> temp_list = new List<GameObject>();
            foreach (GameObject go in temp)
            {
                temp_list.Add(go);
                //if (go.GetComponent<PathNode>().is_connector)
                //{
                //    all_connector_nodes.Add(go);
                //}
            }
            path_nodes.Add(t, temp_list);
        }
        //Debug.Log("total path: " + all_path_nodes.Count);
        //Debug.Log("total connector: " + all_connector_nodes.Count);
    }

    public void CreateAConnectorTable()
    {

        foreach (GameObject g1 in all_connector_nodes)
        {
            Dictionary<GameObject, Node> g1_pathes_to_anthors = new Dictionary<GameObject, Node>();
            foreach (GameObject g2 in all_connector_nodes)
            {
                Node all_paths_and_cost_btwg1g2 = new Node();
                if (g1 != g2)
                {
                    List<GameObject> temp_pathes_list = AlgorithmA(g1, g2);
                    float cost = g2.GetComponent<PathNode>().total_cost;
                    all_paths_and_cost_btwg1g2.Set(temp_pathes_list, cost, HeuristicType.Dijkstra);
                }
                g1_pathes_to_anthors.Add(g2, all_paths_and_cost_btwg1g2);
            }

            connecters_path_table.Add(g1, g1_pathes_to_anthors);
        }
    }

    public void FindNeighbourForEachNode(string[] tag)
    {
        foreach (string item in tag)
        {
            foreach (GameObject g1 in path_nodes[item])
            {
                foreach (GameObject g2 in path_nodes[item])
                {
                    if (g1 != g2)
                    {
                        Vector3 direction = g2.transform.position - g1.transform.position;
                        direction.Normalize();

                        float distance = Vector3.Distance(g2.transform.position, g1.transform.position);
                        //using raycast to find if g2 is visible to g1
                        RaycastHit hit;
                        if (Physics.Raycast(g1.transform.position, direction, out hit, distance))
                        {
                            if (hit.collider.gameObject != null && hit.collider.tag == item)
                            {
                                //Debug.Log("neighbour:" + hit.collider.name);
                                g1.GetComponent<PathNode>().AddNeighbour(hit.collider.gameObject);
                            }
                        }
                    }
                }
                g1.GetComponent<PathNode>().neighbour = g1.GetComponent<PathNode>().neighbour.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            }
        }
        //double foreach loop to get neighbour of each node
       
    }
    static public List<GameObject> AlgorithmA(GameObject start, GameObject end)
    {
        //Debug.Log("A, start:" + start.name);
        //Debug.Log("A, end:" + end.name);
        List<GameObject> open_list = new List<GameObject>();
        List<GameObject> close_list = new List<GameObject>();
        List<GameObject> a_path = new List<GameObject>();
        foreach (KeyValuePair<string, List<GameObject>> go in path_nodes)
        {
            foreach (GameObject g in go.Value)
            {
                g.GetComponent<PathNode>().total_cost = 0;
                g.GetComponent<PathNode>().ResetPath();
            }
        }
        StartAlgorithmA(start, end, close_list, open_list);
        foreach (GameObject go in end.GetComponent<PathNode>().self.path)
        {
            a_path.Add(go);
        }
        //Debug.Log("Path length" +a_path.Count);
        return a_path;
    }

    //in Eucliean distacne heurstic, using the distance between each node to the end node as their heurstic vaule
    static public void AlgorithmEDH(GameObject start, GameObject end)
    {
       
        start.GetComponent<PathNode>().self.heuristic_vlaue = Vector3.Distance(end.transform.position, start.transform.position);
    }

    static public List<GameObject> AlgorithmCluster(GameObject previous_node, GameObject next_node)
    {

        // Dictionary<GameObject,Dictionary<list<gameobject>,float> 
        List<GameObject> connectors_in_start_cluster = new List<GameObject>();
        List<GameObject> connectors_in_end_cluster = new List<GameObject>();
        float temp_cost = 10000000f;

        List<GameObject> c_path = new List<GameObject>();
        //step0: check if 2 node in same zone
        NodeZone previouse_zone = previous_node.GetComponent<PathNode>().my_zone;
        NodeZone next_zone = next_node.GetComponent<PathNode>().my_zone;

        if (previouse_zone == next_zone)
        {

            return AlgorithmA(previous_node, next_node);
        }

        //step1: find all connector nodes in start cluster && all connector nodes in end cluster
        for (int i = 0; i < all_connector_nodes.Count; i++)
        {
            if (all_connector_nodes[i].GetComponent<PathNode>().my_zone == previouse_zone)
            {
                connectors_in_start_cluster.Add(all_connector_nodes[i]);
            }
            else if (all_connector_nodes[i].GetComponent<PathNode>().my_zone == next_zone)
            {
                connectors_in_end_cluster.Add(all_connector_nodes[i]);
            }
        }

        //step2: For each connector node in start cluster, find all path from previous node to each connector
        //add them to dictionary with key which is that connector node 

        Dictionary<GameObject, Node> all_path_form_previous_to_all_start_cluster = new Dictionary<GameObject, Node>();
        //(endnode,pathfromstart(allpath,totalcost)

        foreach (GameObject go in connectors_in_start_cluster)
        {
            List<GameObject> pathes_btw_previous_go = new List<GameObject>();
            pathes_btw_previous_go = AlgorithmA(previous_node, go);
            float cost = go.GetComponent<PathNode>().total_cost;
            Node pathes_and_cost_btw_previous_go = new Node(pathes_btw_previous_go, cost, HeuristicType.Dijkstra);
            //add into the table of all_path_from...
            all_path_form_previous_to_all_start_cluster.Add(go, pathes_and_cost_btw_previous_go);

        }
        //step3: For each connector node in end cluster, find all path from  each connector to end node

        Dictionary<GameObject, Node> all_path_form_all_end_cluster_to_next = new Dictionary<GameObject, Node>();
        //(startnode,pathtonextnode(allpathes,totalcost))


        foreach (GameObject go in connectors_in_end_cluster)
        {
            List<GameObject> pathes_btw_go_next = new List<GameObject>();
            pathes_btw_go_next = AlgorithmA(go, next_node);
            float cost = go.GetComponent<PathNode>().total_cost;
            Node pathes_and_cost_btwin_go_next = new Node(pathes_btw_go_next, cost, HeuristicType.Dijkstra);

            all_path_form_all_end_cluster_to_next.Add(go, pathes_and_cost_btwin_go_next);

        }
        // Step4:try to find a path can connect from start zone to end zone
        // by match ( end node of table of start cluster) to (startnode from connectertable) 
        //then match ( end node from connectertable) to (start node of table of endcluster)

        foreach (KeyValuePair<GameObject, Node> start_kvp in all_path_form_previous_to_all_start_cluster)
        {

            // beacuse finding path with algorithA ==> only one element in Dictionary<List<GameObject>, float>> 
            float startcost = start_kvp.Value.cost;
            List<GameObject> temp_start_path = start_kvp.Value.path;


            foreach (KeyValuePair<GameObject, Node> end_kvp in all_path_form_all_end_cluster_to_next)
            {

                float endcost = end_kvp.Value.cost;
                List<GameObject> temp_end_path = end_kvp.Value.path;

                KeyValuePair<GameObject, Dictionary<GameObject, Node>> pair1 = new KeyValuePair<GameObject, Dictionary<GameObject, Node>>(start_kvp.Key, connecters_path_table[start_kvp.Key]);
                KeyValuePair<GameObject, Node> pair2 = new KeyValuePair<GameObject, Node>(end_kvp.Key, connecters_path_table[start_kvp.Key][end_kvp.Key]);

                float temp_total_cost = startcost + endcost + pair2.Value.cost;
                //find path with shortest cost===> add to path
                if (temp_total_cost < temp_cost)
                {
                    temp_cost = temp_total_cost;
                    // List<GameObject> temp_total_path = new List<GameObject>();
                    c_path.AddRange(temp_start_path);
                    c_path.AddRange(pair2.Value.path);
                    c_path.AddRange(temp_end_path);
                }
            }
        }
        // Debug.Log(c_path.Count);
        return c_path;
    }
    //private void CreatePath()
    //{
    //    if (!is_calculated)
    //    {
    //        path = AlgorithmCluster(start_node, end_node);

    //        is_calculated = true;
    //    }
    //    for (int i = 0; i < path.Count - 1; i++)
    //    {
    //        path[i].GetComponent<LineRenderer>().enabled = true;
    //        path[i].GetComponent<LineRenderer>().SetPosition(0, path[i].transform.position);
    //        path[i].GetComponent<LineRenderer>().SetPosition(1, path[i + 1].transform.position);
    //    }
    //}
    static private void StartAlgorithmA(GameObject start, GameObject end, List<GameObject> close, List<GameObject> open)
    {

        if (start != end)
        {
            //Move the current node from the open to close,
            open.Remove(start);
            close.Add(start);
            //Make sure that the firt node's heuristic value is calculated
            if (heuristic_type != 0)
            {
                switch (heuristic_type)
                {
                    case HeuristicType.Dijkstra:
                        start.GetComponent<PathNode>().self.heuristic = 0;
                        break;
                    case HeuristicType.Euclidean:
                        AlgorithmEDH(start, end);
                        break;
                }
            }
            //Iterate all its neighbour
            foreach (KeyValuePair<GameObject, float> kvp in start.GetComponent<PathNode>().neighbour)
            {
                switch (heuristic_type)
                {
                    case HeuristicType.Dijkstra:
                        kvp.Key.GetComponent<PathNode>().self.heuristic = 0;
                        break;
                    case HeuristicType.Euclidean:
                        AlgorithmEDH(kvp.Key, end);
                        break;
                }
                float temp_total_cost = start.GetComponent<PathNode>().total_cost + kvp.Value + kvp.Key.GetComponent<PathNode>().self.heuristic_vlaue;
                //If the neighbour is in the close list
                if (close.Contains(kvp.Key))
                {
                    //If the total cost is smaller than the previous
                    //then do-->
                    if (close.Find(x => x == kvp.Key).GetComponent<PathNode>().total_cost > temp_total_cost)
                    {
                        close.Remove(kvp.Key);
                        open.Add(kvp.Key);
                        kvp.Key.GetComponent<PathNode>().total_cost = temp_total_cost;
                        kvp.Key.GetComponent<PathNode>().UpdatePath(start.GetComponent<PathNode>().self.path);
                    }
                }
                //If the neighbour has already been in the open list
                else if (open.Contains(kvp.Key))
                {
                    //Check if the total cost is smaller than the previous one
                    //If yes, do-->
                    if (open.Find(x => x == kvp.Key).GetComponent<PathNode>().total_cost > temp_total_cost)
                    {
                        kvp.Key.GetComponent<PathNode>().total_cost = temp_total_cost;
                        kvp.Key.GetComponent<PathNode>().UpdatePath(start.GetComponent<PathNode>().self.path);
                    }
                }
                //If the neighbour has not been added to the open list
                //Add it to the open list
                else
                {
                    open.Add(kvp.Key);
                    kvp.Key.GetComponent<PathNode>().total_cost = temp_total_cost;
                    kvp.Key.GetComponent<PathNode>().UpdatePath(start.GetComponent<PathNode>().self.path);
                }
            }
            //make sure the shortest node comes first
            open = open.OrderBy(x => x.GetComponent<PathNode>().total_cost).ToList();
            StartAlgorithmA(open[0], end, close, open);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
