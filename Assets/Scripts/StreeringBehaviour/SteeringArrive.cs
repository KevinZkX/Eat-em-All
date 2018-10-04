using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;
using Coroutine = System.Collections.IEnumerator;

public class SteeringArrive : SteeringBehavior 
{
    public Transform target;
    public Vector3 fixedTarget;
    public float nearRadius;

    public override float Friction
    {
        get
        {
            Vector3 destination = target == null ? fixedTarget : target.position;
            Vector3 toVector = DisplacementVector(transform.position, destination);

            float dist = toVector.magnitude;

            if (dist > nearRadius)
                return 0f;

            return -MaxAcceleration * (1f - dist / nearRadius);
        }
    }
}
