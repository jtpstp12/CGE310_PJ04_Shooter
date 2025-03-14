using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform; // ค้นหา Player
        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
            GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            agent.isStopped = true;
            GetComponent<Animator>().SetBool("isWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Attack()
    {
        GetComponent<Animator>().SetTrigger("zombie_attack");
        player.GetComponent<Health>().TakeDamage(attackDamage);
    }
}
