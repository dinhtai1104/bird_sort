using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class SkinBirdData : MonoBehaviour
{
	public static SkinBirdData _instance;
    public static SkinBirdData Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<SkinBirdData>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    public List<SkeletonDataAsset> listDataBirds;
    public SkeletonDataAsset GetSkeletonSkin(int id)
    {
        return listDataBirds[id];
    }
}

[System.Serializable]
public class DataBird
{
	public SkeletonDataAsset dataAssetSkin;
}