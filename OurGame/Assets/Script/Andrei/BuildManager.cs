using System;
using System.Collections;
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

    void Start()
    {
        // Broadcast the starting resource total
        OnResourcesUpdated?.Invoke(railResources);
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
