using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkunkSkill : Skills {
    public ParticleSystem gas;
    bool coolDown;
    float timer = 0f;

    void Update ()
    {
        if (timer > 5f)
        {
            coolDown = true;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public override void Skill()
    {
        if (coolDown)
        {
            ParticleSystem temp = Instantiate(gas, transform.position, Quaternion.identity);
            Destroy(temp, 10);
            coolDown = false;
            timer = 0;
        }
        
    }
}
