using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSkill : Skills {
    public Vector3 scale_rate = new Vector3(1f, 1f, 1f);
    Vector3 original_scale;
    bool first_trigger = true;

    public override void Skill()
    {
        if (first_trigger)
        {
            original_scale = transform.localScale;
            transform.localScale = scale_rate;
            first_trigger = false;
        }
        else
        {
            transform.localScale = original_scale;
            first_trigger = true;
        }
    }

    
}
