using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;

public class TileModel
{
    public TileType TileType { get; set; }

    public Point Point { get; set; }

    public TileModel(Point point, TileType tileType)
    {
        Point = point;
        TileType = tileType;
    }
}
