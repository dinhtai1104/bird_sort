using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BirdController : MonoBehaviour
{
    public int ID = 1;
    public BirdAnimation birdAnimation;
    public ParticleSystem birdReleaseEffect;
    private bool isRelease = false;

    private void Start()
    {
        //ID = UnityEngine.Random.Range(1, 4);
        //SetSkinBird(ID.ToString());
    }

    private void OnEnable()
    {
        isRelease = false;
    }

    public bool IsReleased()
    {
        return isRelease;
    }
    public void SetRelease(bool release)
    {
        this.isRelease = release;
        if (release)
        {
            ClearTrack();
        }
    }
    public void PlayBirdReleaseEffect(bool play = true)
    {
        if (play)
        {
            birdReleaseEffect.Play();
        }
        else
        {
            birdReleaseEffect.Stop();
        }
    }

    public void SetSkinBird(string skin)
    {
        birdAnimation.SetSkin(skin);
    }

    public void SetAnim(string nameOfAnim, System.Action callback = null, bool loop = false)
    {
        birdAnimation.SetAnim(nameOfAnim, callback, loop);
    }

    public void FlyToPosition(Vector2 targetPos, BranchController branch, System.Action callback = null, bool first = true)
    {
        StopAllCoroutines();
        StartCoroutine(FlyIE(targetPos, branch, callback, first));
    }

    public void AddAnimation(string anim)
    {
        birdAnimation.AddAnimation(anim);
    }

    IEnumerator FlyIE(Vector2 targetPos, BranchController branch, System.Action callback = null, bool first = true)
    {
        branch.HighlightBird(false);
        targetPos = branch.transform.TransformPoint(targetPos);
        List<Vector2> path = new List<Vector2>();
        if (!first)
        {
            path = branch.branchBezierPath.CreatePath(targetPos);
            
        } else
        {
            path.Add(transform.position);
            path.Add(targetPos);
        }
        Debug.Log(path.Count);
        yield return new WaitForSeconds(0.1f);
        SetAnim(BirdAnimation.FLY,null, true);
        Vector3 targetScale = Vector3.one;
        if (targetPos.x > 0)
        {
            targetScale = new Vector3(1, 1, 1);
        } else
        {
            targetScale = new Vector3(-1, 1, 1);
        }

        if (targetPos.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        int index = 0;
        while (index < path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[index], 10 * Time.deltaTime);
            if (Vector2.Distance(transform.position, path[index]) < 0.05f)
            {
                transform.position = path[index];
                index++;
                //break;
            }
            yield return null;
        }
        transform.SetParent(branch.transform);
        transform.localScale = new Vector2(Mathf.Sign(branch.transform.position.x) * Mathf.Sign(branch.transform.localScale.x), 1);

        SetAnim(BirdAnimation.GROUNDING, callback);
    }

    public void SetID(int type)
    {
        this.ID = type;

        int bird = ID / 3;
        int skin = ID % 3 + 1;

        //Debug.Log("Bird: " + bird + " - Skin: " + skin);

        SkeletonDataAsset data = SkinBirdData.Instance.GetSkeletonSkin(bird);
        SetSkeletonSkin(data);
        SetSkinBird(skin.ToString());
    }

    private void SetSkeletonSkin(SkeletonDataAsset data)
    {
        birdAnimation.SetSkeletonData(data);
    }


    public void FlyToOtherPos(Vector2 targetPos)
    {
        StartCoroutine(FlyToPos(targetPos));
    }

    public void ClearTrack()
    {
        birdAnimation.ClearTrack();
    }

    private IEnumerator FlyToPos(Vector2 targetPos)
    {
        yield return new WaitForSeconds(0.1f);
        SetAnim(BirdAnimation.FLY, null, true);
        transform.SetParent(null);
        float speed = UnityEngine.Random.Range(10, 25);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPos) < 0.05f)
            {
                transform.position = targetPos;
                break;
            }
            yield return null;
        }
    }
}
