using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public GameObject door;
    AudioSource pick;
    AudioSource skill;
    void Start()
    {
        AudioSource[] list = this.GetComponents<AudioSource>();
        pick = list[0];
        skill = list[1];

    }
	public void OpenTheDoor()
    {
        skill.Play();
        Destroy(door);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.name == "Sesame")
        {
            pick.Play();
            
            Debug.Log("Open doors");
            OpenTheDoor();
            Destroy(gameObject);
        }
    }
}
