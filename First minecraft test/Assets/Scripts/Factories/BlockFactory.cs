using Assets.Scripts.Models;
using Assets.Scripts.Models.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Factories
{
    public class BlockFactory
    {
        public BlockModel CreateBlock(Point point, BlockType BlockType)
        {
            switch(BlockType)
            {
                case BlockType.DefaultBlock:
                    return new DefaultBlock(point);
                case BlockType.DirtBlock:
                    return new DirtBlock(point);
                case BlockType.GrassBlock:
                    return new GrassBlock(point);
                case BlockType.LogBlock:
                    return new LogBlock(point);
                case BlockType.LeafBlock:
                    return new LeafBlock(point);
                default:
                    throw new Exception("Block not found");
            }
        }
    }
}
