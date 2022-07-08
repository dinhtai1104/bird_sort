using Action = System.Action;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public enum STATE
{
    MOVING, NONE
}
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public BranchController currentTouchBranch;

    public LayerMask branchLayerMask;
    public LevelManager levelManger;
    public STATE state = STATE.NONE;
    public Stack<Action> listMovement = new Stack<Action>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }


    private void Update()
    {
        //if (state == STATE.MOVING) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (IsTouchUI()) return;
            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(mPos, Vector3.zero, Mathf.Infinity, branchLayerMask);
            if (hitInfo.collider != null)
            {
                BranchController branch = hitInfo.collider.GetComponent<BranchController>();

                if (GameInfo.Level == 1 && !PlayerPrefs.HasKey("Tut_lv1"))
                {
                    if (branchTut)
                    {
                        HandTutorialController.Instance.OpenTutorial();

                        if (branch == branchTut)
                        {
                            branchTut = levelManger.GetBranch(1);
                        }
                        else
                        {
                            branchTut = levelManger.GetBranch(0);
                        }
                    }
                    HandTutorialController.Instance.ChangePosition(branchTut.listSlotPosBirds[2].transform.position);
                }
                if (branch != null)
                {

                    if (currentTouchBranch != null)
                    {
                        currentTouchBranch.HighlightBird(false);
                    }
                    if (currentTouchBranch == null)
                    {

                        if (!branch.IsEmpty() && !branch.CheckFullBranchWithSameBird() && branch.IsReady)
                        {
                            SoundManager.Instance.PlayTweetSound();

                            Debug.Log("Set new branch");
                            currentTouchBranch = branch;
                            branch.HighlightBird(true);
                        }
                    } else
                    {

                        if (branch == currentTouchBranch)
                        {
                            HandTutorialController.Instance.EndTutorial();

                            branch.HighlightBird(false);
                            currentTouchBranch.HighlightBird(false);
                            Debug.Log("Touch Current branch");
                            currentTouchBranch = null;
                        }
                        else
                        {

                            if (!branch.CanAddBirdFromOtherBranch(currentTouchBranch))
                            {
                                SoundManager.Instance.PlayTweetSound();

                                currentTouchBranch.HighlightBird(false);
                                currentTouchBranch = branch;
                                currentTouchBranch.HighlightBird(true);
                                return;
                            }

                            if (currentTouchBranch)
                            {
                                Debug.Log("Add Bird");
                                // Day la luot touch thu hai
                                branch.HighlightBird(false);
                                branch.AddBirdFromOtherBranch(currentTouchBranch);
                                currentTouchBranch.HighlightBird(false);
                                branch.HighlightBird(false);
                                currentTouchBranch = null;
                            }
                        }
                    }
                } else
                {
                }
            }
        }
    }


    private BranchController branchTut;
    public void LoadLevel(bool newGame = true)
    {
        UiController.Instance.SetTextLevel(GameInfo.Level);
        HandTutorialController.Instance.EndTutorial();
        ClearUndoSystem();
        levelManger.LoadLevelFC(GameInfo.Level, newGame);
        currentTouchBranch = null;
        

        if (GameInfo.Level == 2 && !PlayerPrefs.HasKey("Tut_addBranch"))
        {
            UiGameplay.Instance.StartTutorial("Hello click this to add new branch",
                ()=>
                {
                    PlayerPrefs.SetInt("Tut_addBranch", 1);
                });
            UiGameplay.Instance.OnTutorial_EnableButton(UiGameplay.Instance.canvasButtonAddBranch);
        }
        branchTut = null;
        StartCoroutine(WaitForAction(0.6f, () =>
        {
            if (GameInfo.Level == 1 && !PlayerPrefs.HasKey("Tut_lv1"))
            {
                HandTutorialController.Instance.OpenTutorial();
                branchTut = levelManger.GetPosTut();
                HandTutorialController.Instance.ChangePosition(branchTut.listSlotPosBirds[2].transform.position);
            }
        }));
        //if (pLevel == 3)
        //{
        //    UiGameplay.Instance.StartTutorial("Hello click this to undo last step");
        //    UiGameplay.Instance.OnTutorial_EnableButton(UiGameplay.Instance.canvasButtonUndo);
        //}
    }
    IEnumerator WaitForAction(float time, System.Action ac)
    {
        yield return new WaitForSeconds(time);
        ac?.Invoke();
    }
    public void AddMovement(Action action)
    {
        if (listMovement == null)
        {
            listMovement = new Stack<Action>();
        }
        listMovement.Push(action);
        UiController.Instance.UpdateUndoBtn(true);
    }

    public void Undo()
    {
        UndoSystem.Instance.Undo();
    }

    public void ClearUndoSystem()
    {
        UndoSystem.Instance.RefreshUndoSystem();
    }

    public void AddNewBranch()
    {
        if (levelManger.getTotalBranch() >= 12) return;
        GameInfo.Branch--;
        UiController.Instance.UpdateBoosterBranch(GameInfo.Branch);
        levelManger.AddNewBranch();
    }

    public void ShuffleBird()
    {
        levelManger.ShuffeBranches();
    }

    public void OpenHighLightTutorial(int id, BranchController branchController)
    {
        levelManger.OpenHighLightTut(id, branchController);
    }

    public void ReplayGame()
    {
        LoadLevel(false);
    }

    public void CloseAllHighLightTutorial()
    {
        levelManger.CloseAllHightLighTut();
    }

    private bool IsTouchUI()
    {
        return IsPointerOverUIElement();
    }

    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;

        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

}
