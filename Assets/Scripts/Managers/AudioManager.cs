using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    AudioClip MenuSound;

    [SerializeField]
    AudioClip ButtonSound;

    [SerializeField]
    AudioClip PlayerShootSound;

    [SerializeField]
    AudioClip EnemyHitSound;

    [SerializeField]
    AudioClip PlayerHitSound;

    [SerializeField]
    AudioClip PlayerDieSound;

    [SerializeField]
    AudioClip[] ClickSounds = new AudioClip[4];

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        audioSource = this.GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        audioSource.Play();
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayMenuSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(MenuSound, 1f);
    }

    public void PlayButtonSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(ButtonSound, 1f);
    }

    public void PlayPlayerShootSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerShootSound, .75f);
    }

    public void PlayEnemyHitSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(EnemyHitSound, .5f);
    }

    public void PlayPlayerHitSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerHitSound, .5f);
    }

    public void PlayPlayerDieSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerDieSound, .5f);
    }

    public void PlayClickSound()
    {
        if (Globals.AudioOn)
        {
            int num = Random.Range(0, ClickSounds.Length - 1);
            audioSource.PlayOneShot(ClickSounds[num], 1f);
        }
    }
}
