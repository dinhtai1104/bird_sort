using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class UndoSystem : MonoBehaviour
{
	public static UndoSystem Instance;

	public List<AllMovement> stackMovement = new List<AllMovement>();
    private void Awake()
    {
		Instance = this;
    }

	public void RefreshUndoSystem()
    {
		stackMovement = new List<AllMovement>();
    }

    public void PushBackMovement(List<Movement> listMovement)
    {
		stackMovement.Add(new AllMovement (listMovement));
		UiController.Instance.UpdateUndoBtn(true);
	}

	public bool CheckUndo()
    {
		return stackMovement.Count > 0;
    }

	public void Undo()
	{
		if (stackMovement.Count == 0) return;
		List<Movement> undoList = stackMovement[stackMovement.Count - 1].movements;
		stackMovement.RemoveAt(stackMovement.Count - 1);

		if (stackMovement.Count == 0)
		{
			UiController.Instance.UpdateUndoBtn(false);
		}

		//undoList = undoList.OrderBy(x => x.slot).ToList();
		for (int i = 0; i < undoList.Count; i++)
        {
			BirdController bird = undoList[i].bird;
			BranchController lastBranch = undoList[i].lastBranch;
			BranchController currentBranch = undoList[i].currenBranch;
			int slot = undoList[i].slot;
			bool isReleased = undoList[i].isReleased;
			if (bird.IsReleased())
				bird.transform.position = lastBranch.spawnPos.position;
			bird.SetRelease(false);
			// Move
			List<BirdController> birds = new List<BirdController>();
			birds.Add(bird);
			if (currentBranch)
				currentBranch.RemoveBird(new List<BirdController>(birds));
			lastBranch.AddBird(bird, true);
		}
	}

    public List<Movement> GetLast()
    {
		if (stackMovement.Count == 0)
        {
			AllMovement m = new AllMovement(new List<Movement>());
			stackMovement.Add(m);
		}
		List<Movement> undoList = stackMovement[stackMovement.Count - 1].movements;
		return undoList;
	}

	public void SetLast(List<Movement> l)
    {
		stackMovement[stackMovement.Count - 1] = new AllMovement(l);
    }
}

[System.Serializable]
public class AllMovement
{
	public List<Movement> movements;
	public AllMovement(List<Movement> m)
    {
		this.movements = m;
    }
}

[System.Serializable]
public class Movement 
{
	public BirdController bird;
	public BranchController lastBranch;
	public BranchController currenBranch;
	public int slot;

	public bool isReleased;

	public Movement(BirdController bird, BranchController lastBranch, BranchController currentBranch, int slot, bool isRelease)
    {
		this.bird = bird;
		this.lastBranch = lastBranch;
		this.currenBranch = currentBranch;
		this.slot = slot;
		this.isReleased = isRelease;
    }

    public int Compare(Movement x, Movement y)
    {
		if (x.slot > y.slot) return 1;
		return -1;
    }
}
