using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.InventoryModels
{
    public class ItemModel
    {
        public ItemType ItemType { get; set; }

        public ItemModel() { }

        public ItemModel(ItemType itemType)
        {
            ItemType = itemType;
        }
    }
}
