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

    int effectCount = 0;

    [SerializeField] private GameObject deathEffectPrefab;

    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        //if (rb == null)
        // {
        //     Debug.LogError("Enemy requires a Rigidbody2D component to move properly!");
        //     this.enabled = false;
        //     return;
        // }

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

    // --- THIS IS THE FUNCTION YOU ASKED FOR ---
    /// <summary>
    /// Spawns the death effect at the enemy's current position.
    /// </summary>
    private IEnumerator SpawnDeathEffect()
    {

        GameObject boom = new GameObject();
        // First, check if the prefab has actually been assigned in the inspector
        if (deathEffectPrefab != null)
        {
            // Spawn the prefab at this enemy's position and with no rotation.
            boom = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.6f);

        if (boom != null)
            Destroy(boom);

        Destroy(gameObject);

    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} was defeated!");

        // Use the existing effectCount check to ensure ALL death logic runs only once.
        if (effectCount < 1)
        {
            effectCount++; // Increment the counter immediately to prevent re-entry

            // Move the reward logic inside this check
            GameManager.Instance.AddChoicePoints(choicePointsOnDeath);
            ActionPhaseManager.Instance.EnemyDied();

            // Start the death effect
            StartCoroutine(SpawnDeathEffect());
        }

        //Destroy(gameObject);
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

        ActionPhaseManager.Instance.EnemyDied();
        Destroy(gameObject);
    }
}