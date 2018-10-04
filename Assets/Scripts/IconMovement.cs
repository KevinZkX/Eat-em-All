using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.gameObject.transform.position = new Vector3(transform.parent.position.x, 10f, transform.parent.position.z);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(90f, transform.parent.position.y, 0f));

	}
}
