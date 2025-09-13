using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPhaseManager : MonoBehaviour
{
    // Public getter so other scripts can read the total
    
    public static ActionPhaseManager Instance;

    [SerializeField] private RailMover moveScript;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] EnemySpawner spawner;

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
            // DontDestroyOnLoad(gameObject);
        }
    }

    private void EnableAction()
    {
        Debug.Log("Action PHASE STARTED: Enabling action tools!");

        canvasObject.SetActive(true);

        deathsThisLevel = 0;

        expectedDeathsThisLevel = CalculateEnemies();

        moveScript.InitializeMover();
        Debug.Log("I start spawning in the manager");

        spawner.ResetWaves();
        spawner.StartSpawning();
    }

    public int CalculateEnemies()
    {
        int expectedEnemies = 0;

        if(currentLevel < spawner.levels.Count)
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

        if(deathsThisLevel >= expectedDeathsThisLevel && expectedDeathsThisLevel > 0)
        {
            Debug.Log("Everyone died!!!!!!!!!!");
            EveryoneDied();
        }
    }

    public void EveryoneDied()
    {
        currentLevel++;

        canvasObject.SetActive(false);

        moveScript.DisableMover();

        GameManager.Instance.BeginBuildPhase();
    }

    // This runs when the action phase starts
    private void DisableAction()
    {

        Debug.Log("Build PHASE Started: Disabling action tools.");

        currentLevel++;

        canvasObject.SetActive(false);

        moveScript.DisableMover();
    }

    void OnEnable()
    {
        GameManager.OnActionPhaseStart += EnableAction;
        GameManager.OnBuildPhaseStart -= DisableAction;
    }

    // 2. ALWAYS unsubscribe when the object is disabled to prevent errors
    void OnDisable()
    {
        GameManager.OnActionPhaseStart -= EnableAction;
        GameManager.OnBuildPhaseStart -= DisableAction;
    }
}
