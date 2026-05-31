using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;


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
        PlayMusic(musics[index]);
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
        newmusic = musics[index];
        audioSource.clip = newmusic;
        audioSource.Play(); 

        musicText.text = newmusic.name;
        animator.SetTrigger("Appear");
    }

    void NextMusic()
    {
        index++;

        if (index == musics.Count)
        {
            index = 0;
        }

        PlayMusic(musics[index]);
    }
}
