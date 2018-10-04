using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SkunkState { wander, flee, attack, gas};

public class Skunk : Monsters {

    private SkunkState skunkState;
    bool change_position = true;
    public float max_acc;
    public float max_vel;
    Vector3 acc;
    Vector3 fack_target;
    float slowDownDistance = 2f;
    public Collider[] spray_range;
    public AudioSource fart;
    public ParticleSystem gas;
    float gasTimer;
    int gasMultiplier = 10;
    public static GameObject[] rocks;
    GameObject[] skunks;

    bool allyInTrouble = false;

    void Start()
    {
        skunks = GameObject.FindGameObjectsWithTag("Skunk");
        Init();
        rocks = GameObject.FindGameObjectsWithTag("Rock");
        //foreach (GameObject go in rocks)
        //{
        //    Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>());
        //}
    }
    private void Update()
    {
        FindTarget();
        CheckScales();
        SkunkStateMachine();
        CheckFlocking();
        MoveTarget();
    }

    void OnDestroy()
    {
        rocks = GameObject.FindGameObjectsWithTag("Rock");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == sesame)
        {
            if (skunkState == SkunkState.flee || skunkState == SkunkState.wander)
            {
                Destroy(gameObject);
            }
            else if (skunkState == SkunkState.attack)
            {
                Destroy(sesame);
            }
        }
    }

    void CheckFlocking()
    {

        if (skunkState == SkunkState.flee)
        {
            foreach (GameObject go in skunks)
            {
                if (go != null && go != this.gameObject)
                {
                    if (spray_range.Contains(go.GetComponent<Collider>()))
                    {
                        Debug.Log("alignment");
                        go.GetComponent<Skunk>().allyInTrouble = true;
                        //go.GetComponent<Skunk>().rigidbody.velocity = rigidbody.velocity;
                        //go.GetComponent<Skunk>().acc = go.GetComponent<Skunk>().rigidbody.velocity.normalized * max_acc;
                    }
                    else
                        go.GetComponent<Skunk>().allyInTrouble = false;
                }
            }
        }
        else if (skunkState == SkunkState.wander)
        {
            //foreach (GameObject go in skunks)
            //{
            //    if (go != null && go != this.gameObject)
            //    {
            //        if (spray_range.Contains(go.GetComponent<Collider>()))
            //        {
            //            go.GetComponent<Skunk>().allyInTrouble = false;
            //            Debug.Log("separate");
            //            rigidbody.velocity = (transform.position - go.transform.position).normalized * max_vel;
            //            acc = (transform.position - go.transform.position).normalized * max_acc;
            //            fack_target = new Vector3(Random.Range(8.5f, 48.5f), 0.5f, Random.Range(1.5f, 45.5f));
            //        }
            //    }
            //}
            foreach (Collider col in spray_range)
            {
                if (col.gameObject != this.gameObject && col.gameObject.tag == "Skunk")
                {
                    //Debug.Log("separate");
                    rigidbody.velocity = (transform.position - col.gameObject.transform.position).normalized * max_vel;
                    acc = (transform.position - col.gameObject.transform.position).normalized * max_acc;
                    fack_target = new Vector3(Random.Range(8.5f, 48.5f), 0.5f, Random.Range(1.5f, 45.5f));
                }
            }
        }
    }

    void FindTarget()
    {
        gasTimer += gasMultiplier * Time.deltaTime;
        spray_range = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Player"));

        if (fow.visibleTargets.Contains(sesame.transform))
        {
            //float random_x = Random.Range(8.5f, 48.5f);
            //float random_z = Random.Range(1.5f, 45.5f);
            //fack_target = new Vector3(random_x, 0.5f, random_z);
            target = sesame;

            gasMultiplier = 1;
            fow.viewAngle = 360;
            if (gasTimer > 10)
            {
                ParticleSystem gas_prefab;
                gas_prefab = Instantiate(gas, transform.position, Quaternion.identity);
                Destroy(gas_prefab, 10);
                gasTimer = 0;
            }
            //fart.Play();
            skunkState = SkunkState.gas;
            
        }
        else
        {
            target = null;
            skunkState = SkunkState.wander;
        }
    }

    void CheckScales()
    {
        if (target)
        {
            if (target.GetComponent<Character>().scales.magnitude < scales.magnitude)
            {
                skunkState = SkunkState.attack;
            }
            else if (target.GetComponent<Character>().scales.magnitude > scales.magnitude || allyInTrouble)
            {
                skunkState = SkunkState.flee;
            }
        }
    }

    void Spray()
    {
        Debug.Log("spraying");
        gas.Play();
    }

    //need to change to mag of acc
    void Flee()
    {
        Vector3 direction = (transform.position - target.transform.position).normalized;
        acc = direction * max_acc;
        //Debug.Log("target: " + fack_target);
    }

    void Wander()
    {
        float z_bound_pos = 45.5f;
        float z_bound_neg = 1.5f;
        float x_bound_pos = 48.5f;
        float x_bound_neg = 8.5f;
        float y = 0.5f;
        if (change_position)
        {
            float random_x = Random.Range(x_bound_neg, x_bound_pos);
            float random_z = Random.Range(z_bound_neg, z_bound_pos);

            fack_target = new Vector3(random_x, y, random_z);
            foreach (GameObject go in rocks)
            {
                while (go.GetComponent<Collider>().bounds.Contains(fack_target))
                {
                    random_x = Random.Range(x_bound_neg, x_bound_pos);
                    random_z = Random.Range(z_bound_neg, z_bound_pos);

                    fack_target = new Vector3(random_x, y, random_z);
                }
            }
            change_position = false;
        }
        if (Vector3.Distance(fack_target, transform.position) < 5)
        {
            change_position = true;
        }
        Vector3 direction = (fack_target - transform.position).normalized;
        acc = direction * max_acc;
        //Debug.Log("target: " + fack_target);
    }

    void Align()
    {
        Quaternion look_where_going = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look_where_going, 50f);
    }

    void Align(GameObject target)
    {
        Quaternion look_where_going;
        if (target != null)
        {
            Vector3 direciton = new Vector3();
            if (skunkState == SkunkState.flee)
            {
                direciton = (transform.position - target.transform.position).normalized;
            }
            else if(skunkState == SkunkState.attack)
            {
                direciton = (target.transform.position - transform.position).normalized;
            }
            
            look_where_going = Quaternion.LookRotation(direciton);
        }
        else
        {
            look_where_going = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, look_where_going, 100f);
    }



    void MoveTarget()
    {
        //Debug.Log(acc.x + " " + acc.y + " " + acc.z);
       
        AvoidObstacles();
        rigidbody.AddForce(acc, ForceMode.Acceleration);
        if (rigidbody.velocity.magnitude > max_vel)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * max_vel;
        }

        //transform.forward = acc.normalized;
        Align();
    }

    void AvoidObstacles()
    {
        RaycastHit rightHit;
        RaycastHit leftHit;
        RaycastHit frontHit;
        //Debug.DrawRay(transform.position, ((transform.forward + transform.right) * 0.75f) * 10, Color.red);
        //Debug.DrawRay(transform.position, ((transform.forward - transform.right) * 0.75f) * 10, Color.red);
        //Debug.DrawRay(transform.position, transform.forward * 9, Color.red);

        //if (Physics.Raycast(transform.position, (transform.forward + transform.right) / 4, out leftHit, 5) &&
        //    Physics.Raycast(transform.position, (transform.forward - transform.right) / 4, out rightHit, 5))
        //{
        //    if (leftHit.collider.tag == "Rock")
        //    {
        //        //fack_target = (leftHit.normal * 3 + leftHit.point);
        //        Vector3 truning_acc = Quaternion.Euler(0, 110, 0) * leftHit.normal;
        //        acc = (acc.normalized + truning_acc.normalized) * max_acc;
        //    }

        //}

        if (Physics.Raycast(transform.position, (transform.forward + transform.right) / 4, out leftHit, 7))
        {
            if (leftHit.collider.tag == "Rock")
            {
                //fack_target = (leftHit.normal * 3 + leftHit.point);
                Vector3 truning_acc = Quaternion.Euler(0, 45, 0) * leftHit.normal;
                acc = (acc.normalized + truning_acc.normalized) * max_acc;
            }
            
        }
        else if (Physics.Raycast(transform.position, (transform.forward - transform.right) / 4, out rightHit, 7))
        {
            if (rightHit.collider.tag == "Rock")
            {
                //fack_target = (rightHit.normal * 3 + rightHit.point);
                Vector3 truning_acc = Quaternion.Euler(0, -45, 0) * rightHit.normal;
                acc = (acc.normalized + truning_acc.normalized) * max_acc;
            }
        }

        else if (Physics.Raycast(transform.position, transform.forward, out frontHit, 3))
        {
            if (frontHit.collider.tag == "Rock")
            {
                fack_target = (frontHit.normal * 3 + frontHit.point);
                //Vector3 truning_acc = Quaternion.Euler(0, 90, 0) * frontHit.normal;
                //acc = (acc.normalized + truning_acc.normalized) * max_acc;
            }
        }

    }

    void SkunkStateMachine()
    {
        switch (skunkState)
        {
            case SkunkState.wander:
                //Debug.Log("Wander");
                Wander();
                break;
            case SkunkState.flee:
               // Debug.Log("Flee");
                Flee();
                break;
            case SkunkState.attack:
                break;
            case SkunkState.gas:
                Spray();
                break;
            default:
                break;
        }
    }
}
