using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    None,
    Row,
    Column,
    Adjacent,
    Color
}
public class Bomb : GamePiece
{
    public BombType bombType;
}
