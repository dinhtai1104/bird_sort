using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BranchController branchPrefab;
    private List<BranchController> listBranchInLevel = new List<BranchController>();
    public Transform startSpawnBranch;
    private Vector2 lastPos;
    public LevelModel levelCurrent;
    public void LoadLevel(int pLevel)
    {
        DestroyBranches();
        listBranchInLevel = new List<BranchController>();

        LevelModel levelModel = LevelLoader.GetLevelModel(pLevel);
        levelCurrent = levelModel;
        if (levelModel==null)
        {
            Debug.LogError("Level load fail");
            return;
        }
        int totalBranch = levelModel.branch;
        lastPos = startSpawnBranch.position;
        List<Piece> pieces = levelModel.pieces;
        for (int i = 0; i < totalBranch; i++)
        {
            BranchController branch = Instantiate(branchPrefab, lastPos, Quaternion.identity);
            listBranchInLevel.Add(branch);
            lastPos += Vector2.down * 2;
            bool firstBird = true;
            foreach (Piece p in pieces)
            {
                if (p.branch == i + 1)
                {
                    BirdController bird = Instantiate(branch.birdPrefab, branch.spawnPos.position, Quaternion.identity);
                    branch.AddBirdToSlotInit(bird, p.possition - 1, firstBird);
                    if (firstBird)
                    {
                        firstBird = false;
                    }
                }
            }
        }
    }

    private void DestroyBranches()
    {
        foreach (BranchController b in listBranchInLevel)
        {
            Destroy(b.gameObject);
        }
    }

    private void Start()
    {
        listBranchInLevel = new List<BranchController>();
        LoadLevel(1);
    }
}
