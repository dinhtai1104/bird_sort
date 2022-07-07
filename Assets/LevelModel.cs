using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModel
{
    public int branch;
    public int max;
    public List<Piece> pieces;
}

[System.Serializable]
public class Piece
{
    public int id;
    public int type;
    public int branch;
    public int possition;
}

[System.Serializable]
public class LevelModelFC
{
    public List<PieceFC> standConfig;
}

[System.Serializable] 
public class StandConfigFC
{
}

[System.Serializable]
public class PieceFC
{
    public int[] idBirds;
    public StandConfigFC standData;
    public int side;
}

[System.Serializable]
public class StandDataFC
{
    public int type;
    public int numSlot;
}