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
        public TreeBuilding() : base(1, 2, 1)
        {
            BuildingBlocks[0, 0, 0] = new LogBlock(new Point(0, 0, 0));
            BuildingBlocks[0, 1, 0] = new LogBlock(new Point(0, 1, 0));
        }
    }
}
