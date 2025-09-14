using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The EXACT name of your Main Menu scene file.")]
    [SerializeField] private string mainMenuScene = "MainMenu";

    [Tooltip("The EXACT name of your first/main Game Level scene file.")]
    [SerializeField] private string gameLevelScene = "GameLevel";

    [Tooltip("The EXACT name of your Game Over scene file.")]
    [SerializeField] private string gameOverScene = "GameOver";

    public static GameOver Instance { get; private set; }

    private void Awake()
    {
        // --- This is the Singleton Pattern logic ---

        // Check if an 'Instance' already exists
        if (Instance == null)
        {
            // If not, this object is the Instance
            Instance = this;

            // Make this GameObject persist when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an Instance DOES already exist, destroy this new duplicate.
            // This happens when you reload a scene that contains the prefab/object.
            Destroy(gameObject);
        }
    }

    // --- Public Scene Loading Functions ---
    // These functions can be called from ANY other script (or UI Button).

    /// <summary>
    /// Loads the main game level.
    /// </summary>
    public void LaunchGameLevel()
    {
        // Add any logic needed BEFORE loading the level (like resetting score, player lives, etc.)
        // Debug.Log("Starting game level...");
        SceneManager.LoadScene(gameLevelScene);
    }

    /// <summary>
    /// Loads the Game Over screen.
    /// </summary>
    public void ShowGameOverScreen()
    {
        // Debug.Log("Game Over!");
        SceneManager.LoadScene(gameOverScene);
    }

    /// <summary>
    /// Loads the Main Menu.
    /// </summary>
    public void LoadMainMenu()
    {
        // Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene(mainMenuScene);
    }

    /// <summary>
    /// Closes the application. (This only works in a built game, not the Editor).
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
