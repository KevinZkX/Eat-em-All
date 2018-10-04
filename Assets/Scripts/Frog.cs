using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum Frog_State { flee, wander, goingToLeaf, standBy };

public class Frog : Monsters {

    Frog_State frogState = Frog_State.wander;
    GameObject[] non_moving_leaves;
    public List<GameObject> fleeing_leaves;
    float wanderTimer;
    float standbyTimer;
    public bool inWater;
    public bool onLeaf;
    bool onMovingPlatform;
    GameObject lastPosition;
    public bool catHere;
    public GameObject[] frogs;

    public GameObject currentLeaf;


    void Start () {
        non_moving_leaves = GameObject.FindGameObjectsWithTag("Leaf");
        frogs = GameObject.FindGameObjectsWithTag("Frog");
        lastPosition = new GameObject();
        inWater = true;
        onMovingPlatform = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        frogs = GameObject.FindGameObjectsWithTag("Frog");
        foreach (GameObject g in frogs)
        {
            if (g != gameObject)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), g.GetComponent<Collider>());
            }
        }

        if (inWater)
        {
            foreach (GameObject go in fleeing_leaves)
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }


        GameObject temp = findClosestMovingPlatform();
        if(temp != null && temp.transform.position == findClosestMovingPlatform().transform.position)
        {
            findClosestMovingPlatform().transform.hasChanged = true;
        }
        else if (temp != null && temp.transform.position != findClosestMovingPlatform().transform.position)
            findClosestMovingPlatform().transform.hasChanged = false;

        wanderTimer += Time.deltaTime;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
        foreach (GameObject g in PathGenerator.path_nodes["FrogNode"])
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), g.GetComponent<Collider>());
        }

        if(wanderTimer > 10 && !onLeaf && !onMovingPlatform)
        {
            frogState = Frog_State.goingToLeaf;
        }
        FiniteStateMechine();

        if (onLeaf)
        {
            foreach (GameObject go in fleeing_leaves)
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }

            if (!GetComponent<FieldOfView>().visibleTargets.Contains(GameObject.Find("Sesame").transform))
            {
                transform.parent = null;
                GetComponent<FieldOfView>().viewAngle = 66;
                transform.position = Vector3.Slerp(transform.position, new Vector3(findClosestLeaf().transform.position.x, transform.position.y, findClosestLeaf().transform.position.z), 0.1f);
                frogState = Frog_State.standBy;
                standbyTimer += Time.deltaTime;
            }
            if (standbyTimer > 10)
            {
                jump();
                standbyTimer = 0;
                wanderTimer = 0;
            }
            if(GetComponent<FieldOfView>().visibleTargets.Contains(GameObject.Find("Sesame").transform))
            {
                standbyTimer = 0;
                frogState = Frog_State.flee;
            }
        }

        if (wanderTimer < 10)
        {
            frogState = Frog_State.wander;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.name.Contains("Ground"))
        {
            inWater = true;
            onLeaf = false;
        }
        if (col.collider.tag == "Leaf" && !inWater)
        {
            transform.parent = null;
            currentLeaf = col.gameObject;
            onLeaf = true;
        }

        if (col.collider.tag == "MovingPlatform")
        {
            transform.parent = col.transform;
            onMovingPlatform = true;
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "MovingPlatform")
        {
            foreach (Collider co in col.gameObject.GetComponent<Leaves>().collidingLeaves)
            {
                if (co.gameObject.tag == "Leaf" && Vector3.Distance(co.gameObject.transform.position, currentLeaf.transform.position) > 5)
                {
                    transform.position = co.gameObject.transform.position;
                } 
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.collider.name.Contains("Ground"))
        {
            inWater = false;
        }
        if (col.collider.tag == "Leaf")
        {
            onLeaf = false;
        }
        if (col.collider.tag == "MovingPlatform")
        {
            transform.parent = null;
            onMovingPlatform = false;
        }
    }

    void jump()
    {
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 500, 0));
        //GetComponent<Rigidbody>().velocity = Vector3.up * 1000;
    }

    void flee()
    {
        GetComponent<FieldOfView>().viewAngle = 360;

        foreach (Transform t in GetComponent<FieldOfView>().visibleTargets)
        {
            if (t.gameObject.tag == "MovingPlatform" && t.childCount == 0)
            {
                if(t.GetComponent<Leaves>().collidingLeaves.Contains(gameObject.GetComponent<Collider>()))
                {
                    transform.position = Vector3.Slerp(transform.position, new Vector3(t.position.x, transform.position.y, t.position.z), 10.5f);
                    GetComponent<FieldOfView>().viewAngle = 180;
                }
            }        
        }
    }

    void standBy()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Inverse(transform.rotation), 0.2f);
        transform.Rotate(0, 120 * Time.deltaTime, 0);
    }

    void goToClosestLeaf()
    {
        max_speed = 2;
        if (!m_path.Contains(findClosestLeaf()))
        {
            m_path.Clear();
            path_index = 0;
            start_node = null;
            end_node = null;
            m_path.Add(findClosestLeaf());
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && inWater)
        {
            if (hit.collider.tag == "Leaf")
            {
                jump();
                //m_path.Clear();
            }
        }

        if(Vector3.Distance(transform.position, findClosestLeaf().transform.position) < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;


        }
        
    }

    GameObject findClosestLeaf()
    {
        float dis = 10000;
        GameObject closestLeaf = null;
        foreach (GameObject go in non_moving_leaves)
        {
            Vector3 direction = (go.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(go.transform.position, transform.position);

            if (Vector3.Distance(go.transform.position, transform.position) < dis)
            {
                closestLeaf = go;
                dis = Vector3.Distance(go.transform.position, transform.position);
            }

        }
        return closestLeaf;
    }

    GameObject findLeafToFlee()
    {
        float dis = 10000;
        float max = 1;
        GameObject closestLeaf = null;
        foreach (GameObject go in non_moving_leaves)
        {
            Vector3 direction = (go.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(go.transform.position, transform.position);

            if (GetComponent<FieldOfView>().visibleTargets.Contains(go.transform) && Vector3.Distance(go.transform.position, transform.position) < dis && Vector3.Distance(go.transform.position, GameObject.Find("Sesame").transform.position) > max)
            {
                closestLeaf = go;
                dis = Vector3.Distance(go.transform.position, transform.position);
                max = Vector3.Distance(go.transform.position, GameObject.Find("Sesame").transform.position);
            }

        }
        return closestLeaf;
    }

    GameObject findClosestMovingPlatform()
    {
        float dis = 10000;
        GameObject closestLeaf = null;
        foreach (GameObject go in fleeing_leaves)
        {
            Vector3 direction = (go.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(go.transform.position, transform.position);

            if (GetComponent<FieldOfView>().visibleTargets.Contains(go.transform) && Vector3.Distance(go.transform.position, transform.position) < dis)
            {
                closestLeaf = go;
                dis = Vector3.Distance(go.transform.position, transform.position);
            }

        }
        return closestLeaf;

    }

    GameObject findClosestNode()
    {
        float dis = 100000;

        GameObject closestNode = null;
        foreach (GameObject go in PathGenerator.path_nodes["FrogNode"])
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

    void wander()
    {
        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.forward, out hit, 2) ||
            Physics.Raycast(transform.position, transform.forward + transform.right / 2, out hit, 2) ||
            Physics.Raycast(transform.position, transform.forward - transform.right / 2, out hit, 2) ||
            Physics.Raycast(transform.position, transform.forward + transform.right * 3 / 4, out hit, 2) ||
            Physics.Raycast(transform.position, transform.forward - transform.right * 3 / 4, out hit, 2)
            )
        {
            if (hit.collider.gameObject.tag == "MovingPlatform")
            {
                max_speed = 0;
            }
        }
        else
            max_speed = 2;
        if (start_node == null && end_node == null)
        {
            int random = Random.Range(0, PathGenerator.path_nodes["FrogNode"].Count);
            start_node = findClosestNode();
            end_node = PathGenerator.path_nodes["FrogNode"][random];
            m_path = PathGenerator.AlgorithmA(start_node, end_node);
        }
    }
    void FiniteStateMechine()
    {      
        switch (frogState)
        {
            case Frog_State.wander:
                wander();
                break;
            case Frog_State.goingToLeaf:
                goToClosestLeaf();
                break;
            case Frog_State.flee:
                flee();
                break;
            case Frog_State.standBy:
                standBy();
                break;
            default:
                break;
        }
        Move();
    }
}
