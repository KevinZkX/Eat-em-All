using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringAgent))]
public abstract class SteeringBehavior : MovementBehavior
{
    public float MaxAcceleration;

    public virtual Vector3 Acceleration
    {
        get
        {
            return Vector3.zero;
        }
    }

    public virtual float Friction
    {
        get
        {
            return 0f;
        }
    }

    public virtual bool HaltTranslation
    {
        get
        {
            return false;
        }
    }
}
