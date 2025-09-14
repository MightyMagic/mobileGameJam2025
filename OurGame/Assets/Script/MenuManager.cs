using UnityEngine;
using UnityEngine.SceneManagement; // Добавьте это!

public class MenuManager : MonoBehaviour
{
    // Этот метод будет вызван при нажатии на кнопку
    public void StartGame()
    {
        // Загружаем сцену по имени. Убедитесь, что имя 'GameScene'
        // совпадает с именем вашей игровой сцены.
        SceneManager.LoadScene("MainGame");
    }
}