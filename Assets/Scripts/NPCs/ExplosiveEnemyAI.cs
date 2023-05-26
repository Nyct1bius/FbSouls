using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosiveEnemyAI : MonoBehaviour
{
    public float health;

    private NavMeshAgent agent;

    private Transform player;

    private Vector3 target;

    [SerializeField] private float blastRadius, combatRadius;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        target = new Vector3(player.position.x, player.position.z);
        
        if (Vector3.Distance(transform.position, player.position) <= combatRadius)
        {
            agent.SetDestination(target);
        }

        if (Vector3.Distance(transform.position, player.position) <= blastRadius)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
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
