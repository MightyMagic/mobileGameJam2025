using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SawBladeDamage : MonoBehaviour
{
    [Header("Damage")]
    public float damageOverTime = 10f;
    public float damageInterval = 0.5f;
    public float damageRadius = 1f; // ����� ������������� ������ �����
    public string enemyTag = "Enemy";

    private Coroutine damageCoroutine;

    void Start()
    {
        // ��������� �������� ��� ��������� �����
        damageCoroutine = StartCoroutine(DamageEnemies());
    }

    // �������� ��� �������������� ��������� �����
    IEnumerator DamageEnemies()
    {
        while (true)
        {
            List<Enemy> enemiesToDamage = new List<Enemy>();
            // ������� ���� ������ � ������� �����
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag(enemyTag))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemiesToDamage.Add(enemy);
                    }
                }
            }

            // ������� ���� ���� ������, ��������� � �������
            foreach (Enemy enemy in enemiesToDamage)
            {
                enemy.TakeDamage(damageOverTime);
            }

            // ��� ����� ��������� ���������� �����
            yield return new WaitForSeconds(damageInterval);
        }
    }

    // ������������ ������� ����� � ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}