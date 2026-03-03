using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{

    public List<Transform> waypoints;

    private NavMeshAgent agent;
    private int index = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextWaypoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        agent.destination = waypoints[index].position;
        index = (index + 1) % waypoints.Count;
    }
}
