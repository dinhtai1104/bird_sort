using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public List<Transform> listSlotPosBirds;
    private List<BirdController> listBirdsOnBranch;
    private int numberOfBirdsInBranch;
    public BirdController birdPrefab;
    public Transform spawnPos;
    public Animator animator;


    private void Start()
    {
        listBirdsOnBranch = new List<BirdController>();
        for (int i = 0; i < 2; i++)
        {
            AddBirdOnStart();
        }
    }

    public void AddBirdOnStart()
    {
        Vector2 nextPos = GetNextSlotPos();
        BirdController bird = Instantiate(birdPrefab, spawnPos.position, Quaternion.identity);
        //bird.transform.localPosition = nextPos;
        bird.transform.localScale = new Vector2(Mathf.Sign(transform.position.x), 1);

        bird.FlyToPosition(nextPos, this);

        listBirdsOnBranch.Add(bird);
        numberOfBirdsInBranch++;
    }

    public void AddBird(BirdController bird, BranchController nextBranch, bool firstBird = false)
    {
        bird.transform.SetParent(null);
        Vector2 nextPos = GetNextSlotPos();
        //bird.transform.localPosition = nextPos;
        //bird.transform.localScale = new Vector2(Mathf.Sign(transform.position.x) * Mathf.Sign(transform.localScale.x), 1);
        System.Action callback = () =>
        {
            animator.SetTrigger("touch");
        };
        bird.FlyToPosition(nextPos, nextBranch, firstBird ? callback : null);
        listBirdsOnBranch.Add(bird);
        numberOfBirdsInBranch++;
    }

    public void HighlightBird(bool active)
    {
        if (numberOfBirdsInBranch <= 0) return;
        List<BirdController> allHighLight = GetBirdsCanFly();
        foreach (BirdController b in allHighLight)
        {
            b.SetAnim(active ? BirdAnimation.TOUCHING : BirdAnimation.IDLE);
        }
    }

    public bool IsEmpty()
    {
        return listBirdsOnBranch.Count == 0 || numberOfBirdsInBranch <= 0;
    }

    public void AddBirdToSlotInit(BirdController bird, int slot, bool firstBird = false)
    {
        System.Action callback = () =>
        {
            animator.SetTrigger("touch");
        };
        bird.FlyToPosition(listSlotPosBirds[slot - 1].localPosition, this, firstBird ? callback : null);
        listBirdsOnBranch.Add(bird);
        numberOfBirdsInBranch++;
    }

    public void AddBirdFromOtherBranch(BranchController fromBranch)
    {
        List<BirdController> getAllBirdFromOtherBranch = fromBranch.GetBirdsCanFly();
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
        numberOfBirdsInBranch -= listBirdFly.Count;
    }

    public List<BirdController> GetBirdsCanFly()
    {
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
        if (numberOfBirdsInBranch == listSlotPosBirds.Count)
        {
            return Vector2.zero;
        }
        return listSlotPosBirds[numberOfBirdsInBranch].localPosition;
    }
}
