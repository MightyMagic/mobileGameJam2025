using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static bool IsMuted { get; private set; } = false;

    public void ToggleSound()
    {
        IsMuted = !IsMuted;
        AudioListener.pause = IsMuted;
        Debug.Log("Звук: " + (IsMuted ? "Выключен" : "Включен"));
    }
}