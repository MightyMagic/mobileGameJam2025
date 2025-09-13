using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("Targeting")]
    public float detectionRadius = 5f;
    public string enemyTag = "Enemy";

    public Transform currentTarget;
    private float fireRate = 1f; // ������� ����������������, ����� ��������������

    private void Update()
    {
        FindTarget();
    }

    // ���� ���������� ����� � ������� �����������
    private void FindTarget()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(enemyTag))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hitCollider.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        // ���� ����� ����, ��������� �, ����� ����������
        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    // ����� ��� ������������ ������� � ��������� Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}