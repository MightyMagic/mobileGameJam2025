using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Характеристики противника
    [Header("Base Stats")]
    public float maxHealth;
    public float currentHealth;
    public float moveSpeed;
    public float damage;

    [Header("Rewards")]
    public int choicePointsOnDeath = 10; // Сколько очков даётся за убийство

    [Header("Attack Behavior")]
    public Transform target;
    public float attackDelay = 1f;
    public float attackRange = 1f;

    private bool hasReachedTarget = false;
    private bool isAttacking = false;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Enemy requires a Rigidbody2D component to move properly!");
            this.enabled = false;
            return;
        }

        GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("No target found with 'Target' tag!");
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!hasReachedTarget && target != null)
        {
            Vector2 newPosition = transform.position;
            newPosition.y = Mathf.MoveTowards(transform.position.y, target.position.y, moveSpeed * Time.deltaTime);
            transform.position = newPosition;

            if (transform.position.y <= target.position.y)
            {
                hasReachedTarget = true;
                StartCoroutine(AttackTarget());
            }
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        currentHealth -= incomingDamage;

        Debug.Log($"{gameObject.name} took {incomingDamage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} was defeated!");

        GameManager.AddChoicePoints(choicePointsOnDeath);

        Destroy(gameObject);
    }

    IEnumerator AttackTarget()
    {
        if (isAttacking)
        {
            yield break;
        }

        isAttacking = true;
        Debug.Log($"{gameObject.name} reached the target and will attack in {attackDelay} seconds.");

        yield return new WaitForSeconds(attackDelay);

        if (target != null)
        {
            PlayerBase targetBase = target.GetComponent<PlayerBase>();
            if (targetBase != null)
            {
                targetBase.TakeDamage(damage);
                Debug.Log($"{gameObject.name} attacked the target for {damage} damage.");
            }
            else
            {
                Debug.LogWarning("Target does not have a PlayerBase component!");
            }
        }

        Destroy(gameObject);
    }
}