using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMapTrigger : MonoBehaviour {

	void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            Monsters.CatHere = !Monsters.CatHere;
        }
    }
}
