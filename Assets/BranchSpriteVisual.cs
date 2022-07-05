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
        this.spriteRenderer.sprite = branchSpr;
    }

    private void Start()
    {
        SetSpriteBranch(branchSprites[Random.Range(0, branchSprites.Count)]);
    }
}
