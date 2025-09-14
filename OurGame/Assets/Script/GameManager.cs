using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static GameManager Instance { get; private set; }

    // --- STATE DEFINITION ---
    public enum GameState
    {
        MainMenu,
        Starting,
        BuildPhase,
        ActionPhase,
        Paused,
        GameOver,
        Victory
    }

    public GameState CurrentState { get; private set; }
    private GameState stateBeforePause;

    // --- STATE EVENTS ---
    public static event Action<GameState> OnStateChanged;
    public static event Action OnGameStart;
    public static event Action OnBuildPhaseStart;
    public static event Action OnActionPhaseStart;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    // --- CHOICE POINTS ---
    private int choicePoints = 0;
    private TextMeshProUGUI choicePointsText;
    public int ChoicePoints => choicePoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.BuildPhase);
    }

    // --- Public API ---
    public void BeginBuildPhase()
    {
        ChangeState(GameState.BuildPhase);
    }

    public void BeginActionPhase()
    {
        ChangeState(GameState.ActionPhase);
    }

    public void AddChoicePoints(int amount)
    {
        choicePoints += amount;
        UpdateChoicePointsText();
    }

    // НОВЫЙ МЕТОД ДЛЯ УПРАВЛЕНИЯ ЦИКЛОМ
    public void BeginUpgradeCheck()
    {
        Debug.Log("GameManager: Запущена проверка на апгрейды.");
        // Сначала ставим игру на паузу, чтобы выдать карточки
        stateBeforePause = CurrentState;
        ChangeState(GameState.Paused);

        // Затем вызываем проверку в UpgradeManager
        UpgradeManager.Instance.CheckForUpgrades();
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.ActionPhase || CurrentState == GameState.BuildPhase)
        {
            stateBeforePause = CurrentState;
            ChangeState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(stateBeforePause);
        }
    }

    // --- Private Methods ---
    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
        Debug.Log($"Game state changed to: {newState}");

        switch (newState)
        {
            case GameState.BuildPhase:
                Time.timeScale = 1f;
                OnBuildPhaseStart?.Invoke();
                break;
            case GameState.ActionPhase:
                Time.timeScale = 1f;
                OnActionPhaseStart?.Invoke();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                OnGamePaused?.Invoke();
                break;
            case GameState.MainMenu:
            case GameState.GameOver:
            case GameState.Victory:
                Time.timeScale = 1f;
                break;
        }
    }

    private void UpdateChoicePointsText()
    {
        if (choicePointsText != null)
        {
            choicePointsText.text = choicePoints.ToString();
        }
    }
}