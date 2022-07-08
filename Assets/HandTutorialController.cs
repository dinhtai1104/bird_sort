using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class HandTutorialController : MonoBehaviour
{

    public static HandTutorialController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject handTut;

    private void OnEnable()
    {
        EndTutorial();
    }
    public void OpenTutorial()
    {
        handTut.gameObject.SetActive(true);
    }

    public void ChangePosition(Vector2 pos)
    {
        handTut.transform.position = pos;
    }

    public void EndTutorial()
    {
        handTut.gameObject.SetActive(false);
    }
}
