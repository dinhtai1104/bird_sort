using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public static class LevelLoader
{
    private static Dictionary<int, LevelModel> levelLoaded = new Dictionary<int, LevelModel>();
    private static Dictionary<int, LevelModelFC> levelLoadedFC = new Dictionary<int, LevelModelFC>();

    public static LevelModel GetLevelModel(int pLevel)
    {
        if (levelLoaded.ContainsKey(pLevel))
        {
            return levelLoaded[pLevel];
        }

        TextAsset textAsset = Resources.Load<TextAsset>("Level/lvl" + pLevel);
        string jsonValue = textAsset.text;
        LevelModel level = JsonConvert.DeserializeObject<LevelModel>(jsonValue);
        if (level != null)
        {
            levelLoaded.Add(pLevel, level);
            return level;
        }
        return null;
    }

    public static LevelModelFC GetLevelModelFC(int pLevel)
    {
        if (levelLoadedFC.ContainsKey(pLevel))
        {
            return levelLoadedFC[pLevel];
        }

        TextAsset textAsset = Resources.Load<TextAsset>("Level2/Level_" + pLevel);
        string jsonValue = textAsset.text;
        LevelModelFC level = JsonConvert.DeserializeObject<LevelModelFC>(jsonValue);
        if (level != null)
        {
            levelLoadedFC.Add(pLevel, level);
            return level;
        }
        return null;
    }
}
