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

    public float suspicion = 0f;
    public float suspicionIncreaseRate = 80f; // per second when player visible
    public float suspicionDecreaseRate = 20f; // per second when player not visible
    public float suspicionMax = 100f;
    public float instantAggroRange = 2f;
    public TextMeshProUGUI suspicionText;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextWaypoint();
    }

    void Update()
    {
        Vector3 direction = rayReceiver.position - raySender.position;
        RaycastHit hit;

        suspicion = Mathf.Clamp(suspicion, 0f, 100f);
        suspicionText.text = ("Suspicion %: " + suspicion);

        if (Physics.Raycast(raySender.position, direction.normalized, out hit, direction.magnitude) && hit.collider.CompareTag("Player") && direction.magnitude <= instantAggroRange)
        {
            suspicion = 100f;
        } 
        else if (Physics.Raycast(raySender.position, direction.normalized, out hit, direction.magnitude) && hit.collider.CompareTag("Player") && playerInCone)
        {
            suspicion += suspicionIncreaseRate * Time.deltaTime;
        }
        else
        {
            losText.text = "";
            suspicion -= suspicionDecreaseRate * Time.deltaTime;

            if (hadLineOfSight && !searching)
            {
                StartCoroutine(SearchForPlayer());
            }
        }

        if (suspicion >= suspicionMax)
        {
            losText.text = "In line of sight";
            hadLineOfSight = true;

            //set player as movement target and increase stoppin distnce
            agent.destination = player.position;
            agent.stoppingDistance = aggroStoppingDistance;

            //constantly rotate enemy towards player
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);
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
