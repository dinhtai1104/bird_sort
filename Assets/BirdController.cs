using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public int ID = 1;
    public BirdAnimation birdAnimation;

    private void Start()
    {
        ID = UnityEngine.Random.Range(1, 4);
        SetSkinBird(ID.ToString());
    }

    public void SetSkinBird(string skin)
    {
        birdAnimation.SetSkin(skin);
    }

    public void SetAnim(string nameOfAnim, System.Action callback = null)
    {
        birdAnimation.SetAnim(nameOfAnim, callback);
    }

    public void FlyToPosition(Vector2 targetPos, BranchController branch, System.Action callback = null)
    {
        StartCoroutine(FlyIE(targetPos, branch, callback));
    }

    IEnumerator FlyIE(Vector2 targetPos, BranchController branch, System.Action callback = null)
    {
        targetPos = branch.transform.TransformPoint(targetPos);
        yield return new WaitForSeconds(0.1f);
        SetAnim(BirdAnimation.FLY);
        Vector3 targetScale = Vector3.one;
        if (targetPos.x > 0)
        {
            targetScale = new Vector3(1, 1, 1);
        } else
        {
            targetScale = new Vector3(-1, 1, 1);
        }

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 13 * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPos) < 0.05f)
            {
                transform.position = targetPos;
                break;
            }
            yield return null;
        }
        //transform.localScale = targetScale;
        transform.SetParent(branch.transform);
        transform.localScale = new Vector2(Mathf.Sign(branch.transform.position.x) * Mathf.Sign(branch.transform.localScale.x), 1);

        SetAnim(BirdAnimation.GROUNDING, callback);
    }
}
