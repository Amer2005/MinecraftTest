using Assets.Scripts.Enums;
using Assets.Scripts.Models.InventoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class InventorySlotModel
    {
        public ItemModel Item { get; set; }
        public int ItemCount { get; set; }

        public InventorySlotModel()
        {
            Item = new ItemModel();
            ItemCount = 0;
        }
    }
}
