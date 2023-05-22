using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingMobAI : MonoBehaviour
{
    public float health;
    
    private NavMeshAgent agent;

    private Transform player;

    [SerializeField] private LayerMask groundMask, playerMask;

    private Vector3 waypoint, boltWaypoint;
    private bool waypointSet, boltWaypointSet;
    [SerializeField] private float waypointRadius;

    [SerializeField] private float agroRange;
    private bool playerInAgroRange, isBolting;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerInAgroRange = Physics.CheckSphere(transform.position, waypointRadius, playerMask);

        if (!playerInAgroRange && !isBolting)
        {
            Patrolling();
        }
        if (playerInAgroRange)
        {
            isBolting = true;

            if (isBolting)
            {
                Bolting();
            }
        }
    }

    private void Patrolling()
    {
        if (!waypointSet)
        {
            FindWaypoint();
        }
        else
        {
            agent.SetDestination(waypoint);
        }

        Vector3 distanceToWaypoint = transform.position - waypoint;

        if (distanceToWaypoint.magnitude <= 1f)
        {
            waypointSet = false;
        }
    }

    private void FindWaypoint()
    {
        float randomZ = Random.Range(-waypointRadius, waypointRadius);
        float randomX = Random.Range(-waypointRadius, waypointRadius);

        waypoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(waypoint, -transform.up, 2f, groundMask))
        {
            waypointSet = true; 
        }
    }

    private void Bolting()
    {
        if (!boltWaypointSet)
        {
            FindBoltWaypoint();
        }
        else
        {
            agent.SetDestination(boltWaypoint); 
        }

        Vector3 distanceToBoltWaypoint = transform.position - waypoint;

        if (distanceToBoltWaypoint.magnitude <= 1f)
        {
            isBolting = false;
        }
    }

    private void FindBoltWaypoint()
    {
        boltWaypoint = new Vector3(transform.position.x - player.position.x, transform.position.y, transform.position.z - player.position.z);
    }

    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
