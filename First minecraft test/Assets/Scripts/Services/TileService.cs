using Assets.Scripts.Enums;
using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using Assets.Scripts.Models.InventoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class TileService
    {
        private TileFactory tileFactory;
        private RandomService randomService;
        private PerlinNoiseService perlinNoise;
        private InventoryService inventoryService;
        public TileService()
        {
            tileFactory = new TileFactory();
            randomService = new RandomService();
            perlinNoise = new PerlinNoiseService();
            inventoryService = new InventoryService();
        }

        public TileModel[,,] GenerateTileMap(int width, int height, int lenght)
        {
            TileModel[,,] tiles = new TileModel[width, height, lenght];

            int minHeight = 4;
            int maxHeight = 6;

           // tiles[0, 0, 0] = tileFactory.CreateTile(new Point(0, 0, 0), TileType.DirtTile);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < lenght; z++)
                {
                    int h = calculateHeight(x, z, 3, tiles);

                    tiles[x, h - 1, z] = tileFactory.CreateTile(new Point(x, (int)h - 1, z), TileType.GrassTile);

                    for (int y = 0;y < h - 1;y++)
                    {
                        tiles[x, y, z] = tileFactory.CreateTile(new Point(x, y, z), TileType.DirtTile);
                    }
                }
            }

            return tiles;
        }

        private int calculateHeight(float x, float y, float scale, TileModel[,,] tiles)
        {
            float xCord = (float)x / tiles.GetLength(0) * scale;
            float yCord = (float)y / tiles.GetLength(2) * scale;

            float sample = perlinNoise.perlinNoise(xCord, yCord);

            float height = sample * (tiles.GetLength(1) - 1); 

            return (int)Math.Round(height);
        }

        public void PlaceBlockFromSlectedSlot(int x, int y, int z, GameModel gameModel)
        {
            if (gameModel.Tiles.GetLength(0) > x && x >= 0 && gameModel.Tiles.GetLength(1) > y && y >= 0 && gameModel.Tiles.GetLength(2) > z && z >= 0)
            {
                InventorySlotModel slot = inventoryService.GetBlockFromSelectedSlot(gameModel);
                if (slot.ItemCount > 0)
                {
                    if (slot.Item.ItemType == ItemType.Blocks)
                    {
                        BlockItemModel block = (BlockItemModel)slot.Item;
                        gameModel.Tiles[x, y, z] = tileFactory.CreateTile(new Point(x, y, z), block.TileType);
                        if (gameModel.Player.Gamemode == Gamemodes.Survival)
                        {
                            gameModel.Player.Inventory.HotBar[gameModel.Player.Inventory.SelectedBlock].ItemCount--;
                        }
                    }
                }
            }
        }

        public void PlaceBlock(int x, int y, int z, TileType tileType, GameModel gameModel)
        {
            if (gameModel.Tiles.GetLength(0) > x && x >= 0 && gameModel.Tiles.GetLength(1) > y && y >= 0 && gameModel.Tiles.GetLength(2) > z && z >= 0)
            {
                gameModel.Tiles[x, y, z] = tileFactory.CreateTile(new Point(x, y, z), tileType);
            }
        }

        public void BreakBlock(int x, int y, int z, GameModel gameModel)
        {
            if (gameModel.Tiles.GetLength(0) > x && x >= 0 && gameModel.Tiles.GetLength(1) > y && y >= 0 && gameModel.Tiles.GetLength(2) > z && z >= 0)
            {
                if (gameModel.Tiles[x, y, z] != null)
                {
                    TileType tileType = gameModel.Tiles[x, y, z].TileType;

                    if (gameModel.Player.Gamemode == Gamemodes.Survival)
                    {
                        inventoryService.AddItemToInventory(tileType, gameModel);
                    }

                    gameModel.Tiles[x, y, z] = null;
                }
            }
        }
    }
}
