using System;
using TMPro;
using UnityEngine;

/// <summary>
/// A central singleton manager to control the game state, specifically managing
/// the cycle between a Build Phase and an Action Phase.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static GameManager Instance { get; private set; }

    // --- STATE DEFINITION ---
    // The GameState enum is updated to include the two new phases.
    public enum GameState
    {
        MainMenu,
        Starting,     // For initial "Get Ready!" countdowns
        BuildPhase,   // Player can build, place items, upgrade
        ActionPhase,  // Enemies spawn, action happens
        Paused,
        GameOver,
        Victory
    }

    public GameState CurrentState { get; private set; }
    private GameState stateBeforePause;

    // --- STATE EVENTS ---
    // We now have specific events for the build and action phases.

    /// <summary>
    /// Generic event that fires every time any state change occurs.
    /// Passes the new state as a parameter.
    /// </summary>
    public static event Action<GameState> OnStateChanged;

    public static event Action OnMainMenu;
    public static event Action OnGameStart;        // Fires when state changes to "Starting"
    public static event Action OnBuildPhaseStart;  // NEW: Fires when Build Phase begins
    public static event Action OnActionPhaseStart; // NEW: Fires when Action Phase begins
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    public static event Action OnVictory;

    //Choices 
    private static int choicePoints = 0;
    private static TextMeshProUGUI choicePointsText;


    private void Awake()
    {
        // Singleton Implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Set the initial state
        //ChangeState(GameState.MainMenu);

        StartFirstBuildPhase();
    }

    /// <summary>
    /// The main engine of the state machine. Call this to change the game state.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;

        // The core switch statement is updated with the new states.
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                OnMainMenu?.Invoke();
                break;

            case GameState.Starting:
                Time.timeScale = 1f;
                OnGameStart?.Invoke(); // A countdown script listens for this
                break;

            // This is the new state replacing "Playing"
            case GameState.BuildPhase:
                Time.timeScale = 1f;
                OnBuildPhaseStart?.Invoke();
                break;

            // This is the other half of the new loop
            case GameState.ActionPhase:
                Time.timeScale = 1f;
                OnActionPhaseStart?.Invoke();
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                OnGamePaused?.Invoke();
                break;

            case GameState.GameOver:
                Time.timeScale = 1f;
                OnGameOver?.Invoke();
                break;

            case GameState.Victory:
                Time.timeScale = 1f;
                OnVictory?.Invoke();
                break;

            default:
                Debug.LogWarning($"GameManager: State {newState} is not handled.");
                break;
        }

        OnStateChanged?.Invoke(newState);
        Debug.Log($"New Game State: {newState}");
    }


    // --- PUBLIC API METHODS ---
    // Updated public functions to control the new game loop.

    /// <summary>
    /// Called by Main Menu UI. Moves to the "Get Ready" state.
    /// </summary>
    public void StartGame()
    {
        ChangeState(GameState.Starting);
    }

    /// <summary>
    /// Called by the "Countdown" script after it finishes. Begins the first build phase.
    /// </summary>
    public void StartFirstBuildPhase()
    {
        ChangeState(GameState.BuildPhase);
    }

    /// <summary>
    /// Called by a UI Button ("Start Wave") or timer during the Build Phase.
    /// </summary>
    public void BeginActionPhase()
    {
        if (CurrentState == GameState.BuildPhase)
        {
            ChangeState(GameState.ActionPhase);
        }
    }

    /// <summary>
    /// Called by an enemy spawner or wave manager when the action phase is over (e.g., all enemies defeated).
    /// </summary>
    public void BeginBuildPhase()
    {
        // Can only return to build phase from an action phase.
        if (CurrentState == GameState.ActionPhase)
        {
            ChangeState(GameState.BuildPhase);
            // You might also fire an "OnWaveComplete" event here before the state change.
        }
    }

    /// <summary>
    /// Call this when the player wins (e.g., completes the final wave).
    /// </summary>
    public void TriggerVictory()
    {
        // Can only win from one of the active gameplay states
        if (CurrentState == GameState.ActionPhase || CurrentState == GameState.BuildPhase)
        {
            ChangeState(GameState.Victory);
        }
    }

    /// <summary>
    /// Call this when the player loses (e.g., base health reaches 0).
    /// </summary>
    public void TriggerGameOver()
    {
        // Can lose from either gameplay state (e.g., running out of money in build phase, or dying in action phase)
        if (CurrentState == GameState.ActionPhase || CurrentState == GameState.BuildPhase)
        {
            ChangeState(GameState.GameOver);
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        // Can only pause during the active gameplay states.
        if (CurrentState == GameState.ActionPhase || CurrentState == GameState.BuildPhase)
        {
            stateBeforePause = CurrentState;
            ChangeState(GameState.Paused);
        }
    }

    /// <summary>
    /// Resumes the game from pause.
    /// </summary>
    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            OnGameResumed?.Invoke();
            // This correctly returns the player to whatever phase they were in (Build or Action).
            ChangeState(stateBeforePause);
        }
    }

    public void ReturnToMenu()
    {
        ChangeState(GameState.MainMenu);
        // Add scene loading logic here:
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuSceneName");
    }

    public static void AddChoicePoints(int amount)
    {
        choicePoints += amount;
    }

    private static void UpdateChoicePointsText()
    {
        if (choicePointsText != null)
        {
            choicePointsText.text = "Очки выбора: " + choicePoints;
        }
    }
}
