using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class PoolingSystem : MonoBehaviour
{
	private static PoolingSystem _instance;
	public static PoolingSystem Instance
    {
		get
        {
			if (!_instance)
            {
                _instance = FindObjectOfType<PoolingSystem>();
            }
            return _instance;
        }
    }

    public BirdController birdPrefab;
    public BranchController branchPrefab;

    private List<BirdController> listBirds = new List<BirdController>();
    private List<BranchController> listBranches = new List<BranchController>();


    public BirdController GetBird()
    {
        foreach (BirdController b in listBirds)
        {
            if (!b.gameObject.activeInHierarchy)
            {
                b.transform.SetParent(null);
                return b;
            }
        }

        BirdController bird = Instantiate(birdPrefab, transform);
        bird.gameObject.SetActive(false);
        listBirds.Add(bird);
        return bird;
    }
    public BranchController GetBranch()
    {
        foreach (BranchController b in listBranches)
        {
            if (!b.gameObject.activeInHierarchy)
            {
                b.transform.SetParent(null);
                return b;
            }
        }

        BranchController bird = Instantiate(branchPrefab, transform);
        bird.gameObject.SetActive(false);
        listBranches.Add(bird);
        return bird;
    }
    public void ResetPool()
    {
        foreach (BirdController b in listBirds)
        {
            b.transform.SetParent(transform);
            b.gameObject.SetActive(false);
        }
        foreach (BranchController b in listBranches)
        {
            b.transform.SetParent(transform);
            b.gameObject.SetActive(false);
        }
    }
}
