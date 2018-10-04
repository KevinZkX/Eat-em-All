using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using Coroutine = System.Collections.IEnumerator;

public sealed class SteeringAgent : MovementBehavior
{
    public float MaxVelocity;

    public Vector3 Velocity { get; private set; }

    private SteeringBehavior[] steeringBehaviors;

    public override void Start()
    {
        base.Start();
        steeringBehaviors = GetComponents<SteeringBehavior>();
    }

    void FixedUpdate()
    {
        UpdateVelocities(Time.deltaTime);

        UpdatePosition(Time.deltaTime);
        UpdateRotation(Time.deltaTime);

        if (Velocity.sqrMagnitude == 0f && rigidbody.velocity.sqrMagnitude > 0) //to prevent sliding off the map
            transform.position = WrapPosition(transform.position);
    }

    public void ResetVelocities()
    {
        Velocity = Vector3.zero;
    }

    private void UpdateVelocities(float deltaTime)
    {
        Vector3 acceleration = Vector3.zero;
        float friction = 0f;

        foreach (SteeringBehavior behavior in steeringBehaviors)
        {
            if (!behavior.enabled)
                continue;

            acceleration += behavior.Acceleration;
            friction += behavior.Friction;
        }

        Velocity += acceleration * deltaTime;

        Velocity = Vector3.ClampMagnitude(Velocity, Math.Max(0f, MaxVelocity - friction));
    }

    private void UpdatePosition(float deltaTime)
    {
        MoveBy(Velocity * deltaTime);
    }

    private void UpdateRotation(float deltaTime)
    {
        if (Velocity.sqrMagnitude > 0f)
            transform.rotation = Quaternion.LookRotation(Velocity.normalized, Vector3.up);
    }
}
