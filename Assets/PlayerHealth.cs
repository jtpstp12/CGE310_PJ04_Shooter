using UnityEngine;
using UnityEngine.UI; // ����Ѻ����� UI

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthSlider; // �ҡ Slider UI �����

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // �ѹ���ʹ�Դź
        healthSlider.value = currentHealth; // �Ѿഷ UI

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ����Թ max
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Player Dead!");
        // ������觷����ҡ����Դ �� Game Over
    }
}
