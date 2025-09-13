using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SawBladeDamage : MonoBehaviour
{
    [Header("Damage")]
    public float damageOverTime = 10f;
    public float damageInterval = 0.5f;
    public float damageRadius = 1f; // Новый настраиваемый радиус урона
    public string enemyTag = "Enemy";

    private Coroutine damageCoroutine;

    void Start()
    {
        // Запускаем корутину для нанесения урона
        damageCoroutine = StartCoroutine(DamageEnemies());
    }

    // Корутина для периодического нанесения урона
    IEnumerator DamageEnemies()
    {
        while (true)
        {
            List<Enemy> enemiesToDamage = new List<Enemy>();
            // Находим всех врагов в радиусе урона
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

            // Наносим урон всем врагам, найденным в радиусе
            foreach (Enemy enemy in enemiesToDamage)
            {
                enemy.TakeDamage(damageOverTime);
            }

            // Ждём перед следующим нанесением урона
            yield return new WaitForSeconds(damageInterval);
        }
    }

    // Визуализация радиуса урона в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}