using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
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

    public float searchRadius = 6f;
    public float searchDuration = 5f;

    private bool searching = false;
    private bool hadLineOfSight = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextWaypoint();
    }

    void Update()
    {
        Vector3 direction = rayReceiver.position - raySender.position;
        RaycastHit hit;

        if (Physics.Raycast(raySender.position, direction.normalized, out hit, direction.magnitude) && hit.collider.CompareTag("Player") && playerInCone)
        {
            losText.text = "In line of sight";
            hadLineOfSight = true;

            agent.destination = player.position;
            agent.stoppingDistance = aggroStoppingDistance;

            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
        else
        {
            losText.text = "";

            if (hadLineOfSight && !searching)
            {
                StartCoroutine(SearchForPlayer());
            }
        }
    }

    IEnumerator SearchForPlayer()
    {
        searching = true;
        float timer = 0f;
        agent.stoppingDistance = 1;

        while (timer < searchDuration)
        {
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection += transform.position;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }

            yield return new WaitForSeconds(3f);

            timer += 3f;
        }

        searching = false;
        hadLineOfSight = false;

        NextWaypoint();
    }

    void NextWaypoint()
    {
        agent.destination = waypoints[index].position;
        index = (index + 1) % waypoints.Count;
    }
}
