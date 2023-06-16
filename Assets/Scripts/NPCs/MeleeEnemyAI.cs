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

    private Transform player;
    private Vector3 combatTarget;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 targetWaypoint;

    private Animator anim;

    [SerializeField] private GameObject meleeHitbox;

    private float timeBetweenAttacks;

    private bool isAlive;

    private void Awake()
    {
        isAlive = true;
        
        state = State.Patrolling;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();   

        player = GameObject.FindGameObjectWithTag("Player").transform;

        FindNewWaypoint();

        anim = GetComponent<Animator>();

        timeBetweenAttacks = 1f;
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

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", true);
                    anim.SetBool("Running", false);
                    anim.SetBool("Dead", false);
                }
                break;

            case State.Attacking:
                if (Vector3.Distance(transform.position, player.position) >= 1.5f)
                {
                    combatTarget = new Vector3(player.position.x, player.position.y, player.position.z);
                    agent.SetDestination(combatTarget);

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Running", true);
                    anim.SetBool("Dead", false);
                }
                else
                {
                    self = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    agent.SetDestination(self);

                    anim.SetBool("Idle", true);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Running", false);
                    anim.SetBool("Dead", false);

                    timeBetweenAttacks -= Time.deltaTime;

                    if (timeBetweenAttacks <= 0)
                    {
                        anim.SetTrigger("Melee");
                    }
                }
                break;

            case State.Dead:
                self = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                agent.SetDestination(self);

                anim.SetBool("Dead", true);
                break;
        }

        SwitchState();
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

    //HEALTH
    public void TakeDamge(int damage)
    {
        health -= damage;

        if (!isAlive)
        {
            anim.SetTrigger("Hit");
        }

        if (health <= 0)
        {
            isAlive = false;
            state = State.Dead;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
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
            timeBetweenAttacks = 1f;
            state = State.Patrolling;
        }
    }

    //COMBAT
    public void MeleeHitboxOn()
    {
        meleeHitbox.SetActive(true);
    }
    public void MeleeHitboxOff()
    {
        meleeHitbox.SetActive(false);

        timeBetweenAttacks = Random.Range(2f, 3f);
    }
}
