using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BranchController branchPrefab;
    private List<BranchController> listBranchInLevel = new List<BranchController>();
    public Transform startSpawnBranchLeft, startSpawnBranchRight;
    private Vector2 lastPosLeft, lastPosRight;
    public LevelModel levelCurrent;

    private List<BirdController> listBirds = new List<BirdController>();
    private int id = 0;


    private void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth * 1.0f / screenHeight;
        float milestoneAspect = 9f / 16f;

        if (screenAspect <= milestoneAspect)
        {
            Camera.main.orthographicSize = (1080 / 100.0f) / (2 * screenAspect);
        }
        else
        {
            Camera.main.orthographicSize = Screen.height / 200f; //1560f / 200f
        }

        float orthorSize = Camera.main.orthographicSize;
        float width = Camera.main.aspect * Camera.main.orthographicSize;
        startSpawnBranchLeft.position = new Vector2(-width, startSpawnBranchLeft.position.y);
        startSpawnBranchRight.position = new Vector2(width, startSpawnBranchRight.position.y);
    }

    public void LoadLevel(int pLevel, bool newGame = false)
    {
        id = 0;
        DestroyBranches();
        listBranchInLevel = new List<BranchController>();
        listBirds = new List<BirdController>();
        LevelModel levelModel = LevelLoader.GetLevelModel(pLevel);
        levelCurrent = levelModel;
        if (levelModel==null)
        {
            Debug.LogError("Level load fail");
            return;
        }
        int totalBranch = levelModel.branch;
        lastPosLeft = startSpawnBranchLeft.position;
        lastPosRight = startSpawnBranchRight.position;
        List<Piece> pieces = levelModel.pieces;
        for (int i = 0; i < totalBranch; i++)
        {
            BranchController branch = AddNewBranch(false);
            bool firstBird = true;
            foreach (Piece p in pieces)
            {
                if (p.branch == id + 1)
                {
                    BirdController bird = PoolingSystem.Instance.GetBird();
                    bird.gameObject.SetActive(true);
                    bird.transform.position = branch.spawnPos.position;

                    branch.AddBirdToSlotInit(bird, p.possition - 1, firstBird);
                    bird.SetID(p.type);
                    if (id % 2 == 0)
                    {
                        // Left
                        bird.transform.localScale = new Vector3(-1, 1, 1);
                    } else
                    {
                        // Right
                        bird.transform.localScale = new Vector3(1, 1, 1);
                    }

                    if (firstBird)
                    {
                        firstBird = false;
                    }

                    listBirds.Add(bird);
                }
            }
            id++;
        }
        upgradeIndex = 0;
        upGradePos = false;
        if (newGame)
            UiController.Instance.JoinGame();
    }

    public void LoadLevelFC(int pLevel, bool newGame = false)
    {
        id = 0;
        DestroyBranches();
        listBranchInLevel = new List<BranchController>();
        listBirds = new List<BirdController>();

        LevelModelFC level = LevelLoader.GetLevelModelFC(pLevel);
        List<PieceFC> pieces = level.standConfig;

        lastPosLeft = startSpawnBranchLeft.position;
        lastPosRight = startSpawnBranchRight.position;
        for (int i = 0; i < pieces.Count; i++)
        {
            BranchController branch = AddNewBranch(false);
            bool firstBird = true;

            int[] idBirds = pieces[i].idBirds;
            for (int x = 0; x < idBirds.Length; x++)
            {

                BirdController bird = PoolingSystem.Instance.GetBird();
                bird.gameObject.SetActive(true);
                bird.transform.position = branch.spawnPos.position;

                branch.AddBirdToSlotInit(bird, x, firstBird);
                bird.SetID(idBirds[x]);
                if (id % 2 == 0)
                {
                    // Left
                    bird.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    // Right
                    bird.transform.localScale = new Vector3(1, 1, 1);
                }

                if (firstBird)
                {
                    firstBird = false;
                }
                listBirds.Add(bird);
            }
            id++;
        }
        upgradeIndex = 0;
        upGradePos = false;
        
        if (newGame)
            UiController.Instance.JoinGame();
    }

    public bool CheckWinGame()
    {
        foreach (BirdController bird in listBirds)
        {
            if (!bird.IsReleased()) return false;
        }
        return true;
    }

    bool upGradePos = false;
    int upgradeIndex = 0;
    public BranchController AddNewBranch(bool autoIncrease=true)
    {

        int branches = listBranchInLevel.Count;
        if (branches >= 12)
        {
            return null;
        }
        else
        {
            if (branches > 5)
            {
                upGradePos = true;
                if (upgradeIndex == 2)
                {
                    // Move branch
                    foreach (BranchController br in listBranchInLevel)
                    {
                        br.transform.position += Vector3.up * 2;
                    }
                    upgradeIndex = 0;
                    lastPosRight += Vector2.up * 2;
                    lastPosLeft += Vector2.up * 2;
                }
                upgradeIndex++;
            }
        }


        //BranchController branch = Instantiate(branchPrefab, (id % 2 == 0) ? lastPosLeft : lastPosRight, Quaternion.identity);
        BranchController branch = PoolingSystem.Instance.GetBranch();

        listBranchInLevel.Add(branch);
        branch.gameObject.SetActive(true);
        branch.transform.SetParent(null);

        if (id % 2 == 0)
        {
            branch.transform.localScale = new Vector3(1, 1, 1);
            branch.transform.position = lastPosLeft;
            lastPosLeft += Vector2.down * 2;

        }
        else
        {
            branch.transform.localScale = new Vector3(-1, 1, 1);
            branch.transform.position = lastPosRight;
            lastPosRight += Vector2.down * 2;
        }
        if (autoIncrease)
            id++;
        return branch;
    }

    

    private void DestroyBranches()
    {
        if (listBranchInLevel == null) return;
        PoolingSystem.Instance.ResetPool();
    }

}
