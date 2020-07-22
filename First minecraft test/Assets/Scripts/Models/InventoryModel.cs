using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class InventoryModel
    {
        public InventorySlotModel[] HotBar { get; set; }

        public int SelectedBlock { get; set; }

        public InventoryModel()
        {
            HotBar = new InventorySlotModel[9];

            for (int i = 0; i < HotBar.Length; i++)
            {
                HotBar[i] = new InventorySlotModel();
            }

            SelectedBlock = 0;
        }
    }
}
