using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    public float maxStun = 1;
    private float stunVal = 0;

    private float defaultSpeed;

    [Header("Attack Parameters")]

    public Transform attackOrigin;
    public GameObject attackWarning;

    public float animationTime = 1f;
    public float shrinkSpeed = 0.5f;
    public float rotationSpeed = 120f;

    public string playerTag = "Player";
    public string sceneToLoad = "GameOver";

    private bool isAttacking = false;

    [Header("Audio Parameters")]

    private AudioSource audioSource;
    public AudioClip aggroClip;
    //public AudioClip hitClip;
    public AudioClip lostClip;

    bool hasPlayedAggro = false;
    bool hasPlayedLost = false;

    private float attackReady = 0;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        NextWaypoint();
        defaultSpeed = agent.speed;
    }

    void Update()
    {
        Vector3 direction = rayReceiver.position - raySender.position;
        RaycastHit hit;

        suspicion = Mathf.Clamp(suspicion, 0f, 100f);
        suspicionText.text = ("Suspicion %: " + suspicion);

        if (stunVal > 0)
        {
            agent.speed = 0;
            stunVal -= Time.deltaTime;
        } 
        else
        {
            agent.speed = defaultSpeed;
        }

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

            //play aggro audio
            if (!hasPlayedAggro)
            {
                audioSource.PlayOneShot(aggroClip);
                hasPlayedAggro = true;
            }
            
            //set player as movement target and increase stoppin distnce
            agent.destination = player.position;
            agent.stoppingDistance = aggroStoppingDistance;

            //constantly rotate enemy towards player
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);

            attackReady += Time.deltaTime;

            if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance && !isAttacking && attackReady > 1.5f)
            {
                Attack();
            }
        }
        else
        {
            //reset aggro audio check
            hasPlayedAggro = false;
            attackReady = 0f;
        }

        if (suspicion < suspicionMax && !searching && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            NextWaypoint();
        }
    }

    IEnumerator SearchForPlayer()
    {
        searching = true;
        float timer = 0f;
        agent.stoppingDistance = 1;

        audioSource.PlayOneShot(lostClip);

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

    public void Stun()
    {
        stunVal = maxStun;
        suspicion = suspicionMax;
    }

    void Attack()
    {
        GameObject indicator = Instantiate(attackWarning, attackOrigin.position, attackOrigin.rotation);
        StartCoroutine(AttackRoutine(indicator));
    }

    IEnumerator AttackRoutine(GameObject indicator)
    {
        isAttacking = true;

        float timer = 0;

        while (timer < animationTime)
        {
            if (stunVal > 0)
            {
                Destroy(indicator);
                isAttacking = false;
                yield break;
            }

            indicator.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);
            indicator.transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
            indicator.transform.position = attackOrigin.position;

            timer += Time.deltaTime;
            yield return null;
        }

        if (stunVal > 0)
        {
            Destroy(indicator);
            isAttacking = false;
            yield break;
        }

        Vector3 direction = (player.position - attackOrigin.position).normalized;
        RaycastHit attackHit;

        if (Physics.Raycast(attackOrigin.position, direction, out attackHit))
        {
            if (attackHit.collider.CompareTag(playerTag))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }

        isAttacking = false;
        Destroy(indicator);
    }
}
