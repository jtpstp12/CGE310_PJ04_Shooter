using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private NavMeshAgent agent;
    private ZombieAI zombieAI;
    public bool isDead = false; // ใช้ตรวจสอบว่าตายหรือยัง

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieAI = GetComponent<ZombieAI>();
    }

    // ฟังก์ชันรับดาเมจ
    public void TakeDamage(float amount)
    {
        if (isDead) return; // ถ้าตายแล้ว ไม่รับดาเมจอีก

        currentHealth -= amount;
        Debug.Log("Zombie took damage: " + amount + " | Remaining HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // ฟังก์ชันตาย
    void Die()
    {
        if (isDead) return; // ป้องกันการตายซ้ำซ้อน
        isDead = true;

        // แจ้ง GameManager ว่าฆ่าซอมบี้สำเร็จ
        GameManager.instance.AddKill();

        // หยุด AI
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (zombieAI != null)
        {
            zombieAI.enabled = false;
        }

        // เล่นอนิเมชันตาย
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // ปิด Collider ถ้าไม่อยากให้โดนยิงต่อ
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // ทำลาย Zombie หลัง 10 วินาที
        Destroy(gameObject, 10f);
    }
}
