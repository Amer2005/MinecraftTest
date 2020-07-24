using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class BuildingService
    {
        private BlockFactory blockFactory;

        public BuildingService()
        {
            blockFactory = new BlockFactory();
        }

        public bool CanYouPlaceBuilding(Point point, BuildingModel building, GameModel gameModel)
        {
            for (int x = 0; x < building.Width; x++)
            {
                for (int y = 0; y < building.Height; y++)
                {
                    for (int z = 0; z < building.Lenght; z++)
                    {
                        if(building.BuildingBlocks[x, y, z] != null)
                        {
                            int blocksX = x + point.X;
                            int blocksY = y + point.Y;
                            int blocksZ = z + point.Z;

                            if (gameModel.Blocks[blocksX, blocksY, blocksZ] != null)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void PlaceBuilding(Point point, BuildingModel building, GameModel gameModel)
        {
            if (!CanYouPlaceBuilding(point, building, gameModel))
            {
                return;
            }

            for (int x = 0; x < building.Width; x++)
            {
                for (int y = 0; y < building.Height; y++)
                {
                    for (int z = 0; z < building.Lenght; z++)
                    {
                        int blocksX = x + point.X;
                        int blocksY = y + point.Y;
                        int blocksZ = z + point.Z;

                        gameModel.Blocks[blocksX, blocksY, blocksZ] = blockFactory.CreateBlock(new Point(blocksX, blocksY, blocksZ), building.BuildingBlocks[x, y, z].BlockType);
                    }
                }
            }
        }
    }
}
