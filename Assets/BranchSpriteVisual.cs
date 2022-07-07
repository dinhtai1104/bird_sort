using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchSpriteVisual : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<Sprite> branchSprites;

    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteBranch(Sprite branchSpr)
    {
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        this.spriteRenderer.sprite = branchSpr;
    }

    private void OnEnable()
    {
        SetSpriteBranch(branchSprites[Random.Range(0, branchSprites.Count)]);
    }
}
