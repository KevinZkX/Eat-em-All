using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Monsters {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target)
        {

        }
		
	}

    //void Pursue()
    //{
    //    Vector3 direction;
    //    if (Vector3.Distance(target.transform.position, transform.position) < 10f)
    //    {
    //        direction = (target.transform.position - transform.position).normalized;
    //    }
    //    else
    //    {
    //        fack_target = (target.transform.position + target.transform.forward) * 1.2f;
    //        direction = (fack_target - transform.position).normalized;
    //    }
    //    acc = direction * max_acc;
    //}

    //void Align()
    //{
    //    Quaternion look_where_going = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, look_where_going, 50f);
    //}

    //void MoveTarget()
    //{
    //    //Debug.Log(acc.x + " " + acc.y + " " + acc.z);

    //    AvoidObstacles();
    //    rigidbody.AddForce(acc, ForceMode.Acceleration);
    //    if (wolfState == WolfState.Flank)
    //    {
    //        if (rigidbody.velocity.magnitude > max_flank_vel)
    //        {
    //            rigidbody.velocity = rigidbody.velocity.normalized * max_flank_vel;
    //        }
    //    }
    //    else
    //    {
    //        if (rigidbody.velocity.magnitude > max_vel)
    //        {
    //            rigidbody.velocity = rigidbody.velocity.normalized * max_vel;
    //        }
    //    }
    //    //transform.forward = acc.normalized;
    //    Align();
    //}

    //void AvoidObstacles()
    //{
    //    RaycastHit rightHit;
    //    RaycastHit leftHit;
    //    RaycastHit frontHit;
    //    Debug.DrawRay(transform.position, ((transform.forward + transform.right) * 0.25f) * 7, Color.red);
    //    Debug.DrawRay(transform.position, ((transform.forward - transform.right) * 0.25f) * 7, Color.red);
    //    Debug.DrawRay(transform.position, transform.forward * 3, Color.red);

    //    //if (Physics.Raycast(transform.position, (transform.forward + transform.right) / 4, out leftHit, 5) &&
    //    //    Physics.Raycast(transform.position, (transform.forward - transform.right) / 4, out rightHit, 5))
    //    //{
    //    //    if (leftHit.collider.tag == "Rock")
    //    //    {
    //    //        //fack_target = (leftHit.normal * 3 + leftHit.point);
    //    //        Vector3 truning_acc = Quaternion.Euler(0, 110, 0) * leftHit.normal;
    //    //        acc = (acc.normalized + truning_acc.normalized) * max_acc;
    //    //    }

    //    //}

    //    if (Physics.Raycast(transform.position, (transform.forward + transform.right) / 4, out leftHit, 7))
    //    {
    //        if (leftHit.collider.tag == "Rock" || leftHit.collider.tag == "Wolf")
    //        {
    //            if (leftHit.collider.name.Contains("Boundary") && wolfState == WolfState.Flank)
    //            {
    //                acc = (sesame.transform.position - transform.position).normalized * max_acc;
    //                return;
    //            }
    //            //fack_target = (leftHit.normal * 3 + leftHit.point);
    //            Vector3 truning_acc = Quaternion.Euler(0, 45, 0) * leftHit.normal;
    //            acc = (acc.normalized + truning_acc.normalized) * max_acc;
    //        }

    //    }
    //    else if (Physics.Raycast(transform.position, (transform.forward - transform.right) / 4, out rightHit, 7))
    //    {
    //        if (rightHit.collider.tag == "Rock" || rightHit.collider.tag == "Wolf")
    //        {
    //            if (rightHit.collider.name.Contains("Boundary") && wolfState == WolfState.Flank)
    //            {
    //                acc = (sesame.transform.position - transform.position).normalized * max_acc;
    //                return;
    //            }
    //            //fack_target = (rightHit.normal * 3 + rightHit.point);
    //            Vector3 truning_acc = Quaternion.Euler(0, -45, 0) * rightHit.normal;
    //            acc = (acc.normalized + truning_acc.normalized) * max_acc;
    //        }
    //    }

    //    else if (Physics.Raycast(transform.position, transform.forward, out frontHit, 3))
    //    {
    //        if (frontHit.collider.tag == "Rock" || frontHit.collider.tag == "Wolf")
    //        {
    //            if (frontHit.collider.name.Contains("Boundary") && wolfState == WolfState.Flank)
    //            {
    //                acc = (sesame.transform.position - transform.position).normalized * max_acc;
    //                return;
    //            }
    //            fack_target = (frontHit.normal * 3 + frontHit.point);
    //            //Vector3 truning_acc = Quaternion.Euler(0, 90, 0) * frontHit.normal;
    //            //acc = (acc.normalized + truning_acc.normalized) * max_acc;
    //        }
    //    }

    //}
}
