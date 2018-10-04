using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;
using Coroutine = System.Collections.IEnumerator;

public class SteeringSeek : SteeringBehavior
{
    public Transform target;
    public Vector3 fixedTarget;

    public override Vector3 Acceleration
    {
        get
        {
            Vector3 destination = target == null ? fixedTarget : target.position;
            Vector3 toVector = DisplacementVector(transform.position, destination);

            return MaxAcceleration * toVector.normalized;
        }
    }

}