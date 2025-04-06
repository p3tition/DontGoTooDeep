using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Range(0f, 2f)]
    [SerializeField] private float musicVolume = 1f;
    [Range(0f, 2f)]
    [SerializeField] private float sfxVolume = 1f;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip sonarSound;
    [SerializeField] private AudioClip destroyShieldSound;
    [SerializeField] private AudioClip[] soundTracks;
    
    private AudioSource sfxSource;
    private AudioSource musicSource;
    private List<AudioClip> unplayedTracks = new List<AudioClip>();
    private AudioClip currentTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = false;
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
        ResetSoundtrackList();
        PlayNextTrack();
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void ResetSoundtrackList()
    {
        unplayedTracks = new List<AudioClip>(soundTracks);
        ShuffleList(unplayedTracks);
    }

    private void ShuffleList(List<AudioClip> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    private void PlayNextTrack()
    {
        if (unplayedTracks.Count == 0)
        {
            ResetSoundtrackList();
        }

        currentTrack = unplayedTracks[0];
        unplayedTracks.RemoveAt(0);
        musicSource.clip = currentTrack;
        musicSource.Play();
    }

    public void StopSoundtrack()
    {
        musicSource.Stop();
    }

    public void PlayNextSoundtrack()
    {
        musicSource.Stop();
        PlayNextTrack();
    }

    public void PlayDeathSound() => sfxSource.PlayOneShot(deathSound);
    public void PlayPowerUpSound() => sfxSource.PlayOneShot(powerUpSound);
    public void PlaySonarSound() => sfxSource.PlayOneShot(sonarSound);
    public void PlayDestroyShieldSound() => sfxSource.PlayOneShot(destroyShieldSound);
}