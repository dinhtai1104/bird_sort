using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class SoundManager : MonoBehaviour
{
	private static SoundManager _instance;
	public static SoundManager Instance
    {
		get
        {
			if (!_instance)
            {
                _instance = FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }
    public AudioSource sfxSource;
    public AudioSource tweetSource;
    public AudioSource birdSource;
    public AudioSource bgmSource;

    public AudioClip[] tweetBirds;
    public AudioClip[] releaseBirds;

    public AudioClip winEffect;
    public AudioClip buttonClick;

    public void OnEnable()
    {
        UpdateMusic();
    }

    public void UpdateMusic()
    {
        if (!GameInfo.Music) 
        {
            bgmSource.DOFade(0, 0.4f).OnComplete(()=>
            {
                bgmSource.mute = true;
            });
        } else
        {
            if (bgmSource.mute)
            {
                bgmSource.mute = false;
                bgmSource.DOFade(1, 0.4f);
            }
        }
    }

    public void PlaySFXSound(AudioClip c)
    {
        if (GameInfo.Sound == false) return;
        sfxSource.PlayOneShot(c);
    }

    public void PlayTweetSound()
    {
        if (GameInfo.Sound == false) return;

        tweetSource.PlayOneShot(tweetBirds[Random.Range(0, tweetBirds.Length)]);
    }
    public void PlayReleaseSound()
    {
        if (GameInfo.Sound == false) return;

        birdSource.PlayOneShot(releaseBirds[Random.Range(0, releaseBirds.Length)]);
    }
    public void PlayWinSound()
    {
        if (GameInfo.Sound == false) return;

        sfxSource.PlayOneShot(winEffect);
    }
}
