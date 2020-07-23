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

        private int selectedBlock;

        public int SelectedBlock 
        { 
            get
            {
                return selectedBlock;
            }
            set
            {
                if(value > HotBar.Length - 1)
                {
                    selectedBlock = 0;
                }
                else if (value < 0)
                {
                    selectedBlock = HotBar.Length - 1;
                }
                else
                {
                    selectedBlock = value;
                }
            }
        }

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
