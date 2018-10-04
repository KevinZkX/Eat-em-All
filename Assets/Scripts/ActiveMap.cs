using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMap : MonoBehaviour {

    GameManager gameManger;

    void Start()
    {
        gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sesame")
        {
            Debug.Log("Trigger");
            gameManger.ActiveMaps();
            Destroy(gameObject);
        }
    }
}
