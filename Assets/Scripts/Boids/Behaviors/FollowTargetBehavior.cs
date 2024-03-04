using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Behavior/Follow Target")]
public class FollowTargetBehavior : BoidBehavior
{
    [SerializeField] float maxSteerForce;
    Vector2 velocity;
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> contexts, Boid boid, Transform target)
    {
        Vector2 offSetTarget = target.transform.position - agent.transform.position;
        return Vector2.SmoothDamp(agent.transform.up, SteerTowards(offSetTarget, boid), ref velocity, maxSteerForce);
    }
    
    private Vector2 SteerTowards(Vector2 vector, Boid boid)
    {
        Vector2 v = vector.normalized * boid.maxSpeed;
        return Vector2.ClampMagnitude(v, maxSteerForce);
    }
}
