using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour {
    int number_of_carrot;
    public GameObject fan;
    public GameObject path;
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MovingCarrot")
        {
            number_of_carrot++;
        }
    }

    void Start ()
    {
        path.SetActive(false);
    }

    void Update ()
    {
        if (number_of_carrot == 3)
        {
            fan.SetActive(false);
            path.SetActive(true);
        }
    }

}
