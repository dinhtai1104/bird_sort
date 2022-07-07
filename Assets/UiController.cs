using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class UiController : MonoBehaviour
{
    private static UiController _instance;
    public static UiController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<UiController>();
            }
            return _instance;
        }
    }

    public Button homeBtn, replayBtn, addBranchBtn, shuffleBtn, undoBtn;
    public UiWin uiWin;
    public GameObject menuUi;
    public UiGameplay gameplayUi;
    public void UpdateUndoBtn(bool interac)
    {
        undoBtn.interactable = interac;
    }

    public void ShowWin()
    {
        StopAllCoroutines();
        gameplayUi.Close();
        StartCoroutine(WaitAction(1f, () => uiWin.gameObject.SetActive(true)));
        //uiWin.gameObject.SetActive(true);
    }


    public void PlayGameButtonOnClicked()
    {
        gameplayUi.gameObject.SetActive(true);
        menuUi.SetActive(false);

        GameController.Instance.LoadLevel();
        gameplayUi.JoinGame();
    }
    IEnumerator WaitAction(float t, System.Action ac) 
    {
        yield return new WaitForSeconds(t);
        ac?.Invoke();
    }


    public void JoinGame()
    {
        homeBtn.interactable = true;
        replayBtn.interactable = true;
        addBranchBtn.interactable = true;
        shuffleBtn.interactable = true;

        undoBtn.interactable = false;
        gameplayUi.JoinGame();

    }

    public void ReplayButtonOnClicked()
    {
        //replayBtn.interactable = false;
        GameController.Instance.ReplayGame();
    }

    public void HomeButtonOnClicked()
    {
        //homeBtn.interactable = false;
        //gameplayUi.SetActive(false);
        //menuUi.SetActive(true);
        //PoolingSystem.Instance.ResetPool();
        gameplayUi.Close(()=>
        {
            PoolingSystem.Instance.ResetPool();
            gameplayUi.gameObject.SetActive(false);
            menuUi.SetActive(true);
        });

    }

    public void AddNewBranchButtonOnClicked()
    {
        //addBranchBtn.interactable = false;
        GameController.Instance.AddNewBranch();
    }

    public void ShuffleButtonOnClicked()
    {
        //shuffleBtn.interactable = false;
        GameController.Instance.ShuffleBird();
    }

    public void UndoButtonOnClicked()
    {
        GameController.Instance.Undo();
    }
}
