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
            return gameModel.Player.Inventory.MainInventory[3, slotNumber];
        }

        public void AddItemToInventory(BlockType BlockType, GameModel gameModel)
        {
            bool found = false;
            int firstZero = -1;

            for (int i = 0; i < gameModel.Player.Inventory.MainInventory.GetLength(1); i++)
            {
                if (gameModel.Player.Inventory.MainInventory[3, i].ItemCount > 0)
                {
                    if (gameModel.Player.Inventory.MainInventory[3, i].Item.ItemType == ItemType.Blocks)
                    {
                        BlockItemModel block = (BlockItemModel)gameModel.Player.Inventory.MainInventory[3, i].Item;
                        if (block.BlockType == BlockType)
                        {
                            gameModel.Player.Inventory.MainInventory[3, i].ItemCount++;
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
                gameModel.Player.Inventory.MainInventory[3, firstZero].Item = block;
                gameModel.Player.Inventory.MainInventory[3, firstZero].ItemCount = 1;
            }
        }

        public void ClickSlotInInventory(int slotX, int slotY, GameModel gameModel)
        {
            if (gameModel.Player.Inventory.ItemOnCursor.ItemCount <= 0)
            {
                gameModel.Player.Inventory.ItemOnCursor = gameModel.Player.Inventory.MainInventory[slotX, slotY];
                gameModel.Player.Inventory.MainInventory[slotX, slotY] = new InventorySlotModel();
            }
            else if (gameModel.Player.Inventory.MainInventory[slotX, slotY].ItemCount <= 0)
            {
                gameModel.Player.Inventory.MainInventory[slotX, slotY] = gameModel.Player.Inventory.ItemOnCursor;
                gameModel.Player.Inventory.ItemOnCursor = new InventorySlotModel();
            }
            else
            {
                var temp = gameModel.Player.Inventory.MainInventory[slotX, slotY];
                gameModel.Player.Inventory.MainInventory[slotX, slotY] = gameModel.Player.Inventory.ItemOnCursor;
                gameModel.Player.Inventory.ItemOnCursor = temp;
            }
        }
    }
}
