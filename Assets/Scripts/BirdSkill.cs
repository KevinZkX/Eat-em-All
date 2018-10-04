using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSkill : Skills {

    public float mass = 0.1f;

    public override void Skill()
    {
        attatched_rigidbody.mass = mass;
    }

}
