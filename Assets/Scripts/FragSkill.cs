using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragSkill : Skills {

    public override void Skill()
    {
        GetComponent<PlayerController>().canJump = true;
    }
}
