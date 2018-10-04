using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaves : MonoBehaviour {

    public Collider[] collidingLeaves;
    public float radius;
    public GameObject frog;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        collidingLeaves = Physics.OverlapSphere(transform.position, radius);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.name.Contains("Frog"))
        {
            frog = col.gameObject;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.name.Contains("Frog"))
        {
            frog = null;
        }
    }
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, radius);
    //}
}
