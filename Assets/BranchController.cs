using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public List<Transform> listSlotPosBirds;
    private List<BirdController> listBirdsOnBranch = new List<BirdController>();
    private int numberOfBirdsInBranch;
    //public BirdController birdPrefab;
    public Transform spawnPos;
    public Animator animator;
    public ParticleSystem birdTouchBranchEffect;
    public BranchBezierPath branchBezierPath;

    private void Start()
    {
        //listBirdsOnBranch = new List<BirdController>();
        //for (int i = 0; i < 2; i++)
        //{
        //    AddBirdOnStart();
        //}
    }


    public int GetCountBird()
    {
        return listBirdsOnBranch.Count;
    }

    private void OnEnable()
    {
        IsReady = false;
        listBirdsOnBranch = new List<BirdController>();
        numberOfBirdsInBranch = 0;
        name = "Branch " + UnityEngine.Random.Range(0, 10000);
    }

    public void PlayBirdTouchBranchEffect(bool play = true)
    {
        if (!play)
        {
            birdTouchBranchEffect.Stop();
        } else
        {
            birdTouchBranchEffect.Play();
        }
    }

    public void AddBirdOnStart(BirdController bird)
    {
        listBirdsOnBranch.Add(bird);
        numberOfBirdsInBranch = listBirdsOnBranch.Count;
        //numberOfBirdsInBranch++;
    }

    public void AddBird(BirdController bird, bool firstBird = false)
    {
        IsReady = false;
        //if (bird == null) return;
        GameController.Instance.state = STATE.MOVING;

        bird.transform.SetParent(null);
        Vector2 nextPos = GetNextSlotPos();
        if (nextPos == Vector2.zero)
        {
            return;
        }
        listBirdsOnBranch.Add(bird);
        bool checkWin = CheckFullBranchWithSameBird();

        System.Action callback = () =>
        {
            IsReady = true;
            if (checkWin)
            {
                ReleaseBirds();
                SoundManager.Instance.PlayReleaseSound();
                bool CheckWinGame = GameController.Instance.levelManger.CheckWinGame();
                if (CheckWinGame)
                {
                    UiController.Instance.ShowWin();
                }

            }
            PlayBirdTouchBranchEffect();
            animator.SetTrigger("touch");
        };

        bird.FlyToPosition(nextPos, this, callback);
        //numberOfBirdsInBranch/*++;*/
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

    }



    private void ReleaseBirds()
    {
        Vector2 target = new Vector2(0, 60);
        target.x = -Mathf.Sign(transform.position.x) * 20;
        foreach (BirdController b in listBirdsOnBranch)
        {
            b.SetRelease(true);
            b.FlyToOtherPos(target);
            b.PlayBirdReleaseEffect();
        }
        listBirdsOnBranch.Clear();
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

    }

    public void HighlightBird(bool active)
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        if (numberOfBirdsInBranch <= 0) return;
        if (active)
        {
            List<BirdController> allHighLight = GetBirdsCanFly();
            foreach (BirdController b in allHighLight)
            {
                b.AddAnimation(BirdAnimation.TOUCHING);
            }
        } else
        {
            foreach (BirdController b in listBirdsOnBranch)
            {
                b.ClearTrack();
            }
        }
    }

    public bool CheckFullBranchWithSameBird()
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        if (listBirdsOnBranch.Count < 4) return false;
        for (int i = 0; i < numberOfBirdsInBranch; i++)
        {
            if (listBirdsOnBranch[i].ID == listBirdsOnBranch[0].ID) continue;
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool IsEmpty()
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        return listBirdsOnBranch.Count == 0 || numberOfBirdsInBranch <= 0;
    }
    public bool IsReady = false;
    public void AddBirdToSlotInit(BirdController bird, int slot, bool firstBird = false)
    {
        SoundManager.Instance.PlayReleaseSound();

        bird.transform.position = spawnPos.position;
        System.Action callback = () =>
        {
            IsReady = true;
        };
        bird.FlyToPosition(listSlotPosBirds[slot].localPosition, this, callback, false);
        //bird.SetOrder(slot + 3);
    }

    public bool CanAddBirdFromOtherBranch(BranchController fromBranch)
    {
        fromBranch.HighlightBird(false);
        numberOfBirdsInBranch = listBirdsOnBranch.Count;
        if (numberOfBirdsInBranch == 4) return false;
        List<BirdController> getAllBirdFromOtherBranch = fromBranch.GetBirdsCanFly();
        if (getAllBirdFromOtherBranch.Count > 0)
        {
            if (numberOfBirdsInBranch > 0 && (getAllBirdFromOtherBranch[0].ID == this.listBirdsOnBranch[numberOfBirdsInBranch - 1].ID))
            {
                return true;
            } else
            {
                if (numberOfBirdsInBranch == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetPosAllBird()
    {
        if (listBirdsOnBranch.Count == 0 || listBirdsOnBranch == null) return;
        for (int i = 0; i < listBirdsOnBranch.Count; i++)
        {
            BirdController b = listBirdsOnBranch[i];
            AddBirdToSlotInit(b, i, i == 1);
        }
    }

    public void AddBirdFromOtherBranch(BranchController fromBranch)
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        List<BirdController> getAllBirdFromOtherBranch = fromBranch.GetBirdsCanFly();

        if (getAllBirdFromOtherBranch == null) return;
        if (getAllBirdFromOtherBranch.Count > 0)
        {
            // Nếu hiện tại đã có chim và con chim ở ngoài cùng không có ID trùng với chim mới thì không cho phép bay
            if (numberOfBirdsInBranch > 0 && (getAllBirdFromOtherBranch[0].ID != this.listBirdsOnBranch[numberOfBirdsInBranch - 1].ID))
            {
                return;
            }
        }
        if (getAllBirdFromOtherBranch.Count + numberOfBirdsInBranch > 4)
        {
            Debug.LogError("Out of slot");
            int numberSlotCanPlace = 4 - numberOfBirdsInBranch;

            List<BirdController> newPos = new List<BirdController>();
            for (int i = 0; i < numberSlotCanPlace; ++i)
            {
                newPos.Add(getAllBirdFromOtherBranch[i]);
            }

            getAllBirdFromOtherBranch = newPos;
        }
        // Bỏ highlight ở cành cũ
        fromBranch.HighlightBird(false);

        // Xóa chim ở cành cũ
        fromBranch.RemoveBird(getAllBirdFromOtherBranch);

        bool isFull = true;
        for (int i = 0; i < listBirdsOnBranch.Count; i++)
        {
            if (listBirdsOnBranch[i].ID != getAllBirdFromOtherBranch[0].ID)
            {
                isFull = false;
                break;
            }
        }
        isFull = isFull && (getAllBirdFromOtherBranch.Count + numberOfBirdsInBranch) == 4;

        List<BirdController> clearBirds = new List<BirdController>(getAllBirdFromOtherBranch);
        List<BirdController> current = new List<BirdController>(listBirdsOnBranch);
        List<Movement> listMovement = new List<Movement>();

        //if (isFull)
        //{
        if (isFull)
        {
            for (int i = 0; i < current.Count; i++)
            {
                BirdController b = current[i];
                Movement m = new Movement
                (
                    b,
                    this,
                    this,
                    b.slotInBranch,
                    isFull
                );
                listMovement.Add(m);
            }
        }

        for (int i = 0; i < clearBirds.Count; i++)
        {
            BirdController b = clearBirds[i];
            Movement m = new Movement
            (
                b,
                fromBranch,
                this,
                b.slotInBranch,
                isFull
            );
            listMovement.Add(m);
        }
        
        //UndoSystem.Instance.SetLast(listMovement);
        //} else
        //{
        //    List<Movement> listMovement = new List<Movement>();

        //    for (int i = 0; i < clearBirds.Count; i++)
        //    {
        //        BirdController b = clearBirds[i];
        //        Movement m = new Movement
        //        (
        //            b,
        //            fromBranch,
        //            this,
        //            b.slotInBranch,
        //            isFull
        //        );
        //        listMovement.Add(m);
        //    }

        UndoSystem.Instance.PushBackMovement(listMovement);
        //}


        for (int i = 0; i < getAllBirdFromOtherBranch.Count; i++)
        {
            // Di chuyển chim từ cành cũ sang cành hiện tại (this)
            AddBird(getAllBirdFromOtherBranch[i], true);
        }
        //   HighlightBird(false);

        // Setup Undo Sytem

    }


    public void ShuffBranch(BranchController toBranch)
    {
        List<BirdController> listCurrent = new List<BirdController>( listBirdsOnBranch );
        List<BirdController> listOther = new List<BirdController>(toBranch.listBirdsOnBranch);
        toBranch.RemoveBird(listOther);
        RemoveBird(listCurrent);

        for (int i = 0; i < listCurrent.Count; i++)
        {
            // Di chuyển chim từ cành cũ sang cành hiện tại (this)
            toBranch.AddBird(listCurrent[i], true);
        }

        for (int i = 0; i < listOther.Count; i++)
        {
            // Di chuyển chim từ cành cũ sang cành hiện tại (this)
            AddBird(listOther[i], true);
        }
        
    }

    private void UndoTemp()
    {
        //bool isRelease = CheckFullBranchWithSameBird();
        //for (int i = 0; i < getAllBirdFromOtherBranch.Count; i++)
        //{
        //    getAllBirdFromOtherBranch[i].PushBranchStack(this, isRelease);
        //    //getAllBirdFromOtherBranch[i].PushBranchStack(fromBranch);
        //    AddBird(getAllBirdFromOtherBranch[i], this, true);
        //}

        // UNDO SYSTEM NOT COMPLETED!
        ///----------------------------------------------------------
        //if (CheckFullBranchWithSameBird()) return;
        //List<BirdController> undoList = new List<BirdController>(getAllBirdFromOtherBranch);
        //bool isRelease = CheckFullBranchWithSameBird();
        //if (isRelease)
        //{
        //    undoList = new List<BirdController>(listBirdsOnBranch);
        //}
        //Movement movement = new Movement
        //{
        //    fromBranch = fromBranch,
        //    destBranch = this,
        //    IsReleased = isRelease,
        //    action = ()=>
        //    {
        //        for (int i = 0; i < undoList.Count; i++)
        //        {
        //            BirdController bird = undoList[i];
        //            AddBird(bird, fromBranch, true);
        //        }
        //        RemoveBird(undoList);
        //    }
        //};

        //UndoSystem.Instance.PushBackMovement(movement);


        //Action undoMovement = ()=> 
        //{
        //    //RemoveBird(undoList);
        //};
        //bool first = true;
        //for (int i = 0; i < undoList.Count; i++)
        //{
        //    BirdController bird = undoList[i];
        //    BranchController from = bird.GetLastBranch();

        //    undoMovement += () =>
        //    {
        //        if (check)
        //        {
        //            from = bird.GetLastBranch();
        //        }
        //        //if (bird.IsReleased()) return;
        //        AddBird(bird, from, first);
        //    };
        //    first = false;
        //}
        //undoMovement += () =>
        //{
        //    RemoveBird(undoList);
        //};

        //GameController.Instance.AddMovement(undoMovement);

    }

    public void RemoveBird(List<BirdController> listBirdFly)
    {
        List<BirdController> newListBird = new List<BirdController>();
        for (int i = 0; i < listBirdsOnBranch.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < listBirdFly.Count; j++)
            {
                if (listBirdFly[j].IDD == listBirdsOnBranch[i].IDD)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                newListBird.Add(listBirdsOnBranch[i]);
            }
        }
        listBirdsOnBranch.Clear();
        listBirdsOnBranch = newListBird;
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

    }

    public List<BirdController> GetBirdsCanFly()
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        List<BirdController> listBirdsAvailable = new List<BirdController>();
        if (listBirdsOnBranch.Count == 0) return null;

        int idFirstBird = listBirdsOnBranch[listBirdsOnBranch.Count - 1].ID;
        for (int i = listBirdsOnBranch.Count - 1; i >= 0; i--)
        {
            if (idFirstBird == listBirdsOnBranch[i].ID)
            {
                listBirdsAvailable.Add(listBirdsOnBranch[i]);
            } else
            {
                break;
            }
        }
        return listBirdsAvailable;
    }

    public Vector2 GetNextSlotPos()
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        if (numberOfBirdsInBranch == listSlotPosBirds.Count)
        {
            return Vector2.zero;
        }
        return listSlotPosBirds[numberOfBirdsInBranch].localPosition;
    }
}
