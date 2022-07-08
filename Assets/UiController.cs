using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    public Button homeBtn, replayBtn, addBranchBtn, shuffleBtn, undoBtn, settingsBtn;
    public UiWin uiWin;
    public UiMenu menuUi;
    public UiGameplay gameplayUi;
    public UiSettings settingsUi;

    public TextMeshProUGUI levelText;


    [Header("Booster Button - Undo")]
    public GameObject countPanelUndo;
    public TextMeshProUGUI countPanelUndoText;
    public GameObject adsPanelUndo;

    [Header("Booster Button - AddBranch")]
    public GameObject countPanelAddBranch;
    public TextMeshProUGUI countPanelAddBranchText;
    public GameObject adsPanelAddBranch;

    [Header("Booster Button - Shuff")]
    public GameObject countPanelShuff;
    public TextMeshProUGUI countPanelShuffText;
    public GameObject adsPanelShuff;


    public void UpdateUndoBtn(bool interac)
    {
        undoBtn.interactable = interac;
    }

    public void ShowWin()
    {
        StopAllCoroutines();
        gameplayUi.Close();
        StartCoroutine(WaitAction(1f, () => uiWin.gameObject.SetActive(true)));
    }


    public void SettingsButtonOnClicked()
    {
        //settingsBtn.interactable = false;
        settingsUi.gameObject.SetActive(true);
    }

    public void PlayGameButtonOnClicked()
    {
        menuUi.JoinGame(() =>
        {
            gameplayUi.gameObject.SetActive(true);
            menuUi.gameObject.SetActive(false);

            GameController.Instance.LoadLevel();
            gameplayUi.JoinGame();
        });
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
        addBranchBtn.interactable = false;
        shuffleBtn.interactable = false;
        settingsBtn.interactable = false;

        //undoBtn.interactable = false;
        undoBtn.interactable = false;
        gameplayUi.JoinGame();


        InitBooster();
    }

    public void InitBooster()
    {
        Debug.Log("ADd Branch: " + GameInfo.Branch);
        UpdateBoosterBranch(GameInfo.Branch);
        UpdateBoosterShuff(GameInfo.Shuff);
        UpdateBoosterUndo(0);
    }

    public void UpdateBoosterBranch(int number)
    {
        if (number <= 0)
        {
            GameInfo.Branch = 0;
            countPanelAddBranch.SetActive(false);
            adsPanelAddBranch.SetActive(true);
        } else
        {
            countPanelAddBranch.SetActive(true);
            adsPanelAddBranch.SetActive(false);
            countPanelAddBranchText.text = number.ToString();
        }
    }

    public void UpdateBoosterUndo(int number)
    {
        if (number <= 0)
        {
            countPanelUndo.SetActive(false);
        }
        else
        {
            countPanelUndo.SetActive(true);
            adsPanelUndo.SetActive(false);
            countPanelUndoText.text = number.ToString();
        }
    }

    public void UpdateBoosterShuff(int number)
    {
        if (number <= 0)
        {
            GameInfo.Shuff = 0;
            adsPanelShuff.SetActive(true);
            countPanelShuff.SetActive(false);
        }
        else
        {
            adsPanelShuff.SetActive(false);
            countPanelShuff.SetActive(true);
            countPanelShuffText.text = number.ToString();
        }
    }

    public void DisableButtonsJoinGame()
    {
        addBranchBtn.interactable = false;
        shuffleBtn.interactable = false;
        undoBtn.interactable = false;

    }

    public void EnableButtonJoinGame()
    {
        addBranchBtn.interactable = true;
        shuffleBtn.interactable = true;

        undoBtn.interactable = UndoSystem.Instance.CheckUndo();
        if (GameController.Instance.levelManger.getTotalBranch() >= 12)
        {
            addBranchBtn.interactable = false;
        }
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
            menuUi.gameObject.SetActive(true);
        });

    }

    public void AddNewBranchButtonOnClicked()
    {
        if (GameInfo.Branch <= 0)
        {

            return;
        }
        addBranchBtn.interactable = false;
        if (!PlayerPrefs.HasKey("Tut_addBranch") && GameInfo.Level == 2)
        {
            UiGameplay.Instance.SetCallbackTut(() =>
            {
                GameController.Instance.AddNewBranch();
                PlayerPrefs.SetInt("Tut_addBranch", 1);
                //addBranchBtn.interactable = GameController.Instance.levelManger.getTotalBranch() < 12;
                addBranchBtn.interactable = true;
            });
            TutorialBird.Instance?.EndTutorial();
        }
        else 
        {
            addBranchBtn.interactable = true;
            GameController.Instance.AddNewBranch();
        }

    }

    public void ShuffleButtonOnClicked()
    {
        //shuffleBtn.interactable = false;
        if (GameInfo.Shuff <= 0)
        {

            return;
        }
        GameController.Instance.ShuffleBird();
    }

    public void UndoButtonOnClicked()
    {
        undoBtn.interactable = false;
        if (!PlayerPrefs.HasKey("Tut_undo") && GameInfo.Level == 3) 
        {
            TutorialBird.Instance?.EndTutorial();
            UiGameplay.Instance.SetCallbackTut(() =>
            {
                PlayerPrefs.SetInt("Tut_undo", 1);
                GameController.Instance.Undo();
                UpdateUndoBtn(UndoSystem.Instance.CheckUndo());
            });
        } else
        {
            GameController.Instance.Undo();
            UpdateUndoBtn(UndoSystem.Instance.CheckUndo());
        }
    }

    public void SetTextLevel(int pLevel)
    {
        levelText.text = "Level " + pLevel.ToString("00");
    }
}
