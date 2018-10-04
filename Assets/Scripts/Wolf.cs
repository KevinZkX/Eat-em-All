using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum WolfState { Wander, Pursue, Flank, Flee}

public class Wolf : Monsters {

    public WolfState wolfState;
    bool change_position = true;
    public float max_acc;
    public float max_vel;
    public float max_flank_acc;
    public float max_flank_vel;
    public float p_distance_max;
    public float p_distance_min;
    Vector3 acc;
    Vector3 fack_target;
    float slowDownDistance = 2f;
    public float range;
    GameObject[] wolfs;
    Collider[] wolves_in_range;
    Collider[] sound_detection_range;
    static GameObject[] rocks;
    static bool foundSesami;
    static GameObject chaser;
    public static float soundDetection;
    byte flashing = 255;
    bool gotSpray;
    float vulnerableTimer;
    public Image healthBar;
    AudioSource wolverin_ad;
    AudioSource roar_ad;
    AudioSource attack_ad;
    AudioSource die_ad;
	// Use this for initialization
	void Start () {
        health_point = 5;
        soundDetection = 0;
        wolfs = GameObject.FindGameObjectsWithTag("Wolf");
        //foreach (GameObject w in wolfs)
        //{
        //    Physics.IgnoreCollision(w.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        //}
        Init();
        rocks = GameObject.FindGameObjectsWithTag("Rock");
        healthBar = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        AudioSource[] list_ad = this.GetComponents<AudioSource>();
        wolverin_ad = list_ad[0];
        roar_ad = list_ad[1];
        attack_ad = list_ad[2];
        die_ad = list_ad[3];

    }
	
	// Update is called once per frame
	void Update () {

        if (transform.GetChild(0).GetComponent<Renderer>().isVisible)
        {
            healthBar.transform.parent.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));
            healthBar.transform.localScale = (new Vector3(health_point / 5, 0.9f, 0));
        }
        transform.localPosition = new Vector3(transform.localPosition.x, 0.1f, transform.localPosition.z);
        FindTarget();
        //CheckSeparation();
        WolfStateMachine();
        MoveTarget();
        SoundDetection();
        if (gotSpray)
        {
            vulnerableTimer += Time.deltaTime;
            if (vulnerableTimer > 8)
            {
                gotSpray = false;
                wolfState = WolfState.Wander;
            }
        }

        if (health_point <= 0)
        {
            if (GameObject.Find("Boundary1 (1)") != null)
            {
                GameObject.Find("Boundary1 (1)").SetActive(false);
            }
            die_ad.Play();
            Destroy(gameObject);
        }
	}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere (transform.position, range);
 }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == sesame && wolfState != WolfState.Flee)
        {
            sesame.GetComponent<SesameHealth>().decrementHealth(3);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.name.Contains("Smoke_Explosion_Loop_02"))
        {
            gotSpray = true;
            vulnerableTimer = 0;
            wolfState = WolfState.Flee;
        }
    }

    void SoundDetection()
    {
        //IF SESAME NOT IN FOW AND IS IN DETECTION RANGE, CHECK IF ITS MOVING
        if (sound_detection_range.Contains(sesame.GetComponent<Collider>()) && !fow.visibleTargets.Contains(sesame.transform))
        {
            if (PlayerController.isRunning)
            {
                soundDetection += 0.05f;
            }
            else if (PlayerController.isWalking){
                soundDetection += 0.02f;
            }
        }

        if (soundDetection > 0 && soundDetection < 20)
        {
            soundDetection -= 0.01f;
        }
        if (soundDetection > 20)
        {
            fack_target = sesame.transform.position;
            max_vel = 6;
            soundDetection = 20.01f;
            sesame.GetComponent<PlayerController>().detection_bar.GetComponent<Animator>().SetBool("Triggered", true);
        }
    }

    void FindTarget ()
    {
        sound_detection_range = Physics.OverlapSphere(transform.position, range);
        wolves_in_range = Physics.OverlapSphere(transform.position, 1);
        if (chaser == null && fow.visibleTargets.Contains(sesame.transform))
        {
            soundDetection = 20.1f;
            chaser = this.gameObject;
            target = sesame;
            wolfState = WolfState.Pursue;
        }

        else if (chaser != gameObject && chaser != null)
        {
            wolfState = WolfState.Flank;
        }

        else if (gameObject == chaser && !fow.visibleTargets.Contains(sesame.transform))
        {
            chaser = null;
            wolfState = WolfState.Wander;
        }

        else if (chaser == null && !fow.visibleTargets.Contains(sesame.transform))
        {
            wolfState = WolfState.Wander;
        }
    }

    void Pursue ()
    {
        attack_ad.Play();
        Vector3 direction;
        if (Vector3.Distance(target.transform.position, transform.position) < 10f)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
        else
        {
            fack_target = (target.transform.position + target.transform.forward) * 1.2f;
            direction = (fack_target - transform.position).normalized;
        }
        acc = direction * max_acc;
    }

    void Flank()
    {
        wolverin_ad.Play();
        Vector3 direction_to_sesame = sesame.transform.position - transform.position;
        Vector3 sesame_moving_direction = new Vector3(sesame.GetComponent<PlayerController>().horizontal, 0, sesame.GetComponent<PlayerController>().vertical);
        Debug.Log(sesame_moving_direction);
        Vector3 moving_direcion = new Vector3();
        float distance = Vector3.Distance(sesame.transform.position, transform.position);
        float angle = Vector3.Angle(sesame_moving_direction, direction_to_sesame);
        if (angle < 160)
        {
            if (distance < p_distance_max && distance > p_distance_min)
            { 
                Debug.Log("going parallel");
                moving_direcion = sesame_moving_direction.normalized;
            }
            else if (distance > p_distance_max)
            {
                Debug.Log("getting into range");
                moving_direcion = (direction_to_sesame + sesame_moving_direction).normalized;
            }
            else if (distance < p_distance_min)
            {
                moving_direcion = direction_to_sesame;
            }
        }
        else if (angle >= 160)
        {
            moving_direcion = (direction_to_sesame + sesame_moving_direction).normalized;
        }
        acc = moving_direcion * max_flank_acc;
    }

    void CheckSeparation()
    {
        if(wolfState == WolfState.Flank)
        {
            foreach (Collider col in wolves_in_range)
            {
                if(col.gameObject.tag == "Wolf" && col.gameObject != this.gameObject && chaser != null)
                {
                    rigidbody.velocity = (transform.position - col.gameObject.transform.position).normalized * max_vel;
                    acc = (transform.position - col.gameObject.transform.position).normalized * max_acc;
                }
            }
        }
    }

    void Wander()
    {
        //transform.GetChild(0).GetComponent<Animator>().SetBool("isFleeing", false);
        roar_ad.Play();
        float z_bound_pos = 45.5f;
        float z_bound_neg = -24.5f;
        float x_bound_pos = 54.5f;
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

    void Flee()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("isFleeing", true);
        max_vel = 3;
        Vector3 direction = (transform.position - target.transform.position).normalized;
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
            if (wolfState == WolfState.Flee)
            {
                direciton = (transform.position - target.transform.position).normalized;
            }
            else if (wolfState == WolfState.Pursue)
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
        if (wolfState == WolfState.Flank)
        {
            if (rigidbody.velocity.magnitude > max_flank_vel)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * max_flank_vel;
            }
        }
        else
        {
            if (rigidbody.velocity.magnitude > max_vel)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * max_vel;
            }
        }
        //transform.forward = acc.normalized;
        Align();
    }

    void AvoidObstacles()
    {
        RaycastHit rightHit;
        RaycastHit leftHit;
        RaycastHit frontHit;
        Debug.DrawRay(transform.position, ((transform.forward + transform.right) * 0.25f) * 7, Color.red);
        Debug.DrawRay(transform.position, ((transform.forward - transform.right) * 0.25f) * 7, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red);

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
            if (leftHit.collider.tag == "Rock" || leftHit.collider.tag == "Wolf")
            {
                if (leftHit.collider.name.Contains("Boundary") && wolfState == WolfState.Flank)
                {
                    acc = (sesame.transform.position - transform.position).normalized * max_acc;
                    return;
                }
                //fack_target = (leftHit.normal * 3 + leftHit.point);
                Vector3 truning_acc = Quaternion.Euler(0, 45, 0) * leftHit.normal;
                acc = (acc.normalized + truning_acc.normalized) * max_acc;
            }

        }
        else if (Physics.Raycast(transform.position, (transform.forward - transform.right) / 4, out rightHit, 7))
        {
            if (rightHit.collider.tag == "Rock" || rightHit.collider.tag == "Wolf")
            {
                if (rightHit.collider.name.Contains("Boundary") && wolfState == WolfState.Flank)
                {
                    acc = (sesame.transform.position - transform.position).normalized * max_acc;
                    return;
                }
                //fack_target = (rightHit.normal * 3 + rightHit.point);
                Vector3 truning_acc = Quaternion.Euler(0, -45, 0) * rightHit.normal;
                acc = (acc.normalized + truning_acc.normalized) * max_acc;
            }
        }

        else if (Physics.Raycast(transform.position, transform.forward, out frontHit, 3))
        {
            if (frontHit.collider.tag == "Rock" || frontHit.collider.tag == "Wolf")
            {
                if (frontHit.collider.name.Contains("Boundary") && wolfState == WolfState.Flank)
                {
                    acc = (sesame.transform.position - transform.position).normalized * max_acc;
                    return;
                }
                fack_target = (frontHit.normal * 3 + frontHit.point);
                //Vector3 truning_acc = Quaternion.Euler(0, 90, 0) * frontHit.normal;
                //acc = (acc.normalized + truning_acc.normalized) * max_acc;
            }
        }

    }

    void WolfStateMachine()
    {
        switch (wolfState)
        {
            case WolfState.Wander:
                //Debug.Log("Wander");
                Wander();
                break;
            case WolfState.Pursue:
                //Debug.Log("Pursue");
                Pursue();
                break;
            case WolfState.Flank:
                //Debug.Log("Flank");
                Flank();
                break;
            case WolfState.Flee:
                Flee();
                break;
            default:
                break;
        }
    }
}
