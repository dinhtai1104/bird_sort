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