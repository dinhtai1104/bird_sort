using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/


public struct BirdMove
{
}
public class BirdMovement : MonoBehaviour
{
    public delegate void Undo();
    public static Undo undoSytem;
	public Stack<BranchController> branchesBeforeStack;
	public Stack<bool> isReleaseStack;

    private BirdController birdController;


    private void OnEnable()
    {
        if (!birdController)
            birdController = GetComponent<BirdController>();
        branchesBeforeStack = new Stack<BranchController>();
        isReleaseStack = new Stack<bool>();
        undoSytem += UndoEvent;
    }


    private void OnDisable()
    {
        if (!birdController)
            birdController = GetComponent<BirdController>();
        undoSytem -= UndoEvent;
    }

    public void UndoEvent()
    {
        if (!birdController)
            birdController = GetComponent<BirdController>();

        if (branchesBeforeStack.Count == 0) return;
        bool lastReleased = isReleaseStack.Pop();
        BranchController lastBranch = branchesBeforeStack.Pop();
        
        if (lastReleased == true)
        {
            lastBranch = branchesBeforeStack.Pop();
        }
        birdController.FlyToPosition(lastBranch.GetNextSlotPos(), lastBranch);

    }
    public void PushBackBranch(BranchController b, bool isRelease)
    {
        //if (branchesBeforeStack.Count > 0)
        //{
        //    if (branchesBeforeStack.Peek() == b) return;
        //    branchesBeforeStack.Push(b);
        //} else
        {
            isReleaseStack.Push(isRelease);
            branchesBeforeStack.Push(b);
        }
    }

    public BranchController ReleaseToLastBranch()
    {
        if (branchesBeforeStack.Count == 1) return branchesBeforeStack.Peek();
        BranchController last = branchesBeforeStack.Pop();

        return last;
    }
}
