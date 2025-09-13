using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SawTurret : MonoBehaviour
{
    public float damageOverTime = 10f;
    public float damageInterval = 0.5f;
    public float sawRadius = 3f; // Радиус вращения пилы
    public float sawSpeed = 200f; // Скорость вращения пилы в градусах/сек
    public string enemyTag = "Enemy";

    [Header("Saw Visuals")]
    public GameObject sawBladePrefab; // Префаб самой пилы
    private GameObject currentSawBlade;

    private Coroutine attackCoroutine;

    void Start()
    {
        // Создаем пилу и делаем её дочерним объектом
        if (sawBladePrefab != null)
        {
            currentSawBlade = Instantiate(sawBladePrefab, transform.position, Quaternion.identity, transform);
        }

        // Запускаем атаку сразу
        attackCoroutine = StartCoroutine(AttackContinuously());
    }

    void Update()
    {
        // Вращаем пилу вокруг пушки
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
            // Находим всех врагов в радиусе пилы
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

            // Наносим урон всем врагам в области
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