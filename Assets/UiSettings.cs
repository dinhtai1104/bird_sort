using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class UiSettings : MonoBehaviour
{
	public Button soundBtn, musicBtn, vibrationBtn;
	public Image soundImg, musicImg, vibrationImg;
	public Sprite soundOn, soundOff, musicOn, musicOff, vibrationOn, vibrationOff;

    private Animator animator;
    private void OnEnable()
    {
        Init();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        soundBtn.onClick.AddListener(() =>
        {
            GameInfo.Sound = !GameInfo.Sound;
            soundImg.sprite = GameInfo.Sound ? soundOn : soundOff;
            soundImg.SetNativeSize();
        });

        musicBtn.onClick.AddListener(() =>
        {
            GameInfo.Music = !GameInfo.Music;
            musicImg.sprite = GameInfo.Music ? musicOn : musicOff;
            musicImg.SetNativeSize();

            SoundManager.Instance.UpdateMusic();
        });

        vibrationBtn.onClick.AddListener(() =>
        {
            GameInfo.Vibration = !GameInfo.Vibration;
            vibrationImg.sprite = GameInfo.Vibration ? vibrationOn : vibrationOff;
            vibrationImg.SetNativeSize();
        });
    }

    public void Init()
    {
        soundImg.sprite = GameInfo.Sound ? soundOn : soundOff;
        musicImg.sprite = GameInfo.Music ? musicOn : musicOff;
        vibrationImg.sprite = GameInfo.Vibration ? vibrationOn : vibrationOff;

        soundImg.SetNativeSize();
        musicImg.SetNativeSize();
        vibrationImg.SetNativeSize();
    }

    public void OnClose()
    {
        animator.SetTrigger("close");
    }

    public void SetDisActive()
    {
        gameObject.SetActive(false);
    }
}
