using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float moveSpeed = 3.5f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.SetDestination(player.position); // �Թ��� Player
        }
        else if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown; // ������Ҥ�Ŵ�ǹ�
        }
    }

    void Attack()
    {
        Debug.Log("Zombie ���� Player!");
        player.GetComponent<Health>().TakeDamage(attackDamage);
    }
}
