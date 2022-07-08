using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class AnimationButton : MonoBehaviour, IPointerClickHandler
{
    private Button btn;
    private void OnEnable()
    {
        if (!btn)
        {
            btn = GetComponent<Button>();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!btn)
        {
            btn = GetComponent<Button>();
        }
        if (btn)
        {
            //if (btn.interactable)
            {
                SoundManager.Instance.PlaySFXSound(SoundManager.Instance.buttonClick);
                transform.DOScale(Vector3.one * 1.1f, 0.05f).OnComplete(() =>
                {
                    transform.DOScale(Vector3.one, 0.05f);
                });
            }
        }
    }
}
