using Assets.Scripts.Models.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Buildings
{
    class TreeBuilding : BuildingModel
    {
        public TreeBuilding() : base(5, 7, 5, new Point(2,0,2))
        {
            BuildingBlocks[0, 3, 0] = new LeafBlock(new Point(0, 3, 0));
            BuildingBlocks[0, 3, 1] = new LeafBlock(new Point(0, 3, 1));
            BuildingBlocks[0, 3, 2] = new LeafBlock(new Point(0, 3, 2));
            BuildingBlocks[0, 3, 3] = new LeafBlock(new Point(0, 3, 3));
            BuildingBlocks[0, 3, 4] = new LeafBlock(new Point(0, 3, 4));
            BuildingBlocks[0, 4, 1] = new LeafBlock(new Point(0, 4, 1));
            BuildingBlocks[0, 4, 2] = new LeafBlock(new Point(0, 4, 2));
            BuildingBlocks[0, 4, 3] = new LeafBlock(new Point(0, 4, 3));
            BuildingBlocks[1, 3, 0] = new LeafBlock(new Point(1, 3, 0));
            BuildingBlocks[1, 3, 1] = new LeafBlock(new Point(1, 3, 1));
            BuildingBlocks[1, 3, 2] = new LeafBlock(new Point(1, 3, 2));
            BuildingBlocks[1, 3, 3] = new LeafBlock(new Point(1, 3, 3));
            BuildingBlocks[1, 3, 4] = new LeafBlock(new Point(1, 3, 4));
            BuildingBlocks[1, 4, 0] = new LeafBlock(new Point(1, 4, 0));
            BuildingBlocks[1, 4, 1] = new LeafBlock(new Point(1, 4, 1));
            BuildingBlocks[1, 4, 2] = new LeafBlock(new Point(1, 4, 2));
            BuildingBlocks[1, 4, 3] = new LeafBlock(new Point(1, 4, 3));
            BuildingBlocks[1, 4, 4] = new LeafBlock(new Point(1, 4, 4));
            BuildingBlocks[1, 5, 1] = new LeafBlock(new Point(1, 5, 1));
            BuildingBlocks[1, 5, 2] = new LeafBlock(new Point(1, 5, 2));
            BuildingBlocks[1, 5, 3] = new LeafBlock(new Point(1, 5, 3));
            BuildingBlocks[1, 6, 2] = new LeafBlock(new Point(1, 6, 2));
            BuildingBlocks[2, 0, 2] = new LogBlock(new Point(2, 0, 2));
            BuildingBlocks[2, 1, 2] = new LogBlock(new Point(2, 1, 2));
            BuildingBlocks[2, 2, 2] = new LogBlock(new Point(2, 2, 2));
            BuildingBlocks[2, 3, 0] = new LeafBlock(new Point(2, 3, 0));
            BuildingBlocks[2, 3, 1] = new LeafBlock(new Point(2, 3, 1));
            BuildingBlocks[2, 3, 2] = new LogBlock(new Point(2, 3, 2));
            BuildingBlocks[2, 3, 3] = new LeafBlock(new Point(2, 3, 3));
            BuildingBlocks[2, 3, 4] = new LeafBlock(new Point(2, 3, 4));
            BuildingBlocks[2, 4, 0] = new LeafBlock(new Point(2, 4, 0));
            BuildingBlocks[2, 4, 1] = new LeafBlock(new Point(2, 4, 1));
            BuildingBlocks[2, 4, 2] = new LogBlock(new Point(2, 4, 2));
            BuildingBlocks[2, 4, 3] = new LeafBlock(new Point(2, 4, 3));
            BuildingBlocks[2, 4, 4] = new LeafBlock(new Point(2, 4, 4));
            BuildingBlocks[2, 5, 1] = new LeafBlock(new Point(2, 5, 1));
            BuildingBlocks[2, 5, 2] = new LogBlock(new Point(2, 5, 2));
            BuildingBlocks[2, 5, 3] = new LeafBlock(new Point(2, 5, 3));
            BuildingBlocks[2, 6, 1] = new LeafBlock(new Point(2, 6, 1));
            BuildingBlocks[2, 6, 2] = new LeafBlock(new Point(2, 6, 2));
            BuildingBlocks[2, 6, 3] = new LeafBlock(new Point(2, 6, 3));
            BuildingBlocks[3, 3, 0] = new LeafBlock(new Point(3, 3, 0));
            BuildingBlocks[3, 3, 1] = new LeafBlock(new Point(3, 3, 1));
            BuildingBlocks[3, 3, 2] = new LeafBlock(new Point(3, 3, 2));
            BuildingBlocks[3, 3, 3] = new LeafBlock(new Point(3, 3, 3));
            BuildingBlocks[3, 3, 4] = new LeafBlock(new Point(3, 3, 4));
            BuildingBlocks[3, 4, 0] = new LeafBlock(new Point(3, 4, 0));
            BuildingBlocks[3, 4, 1] = new LeafBlock(new Point(3, 4, 1));
            BuildingBlocks[3, 4, 2] = new LeafBlock(new Point(3, 4, 2));
            BuildingBlocks[3, 4, 3] = new LeafBlock(new Point(3, 4, 3));
            BuildingBlocks[3, 4, 4] = new LeafBlock(new Point(3, 4, 4));
            BuildingBlocks[3, 5, 1] = new LeafBlock(new Point(3, 5, 1));
            BuildingBlocks[3, 5, 2] = new LeafBlock(new Point(3, 5, 2));
            BuildingBlocks[3, 5, 3] = new LeafBlock(new Point(3, 5, 3));
            BuildingBlocks[3, 6, 2] = new LeafBlock(new Point(3, 6, 2));
            BuildingBlocks[4, 3, 0] = new LeafBlock(new Point(4, 3, 0));
            BuildingBlocks[4, 3, 1] = new LeafBlock(new Point(4, 3, 1));
            BuildingBlocks[4, 3, 2] = new LeafBlock(new Point(4, 3, 2));
            BuildingBlocks[4, 3, 3] = new LeafBlock(new Point(4, 3, 3));
            BuildingBlocks[4, 3, 4] = new LeafBlock(new Point(4, 3, 4));
            BuildingBlocks[4, 4, 1] = new LeafBlock(new Point(4, 4, 1));
            BuildingBlocks[4, 4, 2] = new LeafBlock(new Point(4, 4, 2));
            BuildingBlocks[4, 4, 3] = new LeafBlock(new Point(4, 4, 3));
        }
    }
}
