using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    // Ссылки на иконки, которые вы перетащите в Unity Inspector
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;

    private Image buttonImage;
    private bool isSoundOn = true;

    void Start()
    {
        // Получаем компонент Image с кнопки
        buttonImage = GetComponent<Image>();
        // Устанавливаем иконку по умолчанию
        buttonImage.sprite = soundOnIcon;

        // При запуске игры звук включен
        AudioListener.volume = 1f;
    }

    // Этот метод будет вызван при нажатии на кнопку
    public void ToggleSound()
    {
        // Меняем состояние звука на противоположное
        isSoundOn = !isSoundOn;

        // Если звук включен
        if (isSoundOn)
        {
            AudioListener.volume = 1f; // Устанавливаем громкость на максимум
            buttonImage.sprite = soundOnIcon; // Меняем иконку на "включено"
        }
        // Если звук выключен
        else
        {
            AudioListener.volume = 0f; // Устанавливаем громкость на ноль
            buttonImage.sprite = soundOffIcon; // Меняем иконку на "выключено"
        }
    }
}