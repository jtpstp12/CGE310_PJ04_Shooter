using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private NavMeshAgent agent;
    private ZombieAI zombieAI;
    public bool isDead = false; // ���Ǩ�ͺ��ҵ�������ѧ

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieAI = GetComponent<ZombieAI>();
    }

    // �ѧ��ѹ�Ѻ�����
    public void TakeDamage(float amount)
    {
        if (isDead) return; // ��ҵ������ ����Ѻ������ա

        currentHealth -= amount;
        Debug.Log("Zombie took damage: " + amount + " | Remaining HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // �ѧ��ѹ���
    void Die()
    {
        if (isDead) return; // ��ͧ�ѹ��õ�«�ӫ�͹
        isDead = true;

        // �� GameManager ��Ҧ�ҫ����������
        GameManager.instance.AddKill();

        // ��ش AI
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (zombieAI != null)
        {
            zombieAI.enabled = false;
        }

        // ���͹����ѹ���
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // �Դ Collider ��������ҡ���ⴹ�ԧ���
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // ����� Zombie ��ѧ 10 �Թҷ�
        Destroy(gameObject, 10f);
    }
}
