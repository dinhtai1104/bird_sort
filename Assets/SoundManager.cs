using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void PlaySFXSound(AudioClip c)
    {
        sfxSource.PlayOneShot(c);
    }

    public void PlayTweetSound()
    {
        tweetSource.PlayOneShot(tweetBirds[Random.Range(0, tweetBirds.Length)]);
    }
    public void PlayReleaseSound()
    {
        birdSource.PlayOneShot(releaseBirds[Random.Range(0, releaseBirds.Length)]);
    }
    public void PlayWinSound()
    {
        sfxSource.PlayOneShot(winEffect);
    }
}
