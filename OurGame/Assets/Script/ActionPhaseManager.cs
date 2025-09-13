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

        moveScript.InitializeMover();
        spawner.StartSpawning();
        
    }

    // This runs when the action phase starts
    private void DisableAction()
    {

        Debug.Log("Build PHASE Started: Disabling action tools.");

        canvasObject.SetActive(false);
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
