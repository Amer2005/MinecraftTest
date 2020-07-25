using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Controllers
{
    public class GameController
    {
        private int worldWidth;
        private int worldHeight;
        private int worldLenght;
        private GameModel gameModel;
        private BlockService blockService;

        public GameController(int worldWidth, int worldHeight, int worldLenght)
        {
            blockService = new BlockService();
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.worldLenght = worldLenght;
        }

        public GameModel GetGameModel()
        {
            if(gameModel == null)
            {
                gameModel = new GameModel();
                gameModel.Player = new PlayerModel(GamemodeType.Survival);
                gameModel.Blocks = blockService.GenerateBlockMap(worldWidth, worldHeight, worldLenght);
            }

            return gameModel;
        }
    }
}
