using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using Assets.Scripts.Models.InventoryModels;
using System.Collections;
using System.Collections.Generic;

public class BlockModel
{
    public BlockType BlockType { get; set; }

    public Point Point { get; set; }

    public BlockModel(Point point, BlockType blockType)
    {
        Point = point;
        BlockType = blockType;
    }
}
