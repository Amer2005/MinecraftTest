using Assets.Scripts.Enums;
using Assets.Scripts.Models.InventoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class PlayerModel
    {
        public InventoryModel Inventory { get; set; }

        public GamemodeType Gamemode { get; set; }

        public PlayerModel(GamemodeType gamemode)
        {
            Inventory = new InventoryModel();
            Gamemode = gamemode;

            if(Gamemode == GamemodeType.Creative)
            {
                Inventory.MainInventory[3,0].Item = new BlockItemModel(BlockType.DefaultBlock);
                Inventory.MainInventory[3,1].Item = new BlockItemModel(BlockType.DirtBlock);
                Inventory.MainInventory[3,2].Item = new BlockItemModel(BlockType.GrassBlock);
                Inventory.MainInventory[3,3].Item = new BlockItemModel(BlockType.LogBlock);
                Inventory.MainInventory[3,4].Item = new BlockItemModel(BlockType.LeafBlock);

                for (int i = 0; i < 5; i++)
                {
                    Inventory.MainInventory[3, i].ItemCount = 1;
                }
            }
        }
    }
}
