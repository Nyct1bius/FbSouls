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

    private Transform player;
    private Vector3 combatTarget;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;

    private void Awake()
    {
        state = State.firstPhase;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.firstPhase:
                if (health <= 400)
                {
                    state = State.secondPhase;
                }
                break;

            case State.secondPhase:
                if (health <= 300)
                {
                    state = State.thirdPhase;
                }
                break;

            case State.thirdPhase:
                if (health <= 200)
                {
                    state = State.fourthPhase;
                }
                break;

            case State.fourthPhase:
                if (health <= 100)
                {
                    state = State.fifthPhase;
                }
                break;

            case State.fifthPhase:
                if (health <= 0)
                {
                    state = State.Dead;
                }
                break;

            case State.Dead:
                break;
        }
    }
}
