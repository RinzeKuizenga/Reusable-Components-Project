using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;


public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();


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

    }


    public void PlaySFX(int index, float volume)
    {
        if (audioClips[index] == null) return;
        audioSource.PlayOneShot(audioClips[index], volume);
    }
}
