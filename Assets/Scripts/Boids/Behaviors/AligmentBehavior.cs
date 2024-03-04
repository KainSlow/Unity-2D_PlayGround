using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[CreateAssetMenu(menuName = "Boid/Behavior/Aligment")]

public class AligmentBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boidm, Transform target)
    {
        Vector2 alignmentMove = Vector2.zero;
        if(context.Count == 0) { return alignmentMove; }
        
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        if(filteredContext.Count == 0) return alignmentMove;

        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= filteredContext.Count;
        //alignmentMove /= context.Count;

        return alignmentMove;
    }
}
