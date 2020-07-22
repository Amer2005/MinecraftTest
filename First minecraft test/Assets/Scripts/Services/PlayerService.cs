using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class PlayerService
    {
        private TileService tileService;

        public PlayerService()
        {
            tileService = new TileService();
        }

        public void LeftClick(int x, int y, int z, GameModel gameModel)
        {
            tileService.BreakBlock(x, y, z, gameModel);
        }

        public void RightClick(int x, int y, int z, GameModel gameModel)
        {
            tileService.PlaceBlockFromSlectedSlot(x, y, z, gameModel); 
        }
    }
}
