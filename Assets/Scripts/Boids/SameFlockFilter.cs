using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Filter/SameFlock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(BoidAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new();
        foreach (Transform item in original)
        {
            BoidAgent itemAgent = item.GetComponent<BoidAgent>();
            if (itemAgent != null && itemAgent.AgentBoid == agent.AgentBoid)
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
