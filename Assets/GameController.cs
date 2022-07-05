using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public BranchController currentTouchBranch;

    public LayerMask branchLayerMask;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
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
                        if (!branch.IsEmpty())
                        {
                            Debug.Log("Set new branch");
                            currentTouchBranch = branch;
                            currentTouchBranch.HighlightBird(true);
                        }
                    } else
                    {
                        if (branch == currentTouchBranch)
                        {
                            currentTouchBranch.HighlightBird(false);
                            Debug.Log("Touch Current branch");
                            currentTouchBranch = null;
                            return;
                        }

                        if (currentTouchBranch)
                        {
                            Debug.Log("Add Bird");
                            // Day la luot touch thu hai
                            currentTouchBranch.HighlightBird(false);
                            branch.AddBirdFromOtherBranch(currentTouchBranch);
                            currentTouchBranch = null;
                        }
                    }
                }
            }
        }
    }

}
