using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class ReferenceEventAnimation : MonoBehaviour
{
    public void ChangeAnim(string anim)
    {
        TutorialBird.Instance.ChangeAnim(anim);
    }

    public void StartOpenTextBoxEvent()
    {
        TutorialBird.Instance.StartOpenTextBoxEvent();
    }

    public void CloseBlackScreen()
    {
        UiGameplay.Instance.EndTutorial();
    }
}
