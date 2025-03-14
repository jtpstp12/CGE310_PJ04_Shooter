using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float chaseRange = 10f;
    public float patrolRadius = 15f;
    public float patrolWaitTime = 3f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;
    private ZombieHealth zombieHealth;

    private bool isPatrolling = true;
    private Vector3 patrolDestination;
    private float patrolTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        agent.speed = moveSpeed;
        zombieHealth = GetComponent<ZombieHealth>();

        SetNewPatrolDestination(); // เริ่มต้นตั้งจุด Patrol
    }

    void Update()
    {
        if (!player || zombieHealth == null || zombieHealth.isDead) return; // ถ้า Zombie ตายแล้วไม่ทำงาน

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ถ้าเจอผู้เล่นในระยะ ไล่ล่า
        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        isPatrolling = true;
        animator.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, patrolDestination) <= 1f)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolDestination();
                patrolTimer = 0f;
            }
        }
        else
        {
            agent.SetDestination(patrolDestination);
            agent.isStopped = false;
        }
    }

    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            patrolDestination = hit.position;
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        isPatrolling = false;
        agent.SetDestination(player.position);
        agent.isStopped = false;
        animator.SetBool("isWalking", true);

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // Animation โจมตี
    }

    // เรียกจาก Animation Event เพื่อทำ Damage
    public void DealDamage()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (player.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(attackDamage);
                Debug.Log("Zombie attacked player!");
            }
        }
    }
}
