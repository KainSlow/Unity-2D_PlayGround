using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Behavior/Stay In Radius")]
public class StayInRadiusBehavior : BoidBehavior
{
    [SerializeField] Vector2 center = Vector2.zero;
    [SerializeField] float radius = 15f;
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> contexts, Boid boid, Transform target)
    {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.9f)
        {
            return Vector2.zero;
        }

        return t * t * centerOffset;
    }
}
