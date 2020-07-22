using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using Assets.Scripts.Models.InventoryModels;
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
