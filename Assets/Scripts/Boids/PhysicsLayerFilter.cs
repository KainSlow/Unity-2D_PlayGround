using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Filter/PhysicsLayer")]
public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask mask;
    public override List<Transform> Filter(BoidAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new();
        foreach (Transform item in original)
        {
            

            if (mask == (mask | (1 << item.gameObject.layer)))
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
