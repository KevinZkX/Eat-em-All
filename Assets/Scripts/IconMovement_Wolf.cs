﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMovement_Wolf : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = new Vector3(transform.parent.position.x, 10.1f, transform.parent.position.z);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
	}
}
