using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("Targeting")]
    public float detectionRadius = 5f;
    public string enemyTag = "Enemy";

    public Transform currentTarget;
    private float fireRate = 1f; // Базовая скорострельность, будет переопределена

    private void Update()
    {
        FindTarget();
    }

    // Ищет ближайшего врага в радиусе обнаружения
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

        // Если нашли цель, назначаем её, иначе сбрасываем
        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    // Метод для визуализации радиуса в редакторе Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}