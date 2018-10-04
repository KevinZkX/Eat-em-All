using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TigerState { Charge, Idol, JumpAttack, Attack, Alert, KeepDistance} 

public class Tiger : Monsters {

    TigerState tigerState = TigerState.Idol;
    bool reachTarget;
    bool charging;
    bool jumpAttack;
    bool inAir;
    bool collided;
    bool finish;
    float idolTimer;
    public GameObject bear;

	// Use this for initialization
	void Start () {
        Init();
        StartCoroutine("TigerStateMachine");
	}
	
    void FindTarget()
    {
        if (fow.visibleTargets.Contains(sesame.transform))
        {
            target = sesame;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        FindTarget();
        CallBear();
        if (target)
        {
            Align();
        }
    }

    private void Update()
    {
        return;
    }

    void CallBear ()
    {
        if (target)
        {
            bear = GameObject.Find("Bear");
            //IF sesame is visible for triger--> triger call bear;
            this.GetComponent<AudioSource>().Play();
            bear.GetComponent<AudioSource>().PlayDelayed(1.5f);
            if (bear)
            {
                bear.GetComponent<Bear>().target = sesame;
            }
        }
    }

    float CheckDistance ()
    {
        float distance = 100000;
        if (target)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
        }
        return distance;
    }

    void Align()
    {
        Quaternion look_where_going = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look_where_going, 50f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Sesame" && (tigerState == TigerState.Charge || tigerState == TigerState.JumpAttack))
        {
            collided = true;
            Debug.Log("Love Sesame");
            rigidbody.velocity = Vector3.zero;
            target.GetComponent<SesameHealth>().decrementHealth(40);
            tigerState = TigerState.Idol;
        }
    }

    IEnumerator Charge()
    {
        Debug.Log("Charge");
        Vector3 direction = (target.transform.position - transform.position).normalized;
        //rigidbody.AddForce(direction * 10f, ForceMode.Impulse);
        //Align();
        rigidbody.velocity = direction * 10f;
        yield return new WaitUntil(() => CheckDistance() < 0.5f);
        finish = true;
        Debug.Log("end of charge");
    }

    IEnumerator Alert()
    {
        Debug.Log("Alert");
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rigidbody.velocity = Quaternion.Euler(0, -90, 0) * direction * max_speed_mag;
        Quaternion look_where_going = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look_where_going, 50f);
        Debug.Log("end of alert attack");
        yield return new WaitForSeconds(3);
        finish = true;
    }

    IEnumerator JumpAttack ()
    {
        Debug.Log("JumpAttack");
        if (!inAir)
        {
            rigidbody.AddForce(new Vector3(0, 100000, 0), ForceMode.Impulse);
            inAir = true;
        }
        yield return new WaitUntil(() => rigidbody.velocity.y <= 0);
        Vector3 distination = target.transform.position;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion look_where_going = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look_where_going, 50f);
        transform.position = Vector3.Slerp(transform.position, target.transform.position, 0.5f);
        yield return new WaitUntil(() => CheckDistance() < 0.5f);
        //rigidbody.AddForce(direction * 10, ForceMode.Impulse);
        Debug.Log("end of jump attack");
        finish = true;

    }

    IEnumerator Palm ()
    {
        Debug.Log("Palm");
        target.GetComponent<SesameHealth>().decrementHealth(10);
        Debug.Log("end of palm attack");
        yield return new WaitForSeconds(1.0f);
        finish = true;

    }

    IEnumerator Idol ()
    {
        Debug.Log("Idol");
        float random = Random.Range(0, 3);
        if (CheckDistance() < 3f)
        {
            collided = false;
            if (random >= 0 && random < 1)
            {
                tigerState = TigerState.Attack;
            }
            else if (random >= 1 && random < 2)
            {
                tigerState = TigerState.Idol;
            }
            else if (random >= 2 && random < 3)
            {
                tigerState = TigerState.Alert;
            }
        }
        else
        {
            if (random >= 0 && random < 1)
            {
                tigerState = TigerState.Charge;
            }
            else if (random >= 1 && random < 2)
            {
                tigerState = TigerState.Alert;
            }
            else if (random >= 2 && random < 3)
            {
                tigerState = TigerState.JumpAttack;
            }
        }
        Debug.Log("end of idol attack");
        Debug.Log("Next State: " + tigerState);
        finish = false;
        yield return null;
    }

    IEnumerator TigerStateMachine ()
    {
        while (true)
        {
            Debug.Log("State Machine");
            if (target)
            {
                switch (tigerState)
                {
                    case TigerState.Charge:
                        StartCoroutine("Charge");
                        yield return new WaitUntil(() => finish);
                        tigerState = TigerState.Idol;
                        break;
                    case TigerState.Idol:;
                        StartCoroutine("Idol");
                        yield return new WaitForEndOfFrame();
                        break;
                    case TigerState.JumpAttack:
                        StartCoroutine("JumpAttack");
                        yield return new WaitUntil(() => finish);
                        tigerState = TigerState.Idol;
                        break;
                    case TigerState.Attack:
                        StartCoroutine("Palm");
                        yield return new WaitUntil(() => finish);
                        tigerState = TigerState.Idol;
                        break;
                    case TigerState.Alert:
                        StartCoroutine("Alert");
                        yield return new WaitUntil(() => finish);
                        tigerState = TigerState.Idol;
                        break;
                    default:
                        yield return new WaitUntil(() => finish);
                        break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
