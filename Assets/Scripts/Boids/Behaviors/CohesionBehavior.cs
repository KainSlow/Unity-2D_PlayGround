using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[CreateAssetMenu(menuName = "Boid/Behavior/Cohesion")]
public class CohesionBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid, Transform target)
    {
        Vector2 cohesionMove = Vector2.zero;
        if (context.Count == 0) { return cohesionMove; }
        
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        if (filteredContext.Count == 0) { return cohesionMove; }

        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= filteredContext.Count;

        //create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }

   
}
