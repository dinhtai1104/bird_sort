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
                    if (currentTouchBranch == null)
                    {
                        if (!branch.IsEmpty() && !branch.CheckFullBranchWithSameBird())
                        {
                            Debug.Log("Set new branch");
                            currentTouchBranch = branch;
                            branch.HighlightBird(true);
                        }
                    } else
                    {
                        if (branch == currentTouchBranch)
                        {
                            currentTouchBranch.HighlightBird(false);
                            Debug.Log("Touch Current branch");
                            currentTouchBranch = null;
                        }
                        else
                        {
                            if (currentTouchBranch)
                            {
                                Debug.Log("Add Bird");
                                // Day la luot touch thu hai
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
        if (currentTouchBranch)
        {
            currentTouchBranch.HighlightBird(false);
            currentTouchBranch = null;
        }
        if (listMovement.Count > 0)
        {
            Action undo = listMovement.Pop();
            undo?.Invoke();
            if (listMovement.Count == 0)
            {
                UiController.Instance.UpdateUndoBtn(false);
            }
        }
    }

    public void ClearUndoSystem()
    {
        listMovement.Clear();
        if (listMovement.Count == 0)
        {
            UiController.Instance.UpdateUndoBtn(false);
        }
    }

    public void AddNewBranch()
    {
        levelManger.AddNewBranch();
    }

    public void ShuffleBird()
    {
    }
   
    public void ReplayGame()
    {
        LoadLevel(false);
    }
}
