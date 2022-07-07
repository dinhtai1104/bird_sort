using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;
/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class UiMenu : MonoBehaviour
{
	public static UiMenu Instance;
    private void Awake()
    {
        Instance = this;
    }

    public SkeletonGraphic skeletonGraphic;
    public Animator animator;
    public Button playBtn;

    private System.Action callback;
    public void JoinGame(System.Action ac = null)
    {
        playBtn.interactable = false;
        animator.SetTrigger("close");
        callback = ac;
    }

    private void OnEnable()
    {
        playBtn.interactable = true;
    }
    public void ChangeAnim(string ani)
    {
        skeletonGraphic.AnimationState.SetAnimation(0, ani, true);
    }

    public void StartEndEvent()
    {
        callback?.Invoke();
        //gameObject.SetActive(false);
        //UiController.Instance.JoinGame();
    }

    public void EndEndEvent()
    {
    }
}
