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
        public BlockType BlockType { get; set; }

        public BlockItemModel(BlockType blockType) : base(ItemType.Blocks)
        {
            BlockType = blockType;
        }
    }
}
