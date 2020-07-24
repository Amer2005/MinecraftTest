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

        public bool CanYouPlaceBuilding(Point point, BuildingModel building, BlockModel[,,] blocks)
        {
            if (point.X < 0 || point.Y < 0 || point.Z < 0)
            {
                return false;
            }

            if (point.X > blocks.GetLength(0) - 1 || point.Y > blocks.GetLength(1) - 1 || point.Z > blocks.GetLength(2) - 1)
            {
                return false;
            }

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

                            if(blocks.GetLength(0) - 1 < blocksX || blocks.GetLength(1) - 1 < blocksY || blocks.GetLength(2) - 1 < blocksZ)
                            {
                                return false;
                            }

                            if (blocks[blocksX, blocksY, blocksZ] != null)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void PlaceBuilding(Point point, BuildingModel building, BlockModel[,,] blocks)
        {
            point = point - building.StartPoint;

            if (!CanYouPlaceBuilding(point, building, blocks))
            {
                return;
            }

            for (int x = 0; x < building.Width; x++)
            {
                for (int y = 0; y < building.Height; y++)
                {
                    for (int z = 0; z < building.Lenght; z++)
                    {
                        if (building.BuildingBlocks[x, y, z] != null)
                        {
                            int blocksX = x + point.X;
                            int blocksY = y + point.Y;
                            int blocksZ = z + point.Z;

                            blocks[blocksX, blocksY, blocksZ] = blockFactory.CreateBlock(new Point(blocksX, blocksY, blocksZ), building.BuildingBlocks[x, y, z].BlockType);
                        }
                    }
                }
            }
        }
    }
}
