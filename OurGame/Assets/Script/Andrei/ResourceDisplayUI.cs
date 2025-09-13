using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceDisplayUI : MonoBehaviour
{
    [Header("Component Reference")]
    [Tooltip("Drag the TMP_Text component you want to update here.")]
    [SerializeField] private TextMeshProUGUI resourceTextDisplay;

    [SerializeField] RailMover moveScript;

    [Header("Display Settings")]
    [Tooltip("A prefix to show before the number (e.g., 'Resources: ' or 'Wood: ')")]
    [SerializeField] private string textPrefix = "Resources: ";


    // 2. SUBSCRIBE to the event when this object is enabled
    private void OnEnable()
    {
        // Tell the BuildManager: "When your OnResourcesUpdated event fires,
        // also run my 'UpdateResourceText' method."
        BuildManager.OnResourcesUpdated += UpdateResourceText;
    }

    // 3. UNSUBSCRIBE when this object is disabled
    // This is critical to prevent errors and memory leaks if this UI object is ever destroyed.
    private void OnDisable()
    {
        // Tell the BuildManager to stop trying to call our method if we are disabled.
        BuildManager.OnResourcesUpdated -= UpdateResourceText;
    }

    /// <summary>
    /// This method runs *because* the event calls it. 
    /// Its signature (void function, one int parameter) MUST match the event (Action<int>).
    /// </summary>
    private void UpdateResourceText(int newTotal)
    {
        if (resourceTextDisplay != null)
        {
            // 4. Update the actual text on screen.
            resourceTextDisplay.text = textPrefix + newTotal.ToString();
        }
    }

    // Optional: This ensures the text is correct when the scene first loads,
    // in case this script loads *after* the BuildManager fires its first event in Start().
    private void Start()
    {
        if (BuildManager.Instance != null)
        {
            // Manually set the text to the current value on startup
            UpdateResourceText(BuildManager.Instance.RailResources);
        }
        else if (resourceTextDisplay != null)
        {
            // Failsafe in case the BuildManager doesn't exist yet
            resourceTextDisplay.text = textPrefix + "---";
        }
    }

    public void StartFight()
    {
        if(BuildManager.Instance != null)
        {
            if (BuildManager.Instance.placementStage)
            {
                Debug.Log("Net is connected: " + TileManager.Instance.IsOccupiedNetConnected().ToString() + " occupied list length is " + TileManager.Instance.occupiedTiles.Count);

                if (TileManager.Instance.IsOccupiedNetConnected())
                {
                    BuildManager.Instance.placementStage = false;
                    moveScript.InitializeMover();
                }
            }
        }
    }
}
