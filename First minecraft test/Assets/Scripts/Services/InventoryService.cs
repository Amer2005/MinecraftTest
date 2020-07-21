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
            return GetBlockFromSlot(gameModel.Inventory.SelectedBlock, gameModel);
        }

        public InventorySlotModel GetBlockFromSlot(int slotNumber, GameModel gameModel)
        {
            return gameModel.Inventory.HotBar[slotNumber];
        }

        public void AddItemToInventory(TileType tileType, GameModel gameModel)
        {
            bool found = false;
            int firstZero = -1;

            for (int i = 0; i < gameModel.Inventory.HotBarSize; i++)
            {
                if (gameModel.Inventory.HotBar[i].ItemCount > 0)
                {
                    if (gameModel.Inventory.HotBar[i].Item == tileType)
                    {
                        gameModel.Inventory.HotBar[i].ItemCount++;
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
                gameModel.Inventory.HotBar[firstZero].Item = tileType;
                gameModel.Inventory.HotBar[firstZero].ItemCount = 1;
            }
        }
    }
}
