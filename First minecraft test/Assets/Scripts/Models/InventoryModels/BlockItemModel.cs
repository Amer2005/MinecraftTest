using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Models.InventoryModels
{
    public class BlockItemModel : ItemModel
    {
        public TileType TileType { get; set; }

        public BlockItemModel(TileType tileType) : base(ItemType.Blocks)
        {
            TileType = tileType;
        }
    }
}
