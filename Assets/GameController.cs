using Action = System.Action;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int pLevel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pLevel = PlayerPrefs.GetInt("Level", 1);
    }


    private void Update()
    {
        //if (state == STATE.MOVING) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(mPos, Vector3.zero, Mathf.Infinity, branchLayerMask);
            if (hitInfo.collider != null)
            {
                BranchController branch = hitInfo.collider.GetComponent<BranchController>();
                
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
                            Debug.Log("Set new branch");
                            currentTouchBranch = branch;
                            branch.HighlightBird(true);
                        }
                    } else
                    {
                        if (branch == currentTouchBranch)
                        {
                            branch.HighlightBird(false);
                            currentTouchBranch.HighlightBird(false);
                            Debug.Log("Touch Current branch");
                            currentTouchBranch = null;
                        }
                        else
                        {
                            if (!branch.CanAddBirdFromOtherBranch(currentTouchBranch))
                            {
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
                }
            }
        }
    }

    public void LoadLevel(bool newGame = true)
    {
        ClearUndoSystem();
        levelManger.LoadLevelFC(pLevel, newGame);
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
        levelManger.AddNewBranch();
    }

    public void ShuffleBird()
    {
        levelManger.ShuffeBranches();
    }
   
    public void ReplayGame()
    {
        LoadLevel(false);
    }
}
