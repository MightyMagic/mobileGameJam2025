using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlamethrowerTurret : MonoBehaviour
{
    public float damageOverTime = 5f;
    public float damageInterval = 0.25f;
    public float attackRadius = 3f;
    public float coneAngle = 60f;
    public float rotationSpeed = 10f; // Скорость поворота пушки
    public string enemyTag = "Enemy";

    [Header("Visual Effects")]
    public GameObject fireEffectPrefab;
    public Transform fireEffectSpawnPoint;
    public string animatorTriggerName = "IsFiring";

    private Turret baseTurret;
    private Coroutine attackCoroutine;
    private GameObject currentFireEffectInstance;
    private Animator fireEffectAnimator;

    void Start()
    {
        baseTurret = GetComponent<Turret>();

        if (fireEffectPrefab != null)
        {
            Transform spawnPoint = fireEffectSpawnPoint != null ? fireEffectSpawnPoint : transform;
            currentFireEffectInstance = Instantiate(fireEffectPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            fireEffectAnimator = currentFireEffectInstance.GetComponent<Animator>();
            if (fireEffectAnimator == null)
            {
                Debug.LogWarning("Fire effect prefab does not have an Animator component!", this);
            }
            currentFireEffectInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (baseTurret.currentTarget != null)
        {
            // Плавно поворачиваемся к цели
            Vector2 direction = (Vector2)baseTurret.currentTarget.position - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // -90, если спрайт "смотрит" вверх
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackContinuously());
                PlayFireEffect();
            }
        }
        else if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            StopFireEffect();
        }
    }

    IEnumerator AttackContinuously()
    {
        while (baseTurret.currentTarget != null)
        {
            List<Enemy> enemiesToDamage = new List<Enemy>();
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag(enemyTag))
                {
                    Vector2 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                    Vector2 forwardDirection = transform.up; // Теперь используем направление пушки

                    float angle = Vector2.Angle(forwardDirection, directionToEnemy);

                    if (angle < coneAngle / 2f)
                    {
                        Enemy enemy = hitCollider.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemiesToDamage.Add(enemy);
                        }
                    }
                }
            }

            foreach (Enemy enemy in enemiesToDamage)
            {
                enemy.TakeDamage(damageOverTime);
            }

            yield return new WaitForSeconds(damageInterval);
        }
        StopFireEffect();
    }

    private void PlayFireEffect()
    {
        if (currentFireEffectInstance != null)
        {
            currentFireEffectInstance.SetActive(true);
            if (fireEffectAnimator != null)
            {
                fireEffectAnimator.SetBool(animatorTriggerName, true);
            }
        }
    }

    private void StopFireEffect()
    {
        if (currentFireEffectInstance != null)
        {
            if (fireEffectAnimator != null)
            {
                fireEffectAnimator.SetBool(animatorTriggerName, false);
            }
            currentFireEffectInstance.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (baseTurret != null && baseTurret.currentTarget != null)
        {
            Gizmos.color = Color.yellow;
            Vector2 forwardDirection = (baseTurret.currentTarget.position - transform.position).normalized;
            DrawCone(forwardDirection, coneAngle, attackRadius);
        }
        else
        {
            Gizmos.color = Color.cyan;
            DrawCone(transform.up, coneAngle, attackRadius);
        }
    }

    private void DrawCone(Vector2 direction, float angle, float length)
    {
        Quaternion rotation = Quaternion.AngleAxis(-angle / 2f, Vector3.forward);
        Vector2 startDirection = rotation * direction;

        Gizmos.DrawRay(transform.position, startDirection * length);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(angle, Vector3.forward) * startDirection * length);
    }
}