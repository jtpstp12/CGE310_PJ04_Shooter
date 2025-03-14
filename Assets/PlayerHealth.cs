using UnityEngine;
using UnityEngine.UI; // สำหรับเชื่อม UI

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthSlider; // ลาก Slider UI มาใส่

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // กันเลือดติดลบ
        healthSlider.value = currentHealth; // อัพเดท UI

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ไม่เกิน max
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Player Dead!");
        // เพิ่มสิ่งที่อยากให้เกิด เช่น Game Over
    }
}
