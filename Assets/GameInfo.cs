using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public static class GameInfo
{
	private static readonly string GAME = "TGAME";

	private static readonly string SOUND = "SOUND";
	private static readonly string MUSIC = "MUSIC";
	private static readonly string VIBRATE = "VIBRATE";

	private static readonly string COIN = "COIN";
	private static readonly string LEVEL = "LEVEL";


	private static readonly string BRANCH = "BRANCH";
	private static readonly string UNDO = "UNDO";
	private static readonly string SHUFF = "SHUFF";

	private static bool sound = true;
	public static bool Sound
    {
		get
        {
			return sound;
        }
		set
        {
			sound = value;
			PlayerPrefs.SetInt(SOUND, sound ? 1 : 0);
        }
    }

	private static bool music = true;
	public static bool Music
	{
		get
		{
			return music;
		}
		set
		{
			music = value;
			PlayerPrefs.SetInt(MUSIC, sound ? 1 : 0);
		}
	}

	private static bool vibration = true;
	public static bool Vibration
	{
		get
		{
			return vibration;
		}
		set
		{
			vibration = value;
			PlayerPrefs.SetInt(VIBRATE, sound ? 1 : 0);
		}
	}

	private static int level;
	public static int Level
    {
		get
        {
			return level;
        }
		set
        {
			level = value;
			PlayerPrefs.SetInt(LEVEL, level);
		}
	}
	private static int coin;
	public static int Coin
	{
		get
		{
			return coin;
		}
		set
		{
			coin = value;
			PlayerPrefs.SetInt(COIN, coin);
		}
	}


	private static int branch = 7;
	public static int Branch
    {
		get
        {
			return branch;
        }
		set
        {
			branch = value;
			PlayerPrefs.SetInt(BRANCH, branch);
        }
    }
	
	private static int undo = 7;
	public static int Undo
	{
		get
		{
			return undo;
		}
		set
		{
			undo = value;
			PlayerPrefs.SetInt(UNDO, undo);
		}
	}

	private static int shuff = 7;
	public static int Shuff
	{
		get
		{
			return shuff;
		}
		set
		{
			shuff = value;
			PlayerPrefs.SetInt(SHUFF, shuff);
		}
	}

	public static void LoadData()
    {
		if (!PlayerPrefs.HasKey(GAME))
        {
			PlayerPrefs.SetFloat(GAME, 1);
			Sound = true;
			Music = true;
			Vibration = true;
			Level = 1;
			Coin = 0;
			Branch = 7;
			Shuff = 5;
		}
		Level = PlayerPrefs.GetInt(LEVEL);
		Sound = PlayerPrefs.GetInt(SOUND) == 1;
		Music = PlayerPrefs.GetInt(MUSIC) == 1;
		Vibration = PlayerPrefs.GetInt(VIBRATE) == 1;
		Coin = PlayerPrefs.GetInt(COIN);

		Branch = PlayerPrefs.GetInt(BRANCH);
		Shuff = PlayerPrefs.GetInt(SHUFF);
	}
}
