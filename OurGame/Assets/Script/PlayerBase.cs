using UnityEngine;
using TMPro; // Добавьте эту строку для работы с TextMeshPro

public class PlayerBase : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public TextMeshProUGUI healthText; // Ссылка на UI TextMeshPro

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player Base took {damage} damage. Remaining health: {currentHealth}");

        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            // Обновляем текст, чтобы показать текущее здоровье
            healthText.text = currentHealth.ToString("F0"); // "F0" округляет число до целого
        }
    }

    private void Die()
    {
        Debug.Log("Player Base has been destroyed! Game Over.");
        // Здесь можно добавить логику завершения игры, например, показ меню
        Destroy(gameObject);
    }
}