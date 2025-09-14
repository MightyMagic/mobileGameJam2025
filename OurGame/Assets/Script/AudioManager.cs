using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Required for [System.Serializable]

/*
 * This enum list remains the same.
 */
public enum SoundType
{
    FlameThrower,
    Saw,
    MachineGun,
    DefaultGun,
    Rocket,
    ZombieDeath,
    GameWin
}

/*
 * This helper class now contains its OWN cooldown setting.
 */
[System.Serializable]
public class Sound
{
    public string name;
    public SoundType type;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.8f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;

    // --- CHANGED (1 of 4) ---
    // Added a specific cooldown field for this sound.
    // This allows each sound to have a unique cooldown.
    [Range(0f, 10f)] // Creates a handy slider in the Inspector
    public float cooldown = 1.0f;

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public float lastTimePlayed;
}


/*
 * The main manager script.
 */
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sound Definitions")]
    [SerializeField]
    private List<Sound> sounds;

    // --- CHANGED (2 of 4) ---
    // The "globalSoundCooldown" field is GONE.
    // It is no longer needed since each sound manages its own cooldown.

    private Dictionary<SoundType, Sound> soundDictionary;

    void Awake()
    {
        // --- Setup Singleton (unchanged) ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        // --- Create Dictionary and AudioSources ---
        soundDictionary = new Dictionary<SoundType, Sound>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;

            // --- CHANGED (3 of 4) ---
            // The timer is now initialized using the sound's OWN specific cooldown.
            s.lastTimePlayed = -s.cooldown;

            // --- Add to dictionary (unchanged) ---
            if (soundDictionary.ContainsKey(s.type))
            {
                Debug.LogWarning($"Duplicate SoundType found in AudioManager: {s.type}. Skipping.");
            }
            else
            {
                soundDictionary.Add(s.type, s);
            }
        }
    }

    /// <summary>
    /// Plays a sound based on its enum type, honoring its INDIVIDUAL cooldown.
    /// </summary>
    public void PlaySound(SoundType type)
    {
        if (soundDictionary.TryGetValue(type, out Sound s))
        {
            // --- CHANGED (4 of 4) ---
            // The cooldown check now compares against the sound's OWN cooldown property ("s.cooldown")
            // instead of the old global variable.
            if (Time.time >= s.lastTimePlayed + s.cooldown)
            {
                s.source.Play();
                s.lastTimePlayed = Time.time;
            }
            // else: This specific sound is on cooldown. Do nothing.
        }
        else
        {
            Debug.LogWarning($"AudioManager: Sound type not found in dictionary: {type}");
        }
    }

    /// <summary>
    /// Stops a currently playing sound based on its enum type.
    /// </summary>
    public void StopSound(SoundType type)
    {
        if (soundDictionary.TryGetValue(type, out Sound s))
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning($"AudioManager: Sound type not found in dictionary: {type}");
        }
    }
}