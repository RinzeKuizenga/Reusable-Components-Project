using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlaylistPlayer : MonoBehaviour
{
    public static PlaylistPlayer Instance;

    [SerializeField] private List<AudioClip> musics;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Animator animator;

    private Action OnMusicEnd;


    private Action onMusicEnd;
    int index;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        index = UnityEngine.Random.Range(0, musics.Count);
    }

    private void Start()
    {
        onMusicEnd += NextMusic;
    }

    private void Update()
    {
        if (!audioSource.isPlaying) onMusicEnd?.Invoke();
    }

    void PlayMusic(AudioClip newmusic)
    {
        if (audioSource.isPlaying) return;
        newmusic = musics[index];
        audioSource.clip = newmusic;
        audioSource.volume = 0.11f;
        audioSource.Play(); 

        musicText.text = newmusic.name;
        animator.SetTrigger("Appear");
    }

    public void NextMusic()
    {
        index++;

        if (index == musics.Count)
        {
            index = 0;
        }

        PlayMusic(musics[index]);
    }


    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine(2f)); 
    }

    IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);

            yield return null;
        }

        onMusicEnd -= NextMusic;
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
