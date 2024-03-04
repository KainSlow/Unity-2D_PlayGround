using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidAgent agentPrefab;

    readonly List<BoidAgent> agents = new();
    
    public BoidBehavior behavior;

    const float AGENTDENSITY = 0.08f;

    [Header("Properties")]
    [Range(2, 500), SerializeField]
    int startingCount = 20;
    
    [Range(1f, 100f), SerializeField]
    float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 7f;

    [Range(1f, 10f), SerializeField]
    float neighborRadius = 2f;

    [Range(0f, 1f), SerializeField]
    float avoidanceRadiusMultiplier = .5f;

    [SerializeField] public GameObject target;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius
    { get { return squareAvoidanceRadius; } }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i = 0; i <  startingCount; i++)
        {
            var agent = Instantiate(
                agentPrefab,
                AGENTDENSITY * startingCount * UnityEngine.Random.insideUnitCircle,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            agent.name = $"Agent ({i})";
            agent.Initialize(this);
            agents.Add( agent );
        }
    }
    private void Update()
    {
        foreach(var agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector2 move = behavior.CalculateMove(agent, context, this, target.transform);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(BoidAgent agent)
    {
        List<Transform> context = new();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach(Collider2D c in contextColliders)
        {
            if(c != agent.AgentCollider)
            context.Add(c.transform);
        }
        return context;
    }
}
