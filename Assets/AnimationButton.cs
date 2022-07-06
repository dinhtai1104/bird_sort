using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class AnimationButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFXSound(SoundManager.Instance.buttonClick);
    }
}
