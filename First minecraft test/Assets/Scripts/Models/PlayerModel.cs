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

        public Gamemodes Gamemode { get; set; }

        public PlayerModel(Gamemodes gamemode)
        {
            Inventory = new InventoryModel();
            Gamemode = gamemode;

            if(Gamemode == Gamemodes.Creative)
            {
                Inventory.HotBar[0].Item = new BlockItemModel(BlockType.DefaultBlock);
                Inventory.HotBar[1].Item = new BlockItemModel(BlockType.DirtBlock);
                Inventory.HotBar[2].Item = new BlockItemModel(BlockType.GrassBlock);
                Inventory.HotBar[3].Item = new BlockItemModel(BlockType.LogBlock);
                Inventory.HotBar[4].Item = new BlockItemModel(BlockType.LeafBlock);

                for (int i = 0; i < 5; i++)
                {
                    Inventory.HotBar[i].ItemCount = 1;
                }
            }
        }
    }
}
