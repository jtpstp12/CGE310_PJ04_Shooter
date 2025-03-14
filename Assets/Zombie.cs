using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float detectionRange = 10f;  // ���з���������繼�����
    public float roamRadius = 5f; // ����շ���Թ������
    public float roamCooldown = 3f; // ������ش�ѡ�����ҧ�Թ����

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;
    private float nextRoamTime = 0f;
    private ZombieHealth zombieHealth;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player")?.transform;
        agent.speed = moveSpeed;
        zombieHealth = GetComponent<ZombieHealth>();
    }

    void Update()
    {
        if (!player || zombieHealth == null || zombieHealth.isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            isChasing = true;
        }
        else if (distance > detectionRange && isChasing)
        {
            isChasing = false;
            nextRoamTime = Time.time + roamCooldown; // ������Ѻ�����Թ�����ա����
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            RoamAround();
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero; // ��ͧ�ѹ�������͹�Դ��Ҵ
            animator.SetBool("isWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void RoamAround()
    {
        if (Time.time >= nextRoamTime && !agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            Vector3 newDestination = GetRandomPoint(transform.position, roamRadius);
            agent.SetDestination(newDestination);
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            nextRoamTime = Time.time + roamCooldown;
        }
        else if (agent.remainingDistance <= 0.5f)
        {
            animator.SetBool("isWalking", false); // ����¹�� Idle
        }
    }

    Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle * radius;
        Vector3 randomPoint = new Vector3(origin.x + randomDirection.x, origin.y, origin.z + randomDirection.y);

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return origin;
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // ���͹����ѹ����
        Invoke("ResetAttackTrigger", 0.5f); // ���絷�ԡ������ѧ�ҡ���令����Թҷ�
    }

    void ResetAttackTrigger()
    {
        animator.ResetTrigger("Attack");
    }


    void DealDamage()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (player.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.TakeDamage(attackDamage); // Ŵ���ʹ������
                Debug.Log("Zombie attacked player! Damage: " + attackDamage);

            }
            else
            {
                Debug.Log("PlayerHealth component not found on player!");
            }
        }
        else
        {
            Debug.Log("Player is out of attack range!");
        }
    }

}
