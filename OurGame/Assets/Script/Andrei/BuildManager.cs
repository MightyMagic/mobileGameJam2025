using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // 1. Singleton
    public static BuildManager Instance { get; private set; }

    // 2. C# Event (now only sends one 'int' for the new total)
    public static event Action<int> OnResourcesUpdated;

    [Header("Player Resources")]
    [Tooltip("The primary currency for building rails.")]
    [SerializeField] private int railResources = 10;

    //[SerializeField] GameObject tileGameObjects;

    [SerializeField] private List<Collider2D> tileCollider = new List<Collider2D>();

    // Public getter so other scripts can read the total
    public int RailResources => railResources;

    public bool placementStage = true;

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

    // This is the method we want to run when the build phase starts.
    private void EnableBuilding()
    {
        Debug.Log("BUILD PHASE STARTED: Enabling build tools!");

        //tileGameObjects.SetActive(true);
        placementStage = true;

        for (int i = 0; i < tileCollider.Count; ++i)
        {
            tileCollider[i].enabled = true;
        }

        OnResourcesUpdated?.Invoke(railResources);
        // Your logic here:
        // e.g., showBuildCursor = true;
        //      player.GetComponent<WeaponController>().enabled = false;
    }

    // This runs when the action phase starts
    private void DisableBuilding()
    {


        Debug.Log("ACTION PHASE STARTED: Disabling build tools.");
        for (int i = 0; i < tileCollider.Count; ++i)
        {
            tileCollider[i].enabled = false;
        }

        // Your logic here:
        // e.g., showBuildCursor = false;
        //      player.GetComponent<WeaponController>().enabled = true;
    }


    // 1. Subscribe to the event when this object is enabled
    void OnEnable()
    {
        GameManager.OnBuildPhaseStart += EnableBuilding;
        GameManager.OnActionPhaseStart += DisableBuilding;
    }

    // 2. ALWAYS unsubscribe when the object is disabled to prevent errors
    void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= EnableBuilding;
        GameManager.OnActionPhaseStart -= DisableBuilding;
    }

    void Start()
    {
        // Broadcast the starting resource total
        //OnResourcesUpdated?.Invoke(railResources);
    }

    // 3. Public API

    /// <summary>
    /// Attempts to spend the specified amount of rail resources.
    /// Returns TRUE if successful, FALSE if the player cannot afford it.
    /// </summary>
    public bool AttemptToSpend(int cost)
    {
        if (CanAfford(cost))
        {
            // Subtract resources
            railResources -= cost;

            Debug.Log($"Spent: {cost} resources. Remaining: {railResources}");

            // Fire the event to notify listeners (UI) of the new total
            OnResourcesUpdated?.Invoke(railResources);

            return true; // Purchase successful
        }

        Debug.LogWarning($"Not enough resources! Need: {cost}, Have: {railResources}");
        return false; // Purchase failed
    }

    /// <summary>
    /// Public check to see if the player has enough resources (does not spend).
    /// </summary>
    public bool CanAfford(int cost)
    {
        return (railResources >= cost);
    }

    /// <summary>
    /// Adds a specified amount of resources to the player's total.
    /// </summary>
    public void AddResources(int amount)
    {
        railResources += amount;
        Debug.Log($"Gained: {amount} resources. Total: {railResources}");

        // Fire the event to update the UI
        OnResourcesUpdated?.Invoke(railResources);
    }
}
