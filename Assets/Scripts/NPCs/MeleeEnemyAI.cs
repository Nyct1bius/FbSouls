using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : MonoBehaviour
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

    private Rigidbody rb;

    private Transform player;
    private Vector3 combatTarget;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;

    private void Awake()
    {
        state = State.Patrolling;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        FindNewWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Patrolling:
                if (Vector3.Distance(transform.position, target) <= 1.5f)
                {
                    IterateWaypointIndex();
                    FindNewWaypoint();
                }
                break;

            case State.Attacking:
                combatTarget = new Vector3(player.position.x, player.position.y, player.position.z);
                self = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                if (Vector3.Distance(transform.position, player.position) >= 3f)
                {
                    agent.SetDestination(combatTarget);
                    Debug.Log("Move");
                }
                else
                {
                    agent.SetDestination(self);
                    Debug.Log("Stop");
                }
                break;

            case State.Dead:
                Destroy(gameObject);
                break;
        }

        SwitchState();
    }

    //PATROLLING
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

    //ATTACKING

    //HEALTH
    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            state = State.Dead;
        }
    }

    //STATES
    private void SwitchState()
    {
        if (Vector3.Distance(transform.position, player.position) <= 20f && health > 0)
        {
            state = State.Attacking;
        }
        if (Vector3.Distance(transform.position, player.position) > 20f && health > 0)
        {
            state = State.Patrolling;
        }

        if (health <= 0)
        {
            state = State.Dead;
        }
    }
}
