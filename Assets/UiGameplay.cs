using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UiGameplay : MonoBehaviour
{
    public static UiGameplay Instance;

    public Canvas canvasButtonAddBranch, canvasButtonUndo, canvasButtonShuffe;
    public Image blackScreen;
    private int startOrderCanvasButton = 2;
    private int heightOrderCanvasButton = 4;

    public TutorialBird tutorialBirdPanel;

    private void Awake()
    {
        Instance = this;
    }

    public Animator uiAnimator;
    private System.Action callback;

    private bool isInTutorial = false;
    private System.Action callbackTut;
    public void OnEnable()
    {
        isInTutorial = false;
        canvasButtonAddBranch.sortingOrder = canvasButtonUndo.sortingOrder = canvasButtonShuffe.sortingOrder = startOrderCanvasButton;
        canvasButtonAddBranch.transform.GetChild(1).gameObject.SetActive(false);
        canvasButtonUndo.transform.GetChild(1).gameObject.SetActive(false);
        canvasButtonShuffe.transform.GetChild(1).gameObject.SetActive(false);

    }

    public void SetCallbackTut(System.Action call)
    {
        callbackTut = call;
    }

    public void StartTutorial(string text = "", System.Action callbackTut = null)
    {
        this.callbackTut = callbackTut;
        isInTutorial = true;
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOFade(0.7f, 0.2f).From(0);

        tutorialBirdPanel.gameObject.SetActive(true);
        tutorialBirdPanel.OpenTutorial(text);
    }

    public void EndTutorial()
    {
        if (!isInTutorial) return;
        tutorialBirdPanel.gameObject.SetActive(false);

        callbackTut?.Invoke();

        blackScreen.DOFade(0, 0.2f).OnComplete(() =>
        {
            isInTutorial = false;
            blackScreen.gameObject.SetActive(false);
            OnEnable();
        });
    }


    public void OnTutorial_EnableButton(Canvas butn)
    {
        butn.sortingOrder = heightOrderCanvasButton;
        butn.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void JoinGame()
    {
        //uiAnimator.Update(0f);
        uiAnimator.Play("uigameplay_open", -1, 0);
        //uiAnimator.enabled = false;
        //StartCoroutine(WaitForAction(0.2f, () => uiAnimator.enabled = true));
    }

    public void Close(System.Action callback = null)
    {
        this.callback = callback;
        uiAnimator.SetTrigger("close");
        if (callback != null)
        {
            StartCoroutine(WaitForAction(0.4f, callback));
        }
    }

    IEnumerator WaitForAction(float time, System.Action a)
    {
        yield return new WaitForSeconds(time);
        a?.Invoke();
    }
}
