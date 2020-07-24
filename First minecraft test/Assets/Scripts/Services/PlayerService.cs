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
        private BlockService BlockService;

        public PlayerService()
        {
            BlockService = new BlockService();
        }

        public void LeftClick(int x, int y, int z, GameModel gameModel)
        {
            BlockService.BreakBlock(x, y, z, gameModel);
        }

        public void RightClick(int x, int y, int z, GameModel gameModel)
        {
            BlockService.PlaceBlockFromSlectedSlot(x, y, z, gameModel); 
        }
    }
}
