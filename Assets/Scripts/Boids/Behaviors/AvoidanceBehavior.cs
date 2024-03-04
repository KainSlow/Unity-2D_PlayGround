using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[CreateAssetMenu(menuName = "Boid/Behavior/Avoidance")]

public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid, Transform target)
    {
        Vector2 avoidanceMove = Vector2.zero;

        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return avoidanceMove;

        int nAvoid = 0;

        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        if(filteredContext.Count == 0) return avoidanceMove;

        foreach (Transform item in filteredContext)
        {
            Vector3 closestPoint = item.gameObject.GetComponent<Collider2D>().ClosestPoint(agent.transform.position);

            if (Vector2.SqrMagnitude(closestPoint - agent.transform.position) < boid.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return avoidanceMove;

    }
}
