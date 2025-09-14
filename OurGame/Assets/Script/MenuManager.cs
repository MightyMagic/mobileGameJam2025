using UnityEngine;
using UnityEngine.SceneManagement; // Добавьте это!

public class MenuManager : MonoBehaviour
{
    [Header("End-Level UI")]
    [Tooltip("Drag the parent GameObject that holds your win sprite and TMP text here.")]
    [SerializeField] private GameObject levelCompleteContainer;

    // This is the flag that controls the listener in Update()
    private bool isListeningForNextSceneTap = false;
    // --- END NEW LOGIC ---


    public void StartGame()
    {
        // Загружаем сцену по имени. Убедитесь, что имя 'GameScene'
        // совпадает с именем вашей игровой сцены.
        SceneManager.LoadScene("PrototypeFinal");
    }

    private void Update()
    {
        // If our flag is false, we are not listening, so do nothing.
        if (!isListeningForNextSceneTap)
        {
            return;
        }

        // If the flag IS true, listen for the input.
        // Input.GetMouseButtonDown(0) works for BOTH a mouse click AND a screen touch.
        if (Input.GetMouseButtonDown(0))
        {
            // Input detected!

            // 1. IMPORTANT: Set the flag back to false immediately.
            // This stops the Update loop from listening anymore and prevents
            // this code from accidentally running twice.
            isListeningForNextSceneTap = false;

            // 2. (Optional but good practice) Hide the container you just showed.
            if (levelCompleteContainer != null)
            {
                levelCompleteContainer.SetActive(false);
            }

            // 3. Load the scene you requested.
            SceneManager.LoadScene("PrototypeFinal");
        }
    }

    public void ShowLevelCompleteScreen()
    {
        // 1. Activate the UI parent object (which contains your sprite and text).
        // Make sure this object is set to INACTIVE by default in the scene.
        if (levelCompleteContainer != null)
        {
            levelCompleteContainer.SetActive(true);
        }
        else
        {
            Debug.LogWarning("LevelCompleteContainer is not assigned in the GameManager Inspector!");
        }

        // 2. Set the flag to TRUE. This "arms" the Update() loop,
        // which will now start listening for the tap.
        isListeningForNextSceneTap = true;
    }
    // --- END NEW FUNCTION ---

}