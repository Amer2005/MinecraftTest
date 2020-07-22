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
                gameModel.Player = new PlayerModel();
                gameModel.Tiles = tileService.GenerateTileMap(worldWidth, worldHeight, worldLenght);
            }

            return gameModel;
        }
    }
}
