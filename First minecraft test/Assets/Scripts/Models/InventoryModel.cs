using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class InventoryModel
    {
        public int HotBarSize { get; }
        public InventorySlotModel[] HotBar { get; set; }

        public int SelectedBlock { get; set; }

        public InventoryModel(int hotBarSize)
        {
            HotBarSize = hotBarSize;
            HotBar = new InventorySlotModel[hotBarSize];

            for (int i = 0; i < HotBar.Length; i++)
            {
                HotBar[i] = new InventorySlotModel();
            }

            SelectedBlock = 0;
        }
    }
}
