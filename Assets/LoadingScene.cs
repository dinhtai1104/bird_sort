using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class LoadingScene : MonoBehaviour
{
    public Image loadingFilled;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        AsyncOperation async = SceneManager.LoadSceneAsync("BirdSort");
        async.allowSceneActivation = false;
        GameInfo.LoadData();
        loadingFilled.DOFillAmount(1, 2f).From(0);
        yield return new WaitForSeconds(2f);
        async.allowSceneActivation = true;

    }

   
}
