using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SawTurret : MonoBehaviour
{
    public float damageOverTime = 10f;
    public float damageInterval = 0.5f;
    public float sawRadius = 3f; // ������ �������� ����
    public float sawSpeed = 200f; // �������� �������� ���� � ��������/���
    public string enemyTag = "Enemy";

    [Header("Saw Visuals")]
    public GameObject sawBladePrefab; // ������ ����� ����
    private GameObject currentSawBlade;

    private Coroutine attackCoroutine;

    void Start()
    {
        // ������� ���� � ������ � �������� ��������
        if (sawBladePrefab != null)
        {
            currentSawBlade = Instantiate(sawBladePrefab, transform.position, Quaternion.identity, transform);
        }

        // ��������� ����� �����
        attackCoroutine = StartCoroutine(AttackContinuously());
    }

    void Update()
    {
        // ������� ���� ������ �����
        if (currentSawBlade != null)
        {
            currentSawBlade.transform.RotateAround(transform.position, Vector3.forward, sawSpeed * Time.deltaTime);
        }
    }

    IEnumerator AttackContinuously()
    {
        while (true)
        {
            List<Enemy> enemiesToDamage = new List<Enemy>();
            // ������� ���� ������ � ������� ����
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, sawRadius);

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

            // ������� ���� ���� ������ � �������
            foreach (Enemy enemy in enemiesToDamage)
            {
                enemy.TakeDamage(damageOverTime);
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, sawRadius);
    }
}