using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        FindNewWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) <= 1.5f)
        {
            IterateWaypointIndex();
            FindNewWaypoint();
        }
    }

    private void FindNewWaypoint()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;

        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
