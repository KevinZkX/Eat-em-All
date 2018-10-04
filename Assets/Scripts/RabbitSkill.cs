using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitSkill : Skills {
    [Tooltip("Change the velocity magnitude")]
    public Vector3 jump = new Vector3(0, 10, 0);

    public override void Skill()
    {
        Debug.Log("Higher");
        GetComponent<PlayerController>().jump = jump;
    }
}
