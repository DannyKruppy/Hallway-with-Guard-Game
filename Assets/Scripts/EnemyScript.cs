using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using TMPro;

public class EnemyScript : MonoBehaviour
{

    public List<Transform> waypoints;
    public Transform player;
    public TextMeshProUGUI losText;

    public Transform raySender;
    public Transform rayReceiver;

    public int aggroStoppingDistance = 10;

    public bool playerInCone = false;

    private NavMeshAgent agent;
    private int index = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextWaypoint();
    }

    void Update()
    {
        

        if (playerInCone)
        {
            Vector3 direction = rayReceiver.position - raySender.position;
            RaycastHit hit;

            if (Physics.Raycast(raySender.position, direction.normalized, out hit, direction.magnitude) && hit.collider.CompareTag("Player"))
            {
                losText.text = "In line of sight";
                agent.destination = player.position;
                agent.stoppingDistance = aggroStoppingDistance;

                Vector3 lookDir = player.position - transform.position;
                lookDir.y = 0;
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
            else
            {
                losText.text = "";

                if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                {
                    NextWaypoint();
                }
            }
        }
        else
        {
            losText.text = "";

            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                NextWaypoint();
            }
        }
    }

    void NextWaypoint()
    {
        agent.destination = waypoints[index].position;
        index = (index + 1) % waypoints.Count;
    }
}
