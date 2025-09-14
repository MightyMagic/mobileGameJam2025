using UnityEngine;

public class SwayAnimation : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayMagnitude = 0.05f; // Насколько сильно будет покачиваться
    public float swaySpeed = 1f;        // Скорость покачивания
    public bool isVertical = true;     // Покачивание вверх/вниз

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Вычисляем смещение на основе синусоидальной волны
        float offset = Mathf.Sin(Time.time * swaySpeed) * swayMagnitude;

        // Применяем смещение к локальной позиции
        Vector3 newPosition = initialPosition;
        if (isVertical)
        {
            newPosition.y += offset;
        }
        else
        {
            newPosition.x += offset;
        }

        transform.localPosition = newPosition;
    }
}