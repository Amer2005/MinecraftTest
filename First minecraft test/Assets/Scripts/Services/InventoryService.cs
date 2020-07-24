using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using Assets.Scripts.Models.InventoryModels;
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

        public void AddItemToInventory(BlockType BlockType, GameModel gameModel)
        {
            bool found = false;
            int firstZero = -1;

            for (int i = 0; i < gameModel.Player.Inventory.HotBar.Length; i++)
            {
                if (gameModel.Player.Inventory.HotBar[i].ItemCount > 0)
                {
                    if (gameModel.Player.Inventory.HotBar[i].Item.ItemType == ItemType.Blocks)
                    {
                        BlockItemModel block = (BlockItemModel)gameModel.Player.Inventory.HotBar[i].Item;
                        if (block.BlockType == BlockType)
                        {
                            gameModel.Player.Inventory.HotBar[i].ItemCount++;
                            found = true;
                            break;
                        }
                    }
                }
                else if (firstZero == -1)
                {
                    firstZero = i;
                }
            }

            if (!found && firstZero != -1)
            {
                BlockItemModel block = new BlockItemModel(BlockType);
                gameModel.Player.Inventory.HotBar[firstZero].Item = block;
                gameModel.Player.Inventory.HotBar[firstZero].ItemCount = 1;
            }
        }
    }
}
