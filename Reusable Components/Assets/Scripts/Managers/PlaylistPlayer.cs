using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaylistPlayer : MonoBehaviour
{
    // Global reference so other systems can control the music playlist.
    public static PlaylistPlayer Instance;

    // List of background music tracks that can be played in random order.
    [SerializeField] private List<AudioClip> musics;
    [SerializeField] private AudioSource audioSource;

    // UI references used to display the currently playing music track.
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Animator animator;

    // Invokes the next song when the current track has finished.
    private Action onMusicEnd;

    // Stores the index of the currently selected music track.
    int index;

    private void Awake()
    {
        // Keep one playlist player alive while changing scenes.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Cache the AudioSource attached to this GameObject.
        audioSource = GetComponent<AudioSource>();

        // Start the playlist from a random track.
        index = UnityEngine.Random.Range(0, musics.Count);
    }

    private void Start()
    {
        // Move to the next song whenever the current song has ended.
        onMusicEnd += NextMusic;
    }

    private void Update()
    {
        // Continue the playlist when no audio is currently playing.
        if (!audioSource.isPlaying) onMusicEnd?.Invoke();
    }

    // Plays the currently selected music track and updates the music UI.
    void PlayMusic(AudioClip newmusic)
    {
        // Do not interrupt a track that is already playing.
        if (audioSource.isPlaying) return;

        // Use the current playlist index to choose the next clip.
        newmusic = musics[index];
        audioSource.clip = newmusic;
        audioSource.volume = 0.11f;
        audioSource.Play();

        // Show the active song name and play its UI animation.
        musicText.text = newmusic.name;
        animator.SetTrigger("Appear");
    }

    // Selects the next track in the playlist and starts playing it.
    public void NextMusic()
    {
        index++;

        // Return to the first track after reaching the end of the playlist.
        if (index == musics.Count)
        {
            index = 0;
        }

        PlayMusic(musics[index]);
    }

    // Fades the current music track out before stopping it.
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine(2f));
    }

    // Gradually lowers the music volume over the provided duration.
    IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        // Reduce the audio volume smoothly until the fade duration has finished.
        while (timer < duration)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);

            yield return null;
        }

        // Stop automatic playlist progression after the music has faded out.
        onMusicEnd -= NextMusic;
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}