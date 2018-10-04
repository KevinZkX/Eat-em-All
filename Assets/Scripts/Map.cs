using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public float left_bound = -12;
    public float right_bound = 12;
    public float Upper_bound = 12;
    public float Lower_bound = -12;
    public Vector3 center;
    public MapName map_name;

    void Start()
    {
        center = transform.position;
        left_bound = -12;
        right_bound = 12;
        Upper_bound = 12;
        Lower_bound = -12;
    }

}
