using UnityEngine;
using UnityEngine.SceneManagement; // Добавьте это!

public class MenuManager : MonoBehaviour
{
    [Header("End-Level UI")]
    [Tooltip("Drag the parent GameObject that holds your win sprite and TMP text here.")]
    [SerializeField] private GameObject levelCompleteContainer;

    // This is the flag that controls the listener in Update()
    

    private bool gameStarted = false;

    private void Start()
    {
        levelCompleteContainer.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            if (gameStarted)
            {
                SceneManager.LoadScene("PrototypeFinal");
            }
        }
    }

    public void ShowLevelCompleteScreen()
    {
        gameStarted = true;
        levelCompleteContainer.SetActive(true);
    }

}