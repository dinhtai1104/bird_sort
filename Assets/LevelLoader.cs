using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class LevelLoader
{
    private static Dictionary<int, LevelModel> levelLoaded = new Dictionary<int, LevelModel>();

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
}
