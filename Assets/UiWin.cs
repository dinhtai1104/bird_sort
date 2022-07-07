using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class UiWin : MonoBehaviour
{
	public static UiWin Instance;
    public SkeletonGraphic[] birdSkeletonUi;
    public Animator winAnimator;

    public Button nextLevelButton;
    public ParticleSystem winEffect;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameController.Instance.pLevel++;

        PlayerPrefs.SetInt("Level", GameController.Instance.pLevel);
        //ChangeAnim(BirdAnimation.IDLE);
        nextLevelButton.interactable = true;
    }

    public void WinEffectOpen()
    {
        winEffect.Play();
        SoundManager.Instance.PlayWinSound();
    }

    public void NextLevelOnClicked()
    {
        nextLevelButton.interactable = false;
        winAnimator.SetTrigger("close");
    }

    public void EndCloseWinAnimEvent()
    {
        gameObject.SetActive(false);
        GameController.Instance.LoadLevel();
    }


    #region Event Spine

    public void EventBirdFlyToBanner()
    {
        ChangeAnim(BirdAnimation.FLY);
    }

    public void EventBirdIdleInBanner()
    {
        ChangeAnim(BirdAnimation.IDLE);
    }

    public void ChangeAnim(string anim)
    {
        foreach (SkeletonGraphic sk in birdSkeletonUi)
        {
            sk.AnimationState.SetAnimation(0, anim, true);
        }
    }
    #endregion
}
