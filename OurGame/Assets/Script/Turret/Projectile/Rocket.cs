using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 20f;
    public float explosionRadius = 2f;
    public GameObject explosionEffect; // �������������: ������ ������
    public string enemyTag = "Enemy";

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // ���������� Projectile.cs
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame)
        {
            Explode();
            return;
        }
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void Explode()
    {
        // ������������� ����� (�������������)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // ������� ���� ������ � ������� ������
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(enemyTag))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
        Destroy(gameObject);
    }

    // ������������ ������� ������ � ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}