using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;
using TMPro;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class TutorialBird : MonoBehaviour
{
	public static TutorialBird Instance;

    private void Awake()
    {
		Instance = this;
    }

    public Button IknowButton;
	public Animator birdTutorialAnimator;

	public SkeletonGraphic birdTutorialSpine;
	public TextMeshProUGUI textBoxDialogText;
	public RectTransform textBoxDialogGO;

	private string textTutorial;

	private System.Action callback;
	public void OpenTutorial(string text)
    {
		isEnding = false;
		birdTutorialAnimator.gameObject.SetActive(true);
		this.textTutorial = text;
		textBoxDialogText.text = "";
		IknowButton.gameObject.SetActive(false);

	}


	IEnumerator DoTextTweening(string text)
    {

		WaitForSeconds wfs = new WaitForSeconds(0.03f);
		textBoxDialogText.text = "";
		string tmp = "";
		for (int i = 0; i < text.Length; i++) 
		{
			tmp += text[i];
			textBoxDialogText.text = tmp;
			yield return wfs;
		}
		textBoxDialogText.text = text;
		IknowButton.gameObject.SetActive(true);

	}

	public void StartOpenTextBoxEvent()
    {
		StartCoroutine(DoTextTweening(this.textTutorial));
	}

	public void ChangeTextImmediately(string text)
    {
		textBoxDialogText.text = text;
	}
	private bool isEnding = false;
	public void EndTutorial()
    {

		//if (isEnding) return;

		ChangeTextImmediately("Good job");
		//StopAllCoroutines();
		if (gameObject.activeInHierarchy)
		{
			if (isEnding) return;
			StopAllCoroutines();
			IknowButton.gameObject.SetActive(false);

			isEnding = true;
			StartCoroutine(WaitForAction());
		}
    }

	IEnumerator WaitForAction()
    {
		yield return new WaitForSeconds(0.5f);
		birdTutorialAnimator.SetTrigger("close");
		isEnding = false;

	}

	public void EndTutEvent()
    {
		callback?.Invoke();
    }

	public void ChangeAnim(string ani)
    {
		birdTutorialSpine.AnimationState.SetAnimation(0, ani, true);
    }
}
