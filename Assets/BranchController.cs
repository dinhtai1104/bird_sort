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


    private void OnEnable()
    {
        listBirdsOnBranch = new List<BirdController>();
        numberOfBirdsInBranch = 0;
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

    public void AddBirdOnStart()
    {
        //Vector2 nextPos = GetNextSlotPos();
        //BirdController bird = Instantiate(birdPrefab, spawnPos.position, Quaternion.identity);
        ////bird.transform.localPosition = nextPos;
        //bird.transform.localScale = new Vector2(Mathf.Sign(transform.position.x), 1);
        //System.Action callback = () =>
        //{
        //    animator.SetTrigger("touch");
        //};
        //bird.FlyToPosition(nextPos, this, callback);

        //listBirdsOnBranch.Add(bird);
        //numberOfBirdsInBranch++;
    }

    public void AddBird(BirdController bird, BranchController nextBranch, bool firstBird = false)
    {
        //if (bird == null) return;
        GameController.Instance.state = STATE.MOVING;

        bird.transform.SetParent(null);
        Vector2 nextPos = GetNextSlotPos();
        HighlightBird(false);
        nextBranch.HighlightBird(false);
        if (nextPos == Vector2.zero)
        {
            return;
        }
        listBirdsOnBranch.Add(bird);
        bool checkWin = CheckFullBranchWithSameBird();

        System.Action callback = () =>
        {
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

        bird.FlyToPosition(nextPos, nextBranch, callback);
        numberOfBirdsInBranch++;
        HighlightBird(false);
        nextBranch.HighlightBird(false);
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
        numberOfBirdsInBranch = 0;
        listBirdsOnBranch.Clear();
    }

    public void HighlightBird(bool active)
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;

        if (numberOfBirdsInBranch <= 0) return;
        List<BirdController> allHighLight = GetBirdsCanFly();
        foreach (BirdController b in allHighLight)
        {
            if (active)
            {
                b.AddAnimation(BirdAnimation.TOUCHING);
            }
            else
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

    public void AddBirdToSlotInit(BirdController bird, int slot, bool firstBird = false)
    {
        if (listBirdsOnBranch == null)
        {
            listBirdsOnBranch = new List<BirdController>();
        }
        System.Action callback = () =>
        {
            animator.SetTrigger("touch");
        };
        bird.FlyToPosition(listSlotPosBirds[slot].localPosition, this, callback, false);
        listBirdsOnBranch.Add(bird);
        numberOfBirdsInBranch++;
    }

    public void AddBirdFromOtherBranch(BranchController fromBranch)
    {
        numberOfBirdsInBranch = listBirdsOnBranch.Count;
        HighlightBird(false);
        fromBranch.HighlightBird(false);
        List<BirdController> getAllBirdFromOtherBranch = fromBranch.GetBirdsCanFly();

        if (getAllBirdFromOtherBranch == null) return;
        if (getAllBirdFromOtherBranch.Count > 0)
        {
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


        fromBranch.RemoveBird(getAllBirdFromOtherBranch);

        

        for (int i = 0; i < getAllBirdFromOtherBranch.Count; i++)
        {
            AddBird(getAllBirdFromOtherBranch[i], this, i == 0);
        }
        HighlightBird(false);
        fromBranch.HighlightBird(false);
        if (CheckFullBranchWithSameBird()) return;
        List<BirdController> undoList = getAllBirdFromOtherBranch;

        Action undoMovement = ()=> 
        {
            //if (CheckFullBranchWithSameBird()) return;
            RemoveBird(undoList);
        };
        bool first = true;

        for (int i = undoList.Count - 1; i >= 0; --i)
        {
            BirdController bird = undoList[i];
            undoMovement += () =>
            {
                if (bird.IsReleased()) return;
                Debug.Log("Undo list: " + undoList.Count);
                fromBranch.AddBird(bird, fromBranch, first);
            };
            first = false;
        }

        GameController.Instance.AddMovement(undoMovement);

    }

    public void RemoveBird(List<BirdController> listBirdFly)
    {
        List<BirdController> newListBird = new List<BirdController>();
        for (int i = 0; i < listBirdsOnBranch.Count - listBirdFly.Count; i++)
        {
            newListBird.Add(listBirdsOnBranch[i]);
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
