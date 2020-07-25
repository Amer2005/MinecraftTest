using Assets.Scripts.Enums;
using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Buildings;
using Assets.Scripts.Models.InventoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class BlockService
    {
        private BlockFactory BlockFactory;
        private RandomService randomService;
        private PerlinNoiseService perlinNoise;
        private InventoryService inventoryService;
        private BuildingService buildingService;
        public BlockService()
        {
            BlockFactory = new BlockFactory();
            randomService = new RandomService();
            perlinNoise = new PerlinNoiseService();
            inventoryService = new InventoryService();
            buildingService = new BuildingService();
        }

        public BlockModel[,,] GenerateBlockMap(int width, int height, int lenght)
        {
            BlockModel[,,] blocks = new BlockModel[width, height, lenght];

            int minHeight = 1;
            int maxHeight = 10;

           // Blocks[0, 0, 0] = BlockFactory.CreateBlock(new Point(0, 0, 0), BlockType.DirtBlock);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < lenght; z++)
                {
                    int h = calculateHeight(x, z, 3, blocks, minHeight, maxHeight);

                    h--;

                    int rng = randomService.GenerateInt(1, 101);

                    blocks[x, h, z] = BlockFactory.CreateBlock(new Point(x, h, z), BlockType.GrassBlock);

                    if (rng <= 1)
                    {
                        buildingService.PlaceBuilding(new Point(x, h + 1, z), new TreeBuilding(), blocks);
                        //blocks[x, h, z] = BlockFactory.CreateBlock(new Point(x, h, z), BlockType.DirtBlock);
                    }

                    for (int y = 0;y < h;y++)
                    {
                        blocks[x, y, z] = BlockFactory.CreateBlock(new Point(x, y, z), BlockType.DirtBlock);
                    }
                }
            }

            return blocks;
        }

        private int calculateHeight(float x, float y, float scale, BlockModel[,,] Blocks, int minHeight, int maxHeight)
        {
            float xCord = (float)x / Blocks.GetLength(0) * scale;
            float yCord = (float)y / Blocks.GetLength(2) * scale;

            float sample = perlinNoise.perlinNoise(xCord, yCord);

            float height = sample * ((maxHeight - minHeight)) + minHeight; 

            return (int)Math.Round(height);
        }

        public void PlaceBlockFromSlectedSlot(int x, int y, int z, GameModel gameModel)
        {
            if (gameModel.Blocks.GetLength(0) > x && x >= 0 && gameModel.Blocks.GetLength(1) > y && y >= 0 && gameModel.Blocks.GetLength(2) > z && z >= 0)
            {
                InventorySlotModel slot = inventoryService.GetBlockFromSelectedSlot(gameModel);
                if (slot.ItemCount > 0)
                {
                    if (slot.Item.ItemType == ItemType.Blocks)
                    {
                        BlockItemModel block = (BlockItemModel)slot.Item;
                        gameModel.Blocks[x, y, z] = BlockFactory.CreateBlock(new Point(x, y, z), block.BlockType);
                        if (gameModel.Player.Gamemode == GamemodeType.Survival)
                        {
                            gameModel.Player.Inventory.MainInventory[3, gameModel.Player.Inventory.SelectedBlock].ItemCount--;
                        }
                    }
                }
            }
        }

        public void PlaceBlock(int x, int y, int z, BlockType BlockType, GameModel gameModel)
        {
            if (gameModel.Blocks.GetLength(0) > x && x >= 0 && gameModel.Blocks.GetLength(1) > y && y >= 0 && gameModel.Blocks.GetLength(2) > z && z >= 0)
            {
                gameModel.Blocks[x, y, z] = BlockFactory.CreateBlock(new Point(x, y, z), BlockType);
            }
        }

        public void BreakBlock(int x, int y, int z, GameModel gameModel)
        {
            if (gameModel.Blocks.GetLength(0) > x && x >= 0 && gameModel.Blocks.GetLength(1) > y && y >= 0 && gameModel.Blocks.GetLength(2) > z && z >= 0)
            {
                if (gameModel.Blocks[x, y, z] != null)
                {
                    BlockType BlockType = gameModel.Blocks[x, y, z].BlockType;

                    if (gameModel.Player.Gamemode == GamemodeType.Survival)
                    {
                        inventoryService.AddItemToInventory(BlockType, gameModel);
                    }

                    gameModel.Blocks[x, y, z] = null;
                }
            }
        }
    }
}
