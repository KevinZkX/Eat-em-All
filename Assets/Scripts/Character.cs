using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public float health_point;
    public float attack_point;
    public float max_speed_mag;
    public GameObject target;
    public GameObject food;
    public Vector3 scales;
    public Rigidbody rigidbody;

    protected void InintaleScale()
    {
        scales = transform.localScale;
    }

}
