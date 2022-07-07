using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGameplay : MonoBehaviour
{
    public static UiGameplay Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Animator uiAnimator;
    private System.Action callback;

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
