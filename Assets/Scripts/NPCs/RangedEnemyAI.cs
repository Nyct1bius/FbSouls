using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Shooting,
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

    private void Awake()
    {
        state = State.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        waypointIndex = Random.Range(0, waypoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                self = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                agent.SetDestination(self);

                if (Vector3.Distance(transform.position, player.position) <= 20f && health > 0)
                {
                    state = State.Shooting;
                }
                break;

            case State.Shooting:
                targetWaypoint = waypoints[waypointIndex].position;
                agent.SetDestination(targetWaypoint);

                if (Vector3.Distance(transform.position, targetWaypoint) <= 1.5f)
                {
                    waypointIndex = Random.Range(0, waypoints.Length);
                }

                if(Vector3.Distance(transform.position, player.position) > 40f && health > 0)
                {
                    state = State.Idle;
                }
                break;

            case State.Dead:
                Destroy(gameObject);
                break;
        }
    }

    //HEALTH
    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            state = State.Dead;
        }
    }
}
