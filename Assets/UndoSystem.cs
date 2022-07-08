using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public void PushBackMovement(List<Movement> listMovement, Action callback = null)
    {
		AllMovement m = new AllMovement(listMovement);
		m.callback = callback;
		stackMovement.Add(m);
		UiController.Instance.UpdateUndoBtn(true);

		if (GameInfo.Level == 3 && !PlayerPrefs.HasKey("Tut_undo"))
		{
			UiGameplay.Instance.StartTutorial("Hello click this to undo last step",
				() =>
				{
					PlayerPrefs.SetInt("Tut_undo", 1);
				});
			UiGameplay.Instance.OnTutorial_EnableButton(UiGameplay.Instance.canvasButtonUndo);
		}

		UiController.Instance.UpdateBoosterUndo(stackMovement.Count);

	}

	public bool CheckUndo()
    {
		return stackMovement.Count > 0;
    }

	public void Undo()
	{
		if (stackMovement.Count == 0) return;

		AllMovement m = stackMovement[stackMovement.Count - 1];
		List<Movement> undoList = m.movements;
		stackMovement.RemoveAt(stackMovement.Count - 1);

		UiController.Instance.UpdateBoosterUndo(stackMovement.Count);

		if (stackMovement.Count == 0)
		{
			UiController.Instance.UpdateUndoBtn(false);
		}
		if (m.callback != null)
        {
			m.callback?.Invoke();
			return;
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

	public void SetLast(List<Movement> l, Action callback = null)
    {
		AllMovement m = new AllMovement(l);
		m.callback = callback;
		stackMovement[stackMovement.Count - 1] = m;
    }
}

[System.Serializable]
public class AllMovement
{
	public Action callback;
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
