using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    private enum State
    {
        Patrolling,
        Attacking,
        Dead
    }

    private State state;

    public float health;

    private NavMeshAgent agent;
    private Vector3 self;

    private Transform player;
    private Vector3 combatTarget;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 targetWaypoint;

    private bool isAttacking;

    private void Awake()
    {
        state = State.Patrolling;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        FindNewWaypoint();

        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Patrolling:
                if (Vector3.Distance(transform.position, targetWaypoint) <= 1.5f)
                {
                    IterateWaypointIndex();
                    FindNewWaypoint();
                }
                break;

            case State.Attacking:
                if (Vector3.Distance(transform.position, player.position) >= 3f)
                {
                    combatTarget = new Vector3(player.position.x, player.position.y, player.position.z);
                    agent.SetDestination(combatTarget);
                }
                else
                {
                    self = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    agent.SetDestination(self);
                }

                if (Vector3.Distance(transform.position, player.position) > 30 && health > 0)
                {
                    isAttacking = false;
                    state = State.Patrolling;
                }
                break;

            case State.Dead:
                Destroy(gameObject);
                break;
        }
    }

    //PATROLLING
    private void FindNewWaypoint()
    {
        targetWaypoint = waypoints[waypointIndex].position;
        agent.SetDestination(targetWaypoint);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;

        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    //ATTACKING

    //HEALTH
    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            state = State.Dead;
        }
        if (health > 0 && !isAttacking)
        {
            isAttacking = true;
            state = State.Attacking;
        }
    }
}
