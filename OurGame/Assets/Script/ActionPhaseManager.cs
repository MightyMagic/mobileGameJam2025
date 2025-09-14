using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPhaseManager : MonoBehaviour
{
    public static ActionPhaseManager Instance;

    [SerializeField] private RailMover moveScript;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] private PlayerEquipmentManager playerEquipmentManager;

    [SerializeField] GameObject actionButton;
    [SerializeField] ResourceDisplayUI resourceScript;

    public int currentLevel = 0;
    public int deathsThisLevel;
    public int expectedDeathsThisLevel = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void EnableAction()
    {
        if (resourceScript.DoChecks())
        {
            Debug.Log("Action PHASE STARTED: Enabling action tools!");

            //actionButton.SetActive(false);

            deathsThisLevel = 0;
            expectedDeathsThisLevel = CalculateEnemies();

            moveScript.InitializeMover();
            Debug.Log("I start spawning in the manager");

            spawner.ResetWaves();
            spawner.StartSpawning();

            UpdateEquippedWeaponFromInventory();
        }
    }

    private void UpdateEquippedWeaponFromInventory()
    {
        // Получаем весь список экипированных предметов
        List<UpgradeData> equippedItems = Inventory.Instance.GetEquippedItems();

        if (equippedItems != null && equippedItems.Count > 0)
        {
            // Перебираем каждый предмет в списке
            foreach (var item in equippedItems)
            {
                playerEquipmentManager.EquipWeapon(item.type);
            }
        }
        else
        {
            Debug.LogWarning("No weapons are currently equipped!");
        }
    }

    public int CalculateEnemies()
    {
        int expectedEnemies = 0;
        if (currentLevel < spawner.levels.Count)
        {
            for (int i = 0; i < spawner.levels[currentLevel].waves.Count; i++)
            {
                for (int j = 0; j < spawner.levels[currentLevel].waves[i].enemyGroups.Count; j++)
                {
                    expectedEnemies += spawner.levels[currentLevel].waves[i].enemyGroups[j].numberOfEnemies;
                }
            }
        }
        return expectedEnemies;
    }

    public void EnemyDied()
    {
        deathsThisLevel++;
        if (deathsThisLevel >= expectedDeathsThisLevel && expectedDeathsThisLevel > 0)
        {
            Debug.Log("Everyone died!!!!!!!!!!");
            EveryoneDied();
        }
    }

    public void EveryoneDied()
    {
        currentLevel++;
        GameManager.Instance.BeginUpgradeCheck();
    }

    private void DisableAction()
    {
        Debug.Log("Build PHASE Started: Disabling action tools.");
        actionButton.SetActive(true);
        moveScript.DisableMover();
    }

    void OnEnable()
    {
        GameManager.OnActionPhaseStart += EnableAction;
        GameManager.OnBuildPhaseStart += DisableAction;
    }
}