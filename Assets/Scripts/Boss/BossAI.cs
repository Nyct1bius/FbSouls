using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private enum State
    {
        firstPhase,
        secondPhase,
        thirdPhase,
        fourthPhase,
        fifthPhase,
        Dead
    }

    private State state;

    public float health;

    private NavMeshAgent agent;
    private Vector3 self;
    private Animator anim;

    private Transform player;
    private Vector3 combatTarget;

    private float timeBetweenMeleeAttacks, timeBetweenRangedAttacks;

    [SerializeField] private GameObject meleeHitbox, projectile;

    [SerializeField] private Transform projectileSpawnPoint;

    private Transform centerOfArena;
    private Vector3 arenaTarget;

    private bool fifthFaseOn, isAlive;

    private void Awake()
    {
        fifthFaseOn = false;
        isAlive = true;
        
        state = State.firstPhase;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenMeleeAttacks = 1f;
        timeBetweenRangedAttacks = 1f;

        centerOfArena = GameObject.FindGameObjectWithTag("CenterOfArena").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.firstPhase:
                self = new Vector3(transform.position.x, transform.position.z);
                combatTarget = new Vector3(player.position.x, player.position.z);
                
                if (Vector3.Distance(transform.position, player.position) <= 5f)
                {
                    agent.SetDestination(self);

                    anim.SetBool("Idle", true);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Dead", false);

                    timeBetweenMeleeAttacks -= Time.deltaTime;

                    if(timeBetweenMeleeAttacks <= 0f)
                    {
                        anim.SetTrigger("Melee");
                    }
                }
                else
                {
                    agent.SetDestination(combatTarget);

                    timeBetweenMeleeAttacks = 1f;

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", true);
                    anim.SetBool("Dead", false);
                }
                
                if (health <= 400)
                {
                    state = State.secondPhase;
                }
                break;

            case State.secondPhase:
                self = new Vector3(transform.position.x, transform.position.z);
                combatTarget = new Vector3(player.position.x, player.position.z);

                if (Vector3.Distance(transform.position, player.position) <= 5f)
                {
                    agent.SetDestination(self);

                    timeBetweenRangedAttacks = 1f;

                    anim.SetBool("Idle", true);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Dead", false);

                    timeBetweenMeleeAttacks -= Time.deltaTime;

                    if (timeBetweenMeleeAttacks <= 0f)
                    {
                        anim.SetTrigger("Melee");
                    }
                }
                else
                {
                    timeBetweenMeleeAttacks = 1f;

                    timeBetweenRangedAttacks -= Time.deltaTime; 

                    if (timeBetweenRangedAttacks <= 0f)
                    {
                        agent.SetDestination(self);
                        
                        anim.SetBool("Idle", true);
                        anim.SetBool("Walking", false);
                        anim.SetBool("Dead", false);

                        anim.SetTrigger("Ranged");
                    }
                    else
                    {
                        agent.SetDestination(combatTarget);

                        anim.SetBool("Idle", false);
                        anim.SetBool("Walking", true);
                        anim.SetBool("Dead", false);
                    }
                }

                if (health <= 300)
                {
                    state = State.thirdPhase;
                }
                break;

            case State.thirdPhase:
                arenaTarget = new Vector3(centerOfArena.position.x, centerOfArena.position.z);

                if (Vector3.Distance(transform.position, centerOfArena.position) <= 4f)
                {
                    agent.SetDestination(arenaTarget);

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", true);
                    anim.SetBool("Dead", false);

                    timeBetweenRangedAttacks = 1f;
                }
                else
                {
                    anim.SetBool("Idle", true);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Dead", false);

                    timeBetweenRangedAttacks -= Time.deltaTime;

                    if (timeBetweenRangedAttacks <= 0f)
                    {
                        anim.SetTrigger("Ranged");
                    }
                }

                if (health <= 200)
                {
                    state = State.fourthPhase;
                }
                break;

            case State.fourthPhase:
                self = new Vector3(transform.position.x, transform.position.z);
                combatTarget = new Vector3(player.position.x, player.position.z);

                if (Vector3.Distance(transform.position, player.position) <= 5f)
                {
                    agent.SetDestination(self);

                    timeBetweenRangedAttacks = 1f;

                    anim.SetBool("Idle", true);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Dead", false);

                    timeBetweenMeleeAttacks -= Time.deltaTime;

                    if (timeBetweenMeleeAttacks <= 0f)
                    {
                        anim.SetTrigger("Melee");
                    }
                }
                else
                {
                    timeBetweenMeleeAttacks = 1f;

                    timeBetweenRangedAttacks -= Time.deltaTime;

                    if (timeBetweenRangedAttacks <= 0f)
                    {
                        agent.SetDestination(self);

                        anim.SetBool("Idle", true);
                        anim.SetBool("Walking", false);
                        anim.SetBool("Dead", false);

                        anim.SetTrigger("Ranged");
                    }
                    else
                    {
                        agent.SetDestination(combatTarget);

                        anim.SetBool("Idle", false);
                        anim.SetBool("Walking", true);
                        anim.SetBool("Dead", false);
                    }
                }

                if (health <= 100)
                {
                    state = State.fifthPhase;
                }
                break;

            case State.fifthPhase:
                anim.SetTrigger("FifthFase");

                if (fifthFaseOn)
                {
                    self = new Vector3(transform.position.x, transform.position.z);
                    combatTarget = new Vector3(player.position.x, player.position.z);

                    if (Vector3.Distance(transform.position, player.position) <= 5f)
                    {
                        agent.SetDestination(self);

                        timeBetweenRangedAttacks = 1f;

                        anim.SetBool("Idle", true);
                        anim.SetBool("Walking", false);
                        anim.SetBool("Dead", false);

                        timeBetweenMeleeAttacks -= Time.deltaTime;

                        if (timeBetweenMeleeAttacks <= 0f)
                        {
                            anim.SetTrigger("Melee");
                        }
                    }
                    else
                    {
                        timeBetweenMeleeAttacks = 1f;

                        timeBetweenRangedAttacks -= Time.deltaTime;

                        if (timeBetweenRangedAttacks <= 0f)
                        {
                            agent.SetDestination(self);

                            anim.SetBool("Idle", true);
                            anim.SetBool("Walking", false);
                            anim.SetBool("Dead", false);

                            anim.SetTrigger("Ranged");
                        }
                        else
                        {
                            agent.SetDestination(combatTarget);

                            anim.SetBool("Idle", false);
                            anim.SetBool("Walking", true);
                            anim.SetBool("Dead", false);
                        }
                    }
                }

                if (health <= 0)
                {
                    state = State.Dead;
                }
                break;

            case State.Dead:

                break;
        }
    }

    public void MeleeHitboxOn()
    {
        meleeHitbox.SetActive(true);
    }
    public void MeleeHitboxOff()
    {
        meleeHitbox.SetActive(false);

        timeBetweenMeleeAttacks = Random.Range(1f, 3f);
    }

    public void InstantiateProjectile()
    {
        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);

        timeBetweenRangedAttacks = Random.Range(1f, 3f);
    }

    public void FifthFaseOn()
    {
        fifthFaseOn = true;
    }

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
}
