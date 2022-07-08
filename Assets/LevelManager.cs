using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
public class LevelManager : MonoBehaviour
{
    public BranchController branchPrefab;
    private List<BranchController> listBranchInLevel = new List<BranchController>();
    public Transform startSpawnBranchLeft, startSpawnBranchRight;
    private Vector2 lastPosLeft, lastPosRight;
    public LevelModel levelCurrent;

    private List<BirdController> listBirds = new List<BirdController>();
    private int id = 0;
    private bool isLeft = true;

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
        startSpawnBranchLeft.position = new Vector2(-width - 0.5f, startSpawnBranchLeft.position.y);
        startSpawnBranchRight.position = new Vector2(width + 0.5f, startSpawnBranchRight.position.y);
    }

    public BranchController GetBranch(int v)
    {
        return listBranchInLevel[v];
    }

    public int getTotalBranch() => listBranchInLevel.Count;
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
                    bird.PushBranchStack(branch, false);

                    branch.AddBirdOnStart(bird);
                    bird.SetID(p.type);
                    bird.transform.SetParent(branch.transform);
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

        StopAllCoroutines();
        StartCoroutine(WaitForAction(0.3f, () =>
        {
            foreach (BranchController b in listBranchInLevel)
            {
                b.SetPosAllBird();
            }
        }));

        if (newGame)
            UiController.Instance.JoinGame();
        UiController.Instance.DisableButtonsJoinGame();
    }

    public BranchController GetPosTut()
    {
        return listBranchInLevel[0];
    }

    public void LoadLevelFC(int pLevel, bool newGame = false)
    {
        isLeft = true;
        id = 0;
        DestroyBranches();
        listBranchInLevel = new List<BranchController>();
        listBirds = new List<BirdController>();

        LevelModelFC level = LevelLoader.GetLevelModelFC(pLevel);
        List<PieceFC> pieces = level.standConfig;

        int totalBranches = pieces.Count;
        lastPosLeft = startSpawnBranchLeft.position;
        lastPosRight = startSpawnBranchRight.position;
        float offset = lastPosLeft.y - lastPosRight.y;
        lastPosLeft.y = 0;
        lastPosRight.y = offset;

        lastPosLeft = lastPosLeft + Vector2.up * 1.5f * totalBranches / 4f;
        lastPosRight = lastPosRight + Vector2.up * 1.5f * totalBranches / 4f;


        for (int i = 0; i < pieces.Count; i++)
        {
            BranchController branch = AddNewBranch(false);


            bool firstBird = true;
            branch.IsReady = true;

            int[] idBirds = pieces[i].idBirds;
            for (int x = 0; x < idBirds.Length; x++)
            {
                branch.IsReady = false;
                BirdController bird = PoolingSystem.Instance.GetBird();
                bird.gameObject.SetActive(true);
                bird.transform.position = branch.spawnPos.position;
                branch.AddBirdOnStart(bird);
                //branch.AddBirdToSlotInit(bird, x, firstBird);
                bird.SetID(idBirds[x]);
                bird.transform.SetParent(branch.transform);
                bird.PushBranchStack(branch, false);
                if (isLeft)
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
            //isLeft = !isLeft;
            id++;
        }
        upgradeIndex = 0;
        upGradePos = false;

        StopAllCoroutines();

        StartCoroutine(WaitForAction(0.3f, () =>
        {

            foreach (BranchController b in listBranchInLevel)
            {
                b.SetPosAllBird();
            }
        }));

        
        if (newGame)
            UiController.Instance.JoinGame();
        UiController.Instance.DisableButtonsJoinGame();
    }

    public void CloseAllHightLighTut()
    {
        for (int i = 0; i < listBranchInLevel.Count; i++)
        {
            listBranchInLevel[i].branchTutorial.DisableAll();
        }
    }

    public void OpenHighLightTut(int id, BranchController branchController)
    {
        for (int i = 0; i < listBranchInLevel.Count; i++)
        {
            if (branchController != listBranchInLevel[i])
            {
                if (GameInfo.Level == 2)
                {

                    if (listBranchInLevel[i].GetCountBird() == 0)
                    {
                        listBranchInLevel[i].branchTutorial.ActiveV();
                        continue;
                    }
                    if (listBranchInLevel[i].GetLastId() == id)
                    {
                        listBranchInLevel[i].branchTutorial.ActiveV();
                    }
                    else
                    {
                        listBranchInLevel[i].branchTutorial.ActiveX();
                    }
                }
                else
                {
                    listBranchInLevel[i].branchTutorial.DisableAll();
                }

            }
        }
    }

    IEnumerator WaitForAction(float time, System.Action ac)
    {
        yield return new WaitForSeconds(time);
        ac?.Invoke();
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
            if (branches > 8)
            {
                if (!upGradePos)
                {
                    upgradeIndex = id;
                    upgradeIndex %= 3;
                }
                upGradePos = true;
                if ((branches) % 3 == 0 && !isLeft)
                {
                    // Move branch
                    foreach (BranchController br in listBranchInLevel)
                    {
                        if (autoIncrease)
                        {
                            br.transform.DOMoveY(br.transform.position.y + 2, 0.2f);
                        } else
                        {
                            br.transform.position += Vector3.up * 2;
                        }
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

        if (isLeft)
        {
            branch.transform.localScale = new Vector3(1, 1, 1);
            branch.transform.position = lastPosLeft;
            branch.transform.position += Vector3.left * 3;
            branch.transform.DOMove(lastPosLeft, 0.2f);

            lastPosLeft += Vector2.down * 2;

        }
        else
        {
            branch.transform.localScale = new Vector3(-1, 1, 1);
            branch.transform.position = lastPosRight;
            branch.transform.position += Vector3.right * 3;
            branch.transform.DOMove(lastPosRight, 0.2f);
            lastPosRight += Vector2.down * 2;
        }

        isLeft = !isLeft;

        if (autoIncrease)
            id++;

        if (listBranchInLevel.Count >= 12)
        {
            UiController.Instance.addBranchBtn.interactable = false;
        }
        return branch;
    }


    public void ShuffeBranches()
    {
        GameInfo.Shuff--;
        BranchController aBranch = listBranchInLevel[Random.Range(0, listBranchInLevel.Count)];
        BranchController bBranch = listBranchInLevel[Random.Range(0, listBranchInLevel.Count)];
        int index = 0;
        while (index < 100)
        {
            if (bBranch.GetCountBird() > 0 && bBranch != aBranch)
            {
                Debug.Log(aBranch.name + "-" + bBranch.name);
                bBranch.ShuffBranch(aBranch);

                Action a = () =>
                {
                    aBranch.ShuffBranch(bBranch);
                };

                UndoSystem.Instance.PushBackMovement(null, a);

                UiController.Instance.UpdateBoosterShuff(GameInfo.Shuff);

                return;
            }
            index++;
            bBranch = listBranchInLevel[Random.Range(0, listBranchInLevel.Count)];
        }
        for (int i = 0; i < listBranchInLevel.Count; i++)
        {
            if (listBranchInLevel[i] != aBranch)
            {
                BranchController b = listBranchInLevel[i];
                Debug.Log(aBranch.name + "-" + b.name);

                aBranch.ShuffBranch(b);
                Action a = () =>
                {
                    b.ShuffBranch(aBranch);
                };
                UndoSystem.Instance.PushBackMovement(null, a);
                UiController.Instance.UpdateBoosterShuff(GameInfo.Shuff);

                return;
            }
        }
    }

    private void DestroyBranches()
    {
        if (listBranchInLevel == null) return;
        PoolingSystem.Instance.ResetPool();
    }

}
