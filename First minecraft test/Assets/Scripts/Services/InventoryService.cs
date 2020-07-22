using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class InventoryService
    {
        public InventorySlotModel GetBlockFromSelectedSlot(GameModel gameModel)
        {
            return GetBlockFromSlot(gameModel.Player.Inventory.SelectedBlock, gameModel);
        }

        public InventorySlotModel GetBlockFromSlot(int slotNumber, GameModel gameModel)
        {
            return gameModel.Player.Inventory.HotBar[slotNumber];
        }

        public void AddItemToInventory(TileType tileType, GameModel gameModel)
        {
            bool found = false;
            int firstZero = -1;

            for (int i = 0; i < gameModel.Player.Inventory.HotBar.Length; i++)
            {
                if (gameModel.Player.Inventory.HotBar[i].ItemCount > 0)
                {
                    if (gameModel.Player.Inventory.HotBar[i].Item == tileType)
                    {
                        gameModel.Player.Inventory.HotBar[i].ItemCount++;
                        found = true;
                        break;
                    }
                }
                else if (firstZero == -1)
                {
                    firstZero = i;
                }
            }

            if (!found && firstZero != -1)
            {
                gameModel.Player.Inventory.HotBar[firstZero].Item = tileType;
                gameModel.Player.Inventory.HotBar[firstZero].ItemCount = 1;
            }
        }
    }
}
