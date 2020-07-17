using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Controllers
{
    public class GameController
    {
        private int worldWidth;
        private int worldHeight;
        private int worldLenght;
        private GameModel gameModel;
        private TileService tileService;
        private TileFactory tileFactory;

        public GameController(int worldWidth, int worldHeight, int worldLenght)
        {
            tileService = new TileService();
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.worldLenght = worldLenght;
            tileFactory = new TileFactory();
        }

        public GameModel GetGameModel()
        {
            if(gameModel == null)
            {
                gameModel = new GameModel();
                gameModel.Inventory = new InventoryModel(9);
                gameModel.Inventory.HotBar[0] = TileType.DefaultTile;
                gameModel.Inventory.HotBar[1] = TileType.DirtTile;
                gameModel.Inventory.HotBar[2] = TileType.GrassTile;
                gameModel.Tiles = tileService.GenerateTileMap(worldWidth, worldHeight, worldLenght);
            }

            return gameModel;
        }

        public void DestroyTile(int x, int y, int z)
        {
            gameModel.Tiles[x, y, z] = null;
        }

        public void PlaceTile(int x, int y, int z, TileType tileType)
        {
            if (worldWidth > x && x >= 0 && worldHeight > y && y >= 0 && worldLenght > z && z >= 0)
            {
                gameModel.Tiles[x, y, z] = tileFactory.CreateTile(new Point(x, y, z), tileType);
            }
        }
    }
}
