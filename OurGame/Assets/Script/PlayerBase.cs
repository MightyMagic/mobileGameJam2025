using UnityEngine;
using TMPro; // �������� ��� ������ ��� ������ � TextMeshPro

public class PlayerBase : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public TextMeshProUGUI healthText; // ������ �� UI TextMeshPro

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
            // ��������� �����, ����� �������� ������� ��������
            healthText.text = currentHealth.ToString("F0"); // "F0" ��������� ����� �� ������
        }
    }

    private void Die()
    {
        Debug.Log("Player Base has been destroyed! Game Over.");
        // ����� ����� �������� ������ ���������� ����, ��������, ����� ����
        Destroy(gameObject);
    }
}