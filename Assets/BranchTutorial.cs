using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchTutorial : MonoBehaviour
{
    public GameObject tickV, tickX;
    private void OnEnable()
    {
        tickV.SetActive(false);
        tickX.SetActive(false);

        Vector3 localS = transform.localScale;
        tickV.transform.localScale = tickX.transform.localScale = localS;
    }


    public void ActiveV()
    {
        Vector3 localS = transform.localScale;
        tickV.transform.localScale = tickX.transform.localScale = localS;
        tickV.SetActive(true);
    }

    public void ActiveX()
    {
        Vector3 localS = transform.localScale;
        tickV.transform.localScale = tickX.transform.localScale = localS;
        tickX.SetActive(true);
    }

    public void DisableAll()
    {
        OnEnable();
    }
}
