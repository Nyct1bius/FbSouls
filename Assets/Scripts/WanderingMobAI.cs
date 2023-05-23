using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingMobAI : MonoBehaviour
{
    public float health;
    
    private NavMeshAgent agent;

    [SerializeField] private LayerMask groundMask;

    private Vector3 waypoint;
    private bool waypointSet;
    [SerializeField] private float waypointRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {        
        Patrolling();
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

    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
