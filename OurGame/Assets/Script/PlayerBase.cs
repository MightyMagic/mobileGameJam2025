using UnityEngine;
using UnityEngine.UI;
using TMPro; // Убедитесь, что у вас есть это для TextMeshPro

public class PlayerBase : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthSlider; // Ссылка на наш Slider
    public TextMeshProUGUI healthText; // (Необязательно) Ссылка на текст

    void Start()
    {
        currentHealth = maxHealth;
        // Устанавливаем начальные значения для Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        // Обновляем текст
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Base Health: " + currentHealth);

        // Обновляем Slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Обновляем текст
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString("0") + "/" + maxHealth.ToString("0");
        }
    }

    private void Die()
    {
        Debug.Log("Game Over!");

        GameOver.Instance.ShowGameOverScreen();
        // Добавьте сюда логику конца игры
    }
}