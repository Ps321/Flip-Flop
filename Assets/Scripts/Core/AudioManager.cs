using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private List<Sound> sounds;
    private Dictionary<AudioType, AudioClip> soundMap;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        soundMap = new Dictionary<AudioType, AudioClip>();
        foreach (var sound in sounds)
        {
            soundMap[sound.type] = sound.clip;
        }
    }

    public void Play(AudioType type)
    {
        if (soundMap.TryGetValue(type, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
